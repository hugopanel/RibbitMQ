namespace RibbitMQ;

public class RibbitMQ<TMessage>
{
    public delegate Task MessageHandler(Message<TMessage> message);

    private static Dictionary<TMessage, List<MessageHandler>> _subscribers = new();

    public void Send(TMessage messageType, object? content, object? from = null, object? to = null, SendType sendType = SendType.All)
    {
        Message<TMessage> message = new Message<TMessage>
        {
            MessageType = messageType,
            Content = content,
            From = from,
            To = to,
            SendType = sendType
        };
        if (_subscribers.Keys.Contains(messageType))
        {
            switch (message.SendType)
            {
                case SendType.FirstFree:
                    Task.Run(async () => await _subscribers[messageType].First()(message));
                    break;
                case SendType.All:
                default:
                    _subscribers[messageType].ForEach(s => Task.Run(async () =>  await s(message) ));
                    break;
            }
        }
        else
        {
            Console.WriteLine("Cannot find message type ${0}", messageType);
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