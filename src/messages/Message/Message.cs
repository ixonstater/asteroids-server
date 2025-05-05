namespace AsteroidsServer.Src.Messages.Message;

public class ReadableMessage
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

    public ReadableMessage()
    {
        _stringMessages = new();
        _floatMessages = new();
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

class WriteableMessage
{
    protected byte[] Serialize()
    {
        throw new NotImplementedException();
    }
}