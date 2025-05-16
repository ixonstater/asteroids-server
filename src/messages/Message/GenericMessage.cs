namespace AsteroidsServer.Src.Messages.Message;

public class GenericMessage
{
    private readonly LinkedList<MessageSegment<string>> _stringMessages;
    public LinkedList<MessageSegment<string>> StringMessages
    {
        get => _stringMessages;
    }

    private readonly LinkedList<MessageSegment<float>> _floatMessages;
    public LinkedList<MessageSegment<float>> FloatMessages
    {
        get => _floatMessages;
    }

    public GenericMessage()
    {
        _stringMessages = new();
        _floatMessages = new();
    }

    public GenericMessage(LinkedList<MessageSegment<string>>? _stringMessages, LinkedList<MessageSegment<float>>? _floatMessages)
    {
        this._stringMessages = _stringMessages ?? new();
        this._floatMessages = _floatMessages ?? new();
    }
}

public enum MessageSegmentType
{
    FLOAT = 0x0,
    STRING = 0x1
}

public class MessageSegment<T>(T data)
{
    private readonly T _data = data;

    public T Data
    {
        get => _data;
    }
}