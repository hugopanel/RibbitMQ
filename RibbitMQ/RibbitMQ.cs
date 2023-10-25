namespace RibbitMQ;

public class RibbitMQ<TMessage>
{
    public delegate Task MessageHandler(IMessage<TMessage> message);

    private static Dictionary<TMessage, List<MessageHandler>> _subscribers = new();
    
    public void Send(IMessage<TMessage> message)
    {
        if (_subscribers.Keys.Contains(message.MessageType))
        {
            switch (message.SendType)
            {
                case SendType.FirstFree:
                    Task.Run(async () => await _subscribers[message.MessageType].First()(message));
                    break;
                case SendType.All:
                default:
                    _subscribers[message.MessageType].ForEach(s => Task.Run(async () => await s(message)));
                    break;
            }
        } else {
            Console.WriteLine("Cannot find message type ${0}!", message.MessageType);
        }
    }

    public void Subscribe(TMessage messageType, MessageHandler callback)
    {
        if (!_subscribers.Keys.Contains(messageType))
            _subscribers.Add(messageType, new());
        
        _subscribers[messageType].Add(callback);
    }

    public void Unsubscribe(TMessage messageType, MessageHandler callback)
    {
        if (_subscribers.Keys.Contains(messageType))
            _subscribers[messageType].Remove(callback);
    }
}