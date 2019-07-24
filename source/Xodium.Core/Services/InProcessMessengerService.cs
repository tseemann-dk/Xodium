using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace Xodium.Services
{
    public interface ILogger
    {
        void LogText(string text);
    }

    public class InProcessMessengerService : IMessengerService, IDisposable
    {
        private TimeSpan? cleanupInterval;
        private readonly Dictionary<Type, Channel> channels = new Dictionary<Type, Channel>();

        #region Construction/Disposal

        public InProcessMessengerService(TimeSpan? cleanupInterval = null)
        {
            LoggingLevel = LoggingLevel.Normal;
            this.cleanupInterval = cleanupInterval;
            StartCleanup();
        }

        ~InProcessMessengerService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            StopCleanup();
        }

        #endregion

        #region Subscription Management

        private Channel FindChannel(Type messageType)
        {
            lock (channels)
            {
                channels.TryGetValue(messageType, out Channel channel);
                return channel;
            }
        }

        public IDisposable Subscribe<TMessage>(object recipient, MessageHandler<TMessage> handler)
        {
            return Subscribe(typeof(TMessage), recipient, (sender, message) => handler(sender, (TMessage)message));
        }

        public IDisposable Subscribe(Type messageType, object recipient, MessageHandler<object> handler)
        {
            return AddSubscription(messageType, recipient, handler);
        }

        private Subscription AddSubscription(Type messageType, object recipient, MessageHandler<object> handler)
        {
            lock (channels)
            {
                var channel = FindChannel(messageType) ?? (channels[messageType] = new Channel(this, messageType));
                return channel.AddSubscription(recipient, handler);
            }
        }

        public void Unsubscribe<TMessage>(object recipient)
        {
            Unsubscribe(typeof(TMessage), recipient);
        }

        public void Unsubscribe(Type messageType, object recipient)
        {
            var channel = FindChannel(messageType);

            if (channel == null)
                throw new ArgumentException($"Channel not found: {messageType.FullName}", nameof(messageType));

            channel.RemoveSubscription(recipient);
        }

        #endregion

        #region Publishing

        public void Publish<TMessage>(object sender, TMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var messageType = typeof(TMessage);

            // Publish to channels for message type and all base types
            while (messageType != null)
            {
                var channel = FindChannel(messageType);
                channel?.Publish(sender, message);
                messageType = messageType.GetTypeInfo().BaseType;
            }

            var typeInfo = typeof(TMessage).GetTypeInfo();

            // Publish to channels for all implemented interfaces
            foreach (var implementedInterface in typeInfo.ImplementedInterfaces)
            {
                var channel = FindChannel(implementedInterface);
                channel?.Publish(sender, message);
            }
        }

        #endregion

        #region Logging

        public LoggingLevel LoggingLevel { get; set; }
        public ILogger Logger { get; set; }

        internal void WriteLogEntry(string entry)
        {
            if (LoggingLevel == LoggingLevel.None) return;
            var text = $"{GetType().Name}: {entry}";
            Logger?.LogText(text);
            Debug.WriteLine(text);
        }

        #endregion

        #region Cleanup

        private CancellationTokenSource cleanupCanceller;

        private void StartCleanup()
        {
            if (!cleanupInterval.HasValue) return;
            var cts = new CancellationTokenSource();
            cleanupCanceller = cts;
            Task.Run(() => CleanupLoop(cleanupInterval.Value, cts.Token), cts.Token);
        }

        private void StopCleanup()
        {
            if (cleanupCanceller == null) return;
            cleanupCanceller.Cancel();
            cleanupCanceller = null;
        }

        private void CleanupLoop(TimeSpan interval, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Task.Delay(interval, cancellationToken).Wait(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                Cleanup();
            }
        }

        private void Cleanup()
        {
            RemoveInvalidSubscriptions();
        }

        public void RemoveInvalidSubscriptions()
        {
            WriteLogEntry("Removing invalid subscriptions ...");

            int count = 0;

            lock (channels)
            {
                foreach (var channel in channels.Values.ToArray())
                {
                    count += channel.RemoveInvalidSubscriptions();
                }
            }

            WriteLogEntry($"Removed {count} subscriptions");
        }

        #endregion
    }

    internal class Subscription : IDisposable
    {
        private readonly WeakReference recipient;

        #region Construction/Disposal

        public Subscription(Channel channel, object recipient, MessageHandler<object> handler)
        {
            this.recipient = new WeakReference(recipient);
            Channel = channel;
            Handler = handler;
        }

        ~Subscription()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && IsValid)
            {
                Channel.RemoveSubscription(Recipient);
            }
        }

        #endregion

        public object Recipient => recipient.Target;
        public bool IsValid => recipient.IsAlive;

        public Channel Channel { get; }
        public MessageHandler<object> Handler { get; }
    }

    internal class Channel
    {
        private readonly InProcessMessengerService messenger;
        private readonly Dictionary<WeakReference, Subscription> subscriptions = new Dictionary<WeakReference, Subscription>(); 

        public Channel(InProcessMessengerService messenger, Type messageType)
        {
            this.messenger = messenger;
            MessageType = messageType;
        }

        public Type MessageType { get; }

        #region Add/Remove Subscription

        public Subscription AddSubscription(object recipient, MessageHandler<object> handler)
        {
            var key = subscriptions.Keys.FirstOrDefault(k => k.Target == recipient);
            
            if (key == null)
            {
                key = new WeakReference(recipient);
                WriteLogEntry($"Adding subscriber {GetRecipientLogInfo(recipient)} to channel {GetChannelLogInfo()}");
            }
            else
            {
                WriteLogEntry($"Replacing subscriber {GetRecipientLogInfo(recipient)} on channel {GetChannelLogInfo()}");
            }

            return subscriptions[key] = new Subscription(this, recipient, handler);
        }

        public void RemoveSubscription(object recipient)
        {
            if (recipient == null) return;

            var key = subscriptions.Keys.FirstOrDefault(k => k.Target == recipient);

            if (key != null)
            {
                WriteLogEntry($"Removing subscriber {GetRecipientLogInfo(recipient)} from channel {GetChannelLogInfo()}");
                subscriptions.Remove(key);
            }
        }

        private void RemoveSubscription(Subscription subscription)
        {
            if (subscription.Recipient != null)
            {
                RemoveSubscription(subscription.Recipient);
                return;
            }

            var key = subscriptions.Where(kv => kv.Value == subscription).Select(kv => kv.Key).FirstOrDefault();

            if (key != null)
            {
                WriteLogEntry($"Removing dead subscription from channel {GetChannelLogInfo()}");
                subscriptions.Remove(key);
            }
        }

        public int RemoveInvalidSubscriptions()
        {
            int count = 0;

            foreach (var subscription in subscriptions.Values.ToArray())
            {
                if (!subscription.IsValid)
                {
                    RemoveSubscription(subscription);
                    count++;
                }
            }

            return count;
        }

        public void ClearSubscriptions()
        {
            subscriptions.Clear();
        }

        #endregion

        #region Publishing

        public void Publish(object sender, object message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (!MessageType.GetTypeInfo().IsAssignableFrom(message.GetType().GetTypeInfo()))
                throw new ArgumentException($"Invalid message type {message.GetType()}. Expected {MessageType} or inherited.", nameof(message));

            foreach (var subscription in subscriptions.Values.ToArray())
            {
                if (subscription.IsValid)
                {
                    WriteLogEntry($"Sending message \"{message}\" to subscriber {GetRecipientLogInfo(subscription.Recipient)}");
                    subscription.Handler(sender, message);
                }
            }
        }

        #endregion

        #region Logging

        private void WriteLogEntry(string text)
        {
            messenger.WriteLogEntry(text);
        }

        private string GetRecipientLogInfo(object recipient)
        {
            return recipient == null ? "(null)" : $"\"{recipient}\"";
        }

        private string GetChannelLogInfo()
        {
            return GetTypeLogInfo(MessageType);
        }

        private string GetTypeLogInfo(Type type)
        {
            if (type == null) return "(null)";
            return messenger.LoggingLevel == LoggingLevel.Verbose ? type.FullName : type.Name;
        }

        #endregion
    }
}
