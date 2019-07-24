using System;
using System.Collections.Generic;
using System.Threading;
using Xodium.Services;
using Xunit;

namespace Xodium.Core.Tests
{
    #region Helper Classes

    internal interface ITextPayload
    {
        string Text { get; }
    }

    internal class NotificationMessage : ITextPayload
    {
        public NotificationMessage(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public override string ToString() => Text;
    }

    internal class InformationMessage : ITextPayload
    {
        public InformationMessage(string information)
        {
            Information = information;
        }

        string ITextPayload.Text => Information;

        public string Information { get; }
    }

    class Greeting : NotificationMessage
    {
        public Greeting() : base("Hello!")
        {
        }
    }

    internal class Logger : ILogger
    {
        private readonly List<string> log = new List<string>();

        public void LogText(string text)
        {
            log.Add(text);
        }

        public IReadOnlyList<string> Log => log;
    }

    internal class Receiver
    {
        public Receiver(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString() => Name;
    }

    internal class RichReference<T>
        where T : class
    {
        public RichReference(T instance, WeakReference reference)
        {
            Instance = instance;
            Reference = reference;
        }

        public T Instance { get; private set; }
        public WeakReference Reference { get; }

        public void Unreference()
        {
            Instance = null;
        }
    }

    internal class ReceiverReference : RichReference<Receiver>
    {
        public ReceiverReference(Receiver instance, WeakReference reference)
            : base(instance, reference)
        {
        }
    }

    #endregion

    public class InProcessMessengerServiceTest
    {
        private ReceiverReference GetReceiverReferenceFrom(Func<ReceiverReference> func) => func();
        private IReadOnlyList<ReceiverReference> GetReceiverReferencesFrom(Func<IReadOnlyList<ReceiverReference>> func) => func();

        [Fact]
        public void CanSendAndReceiveExactMessage()
        {
            using (var messenger = new InProcessMessengerService())
            {
                const string salute = "Hi!";
                NotificationMessage receivedMessage = null;

                messenger.Subscribe<NotificationMessage>(this, (s, m) => receivedMessage = m);
                messenger.Publish(this, new NotificationMessage(salute));

                Assert.Equal(salute, receivedMessage.Text);
            }
        }

        [Fact]
        public void CanSendAndReceiveDerivedMessage()
        {
            using (var messenger = new InProcessMessengerService())
            {
                NotificationMessage receivedMessage = null;

                messenger.Subscribe<NotificationMessage>(this, (s, m) => receivedMessage = m);
                messenger.Publish(this, new Greeting());

                Assert.IsType<Greeting>(receivedMessage);
            }
        }

        [Fact]
        public void CanReceiveMessagesByInterface()
        {
            using (var messenger = new InProcessMessengerService())
            {
                var informationMessage = new InformationMessage("Information");
                var notificationMessage = new NotificationMessage("Notification");

                object receivedMessage = null;

                messenger.Subscribe<ITextPayload>(this, (s, m) => receivedMessage = m);
                Assert.Null(receivedMessage);

                messenger.Publish(this, "Unknown");
                Assert.Null(receivedMessage);

                messenger.Publish(this, informationMessage);
                Assert.Equal(informationMessage, receivedMessage);

                messenger.Publish(this, notificationMessage);
                Assert.Equal(notificationMessage, receivedMessage);
            }
        }

        [Fact]
        public void CanUnsubscribeViaDisposal()
        {
            using (var messenger = new InProcessMessengerService())
            {
                var hello = new Greeting();
                object receivedMessage = null;

                using (messenger.Subscribe<Greeting>(this, (s, m) => receivedMessage = m))
                {
                    messenger.Publish(this, hello);
                    Assert.Equal(hello, receivedMessage);
                }

                receivedMessage = null;
                messenger.Publish(this, hello);
                Assert.Null(receivedMessage);
            }
        }

        [Fact]
        public void ReceiverIsGarbageCollected()
        {
            var rr = GetReceiverReferenceFrom(() =>
            {
                var receiver = new Receiver("Receiver");
                var reference = new WeakReference(receiver);
                Assert.True(reference.IsAlive);

                return new ReceiverReference(receiver, reference);
            });

            rr.Unreference();
            Assert.True(rr.Reference.IsAlive);
            GC.Collect(0, GCCollectionMode.Forced, true);
            Assert.False(rr.Reference.IsAlive);
        }

