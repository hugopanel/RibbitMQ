namespace RibbitMQ;

public class Message<TMessage>
{
    public TMessage MessageType { get; set; }
    public object? Content { get; set; }
    public object? From { get; set; }
    public object? To { get; set; }
    public SendType SendType { get; set; } = SendType.All;
}