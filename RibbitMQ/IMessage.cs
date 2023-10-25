namespace RibbitMQ;

public interface IMessage<TMessage>
{
    public TMessage MessageType { get; set; }
    
    public SendType? SendType { get; set; }

    public object? Content { get; set; }
}