        [Fact(Skip = "Work In Progress")]
        public void CleanupIsRemovingInvalidSubscriptions()
        {
            // Start messenger with 50 ms cleanup interval
            var messenger = new InProcessMessengerService(TimeSpan.FromMilliseconds(50));

            // Prepare messages
            var hello = new Greeting();
            object receivedMessage1 = null;
            object receivedMessage2 = null;

            var references = GetReceiverReferencesFrom(() =>
            {
                // Create receivers
                var receiver1 = new Receiver("Receiver 1");
                var receiver2 = new Receiver("Receiver 2");

                // Ensure both receivers are alive
                var ref1 = new WeakReference(receiver1);
                var ref2 = new WeakReference(receiver2);
                Assert.True(ref1.IsAlive);
                Assert.True(ref2.IsAlive);

                // Subscribe and receive message
                messenger.Subscribe<Greeting>(receiver1, (s, m) => receivedMessage1 = m);
                messenger.Subscribe<Greeting>(receiver2, (s, m) => receivedMessage2 = m);
                messenger.Publish(this, hello);
                Assert.Equal(hello, receivedMessage1);
                Assert.Equal(hello, receivedMessage2);

                // Return rich references to both receivers
                return new[] {
                    new ReceiverReference(receiver1, ref1),
                    new ReceiverReference(receiver2, ref2)
                };
            });

            var r1 = references[0];
            var r2 = references[1];

            // Let go of first receiver
            Assert.NotNull(r1.Reference);
            r1.Unreference();
            Assert.Null(r1.Instance);

            // Collect garbage and verify state of both receivers
            GC.Collect(0, GCCollectionMode.Forced, true);
            Assert.False(r1.Reference.IsAlive);
            Assert.True(r2.Reference.IsAlive);

            // Give messenger some time to cleanup
            Thread.Sleep(200);

            // Clear received messages, publish again and verify receival by only the second receiver
            receivedMessage1 = null;
            receivedMessage2 = null;
            messenger.Publish(this, hello);
            Assert.Null(receivedMessage1);
            Assert.Equal(hello, receivedMessage2);

            // Let go of second receiver
            Assert.NotNull(r2.Reference);
            r2.Unreference();
            Assert.Null(r2.Instance);

            // Verify that messages are still received by the second receiver
            receivedMessage1 = null;
            receivedMessage2 = null;
            messenger.Publish(this, hello);
            Assert.Null(receivedMessage1);
            Assert.Equal(hello, receivedMessage2);

            // Collect garbage to get rid of second receiver
            GC.Collect(0, GCCollectionMode.Forced, true);

            // Verify that messages are no longer received by any receiver
            receivedMessage1 = null;
            receivedMessage2 = null;
            messenger.Publish(this, hello);
            Assert.Null(receivedMessage1);
            Assert.Null(receivedMessage2);

            // Dispose messenger
            messenger.Dispose();

            // Collect garbage and verify that both receivers are dead
            GC.Collect(0, GCCollectionMode.Forced, true);
            Assert.False(r1.Reference.IsAlive);
            Assert.False(r2.Reference.IsAlive);
        }

        [Fact]
        public void EventsAreLogged()
        {
            using (var messenger = new InProcessMessengerService())
            {
                var greeting = new Greeting();
                object receivedMessage = null;

                // Assign logger
                var logger = new Logger();
                messenger.Logger = logger;
                Assert.Equal(0, logger.Log.Count);

                // Subscribe
                messenger.Subscribe<Greeting>(this, (s, m) => receivedMessage = m);
                Assert.Equal(1, logger.Log.Count);

                // Publish
                messenger.Publish(this, greeting);
                Assert.Equal(2, logger.Log.Count);
                Assert.Equal(receivedMessage, greeting);

                // Unsubscribe
                messenger.Unsubscribe<Greeting>(this);
                Assert.Equal(3, logger.Log.Count);
            }
        }
    }
}
