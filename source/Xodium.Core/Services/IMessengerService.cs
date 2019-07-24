using System;

namespace Xodium.Services
{
    public enum LoggingLevel { None, Normal, Verbose }

    public delegate void MessageHandler<in TMessage>(object sender, TMessage message);

    public interface IMessengerService
    {
        IDisposable Subscribe<TMessage>(object recipient, MessageHandler<TMessage> handler);
        IDisposable Subscribe(Type messageType, object recipient, MessageHandler<object> handler);

        void Unsubscribe<TMessage>(object recipient);
        void Unsubscribe(Type messageType, object recipient);

        void Publish<TMessage>(object sender, TMessage message);

        LoggingLevel LoggingLevel { get; set; }
    }
}