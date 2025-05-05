using System.Text;

namespace AsteroidsServer.Src.Messages.Message;

// Basic format for messages is (type | message | terminator)...
// Natural terminator and escape characters must be escaped
public class MessageUtils
{
    public readonly static byte terminator = Encoding.ASCII.GetBytes(";")[0];
    public readonly static byte escape = Encoding.ASCII.GetBytes("\\")[0];

    /**
    * Don't pass non-ascii characters here, you have been warned :)
    */
    public static byte[] Serialize(string str)
    {
        var bytes = EscapeSpecialCharacters(Encoding.ASCII.GetBytes(str));
        bytes.AddFirst((byte)MessageSegmentType.STRING);

        return [.. bytes];
    }

    public static byte[] Serialize(float num)
    {
        var bytes = EscapeSpecialCharacters(BitConverter.GetBytes(num));
        bytes.AddFirst((byte)MessageSegmentType.FLOAT);

        return [.. bytes];
    }

    public static LinkedList<byte> EscapeSpecialCharacters(byte[] bytes)
    {
        LinkedList<byte> escapedData = new();
        foreach (byte letter in bytes)
        {
            if (letter == escape || letter == terminator)
            {
                escapedData.AddLast(escape);
                escapedData.AddLast(letter);
            }
            else
            {
                escapedData.AddLast(letter);
            }
        }

        return escapedData;
    }

    public static ReadableMessage Deserialize(byte[] msg)
    {
        ReadableMessage message = new();
        MessageSegmentType? processingType = null;
        bool escapeActive = false;
        // I reckon 1kB is gud enuff fer now ¯\_(ツ)_/¯
        byte[] buffer = new byte[1000];
        int bufferCount = 0;

        foreach (var letter in msg)
        {
            processingType ??= GetMessageTypeFromByte(letter);

            if (!escapeActive && letter == escape)
            {
                escapeActive = true;
                continue;
            }

            if (!escapeActive && letter == terminator)
            {
                AddSegmentToReadableMessage(message, (MessageSegmentType)processingType, buffer, bufferCount);
                bufferCount = 0;
            }

            buffer[++bufferCount] = letter;

            escapeActive = false;
        }

        return message;
    }

    public static void AddSegmentToReadableMessage(ReadableMessage msg, MessageSegmentType type, byte[] bytes, int byteCount)
    {
        if (type == MessageSegmentType.STRING)
        {
            msg.StringMessages.AddLast(new MessageSegment<string>(Encoding.ASCII.GetString(bytes, 0, byteCount - 1)));
        }
        else if (type == MessageSegmentType.FLOAT)
        {
            msg.FloatMessages.AddLast(new MessageSegment<float>(BitConverter.ToSingle(bytes, 0)));
        }
    }

    public static MessageSegmentType GetMessageTypeFromByte(byte type)
    {
        if (type == (int)MessageSegmentType.FLOAT)
        {
            return MessageSegmentType.FLOAT;
        }
        else if (type == (int)MessageSegmentType.STRING)
        {
            return MessageSegmentType.STRING;
        }

        throw new MessageParseException("Failed to parse data type from message byte: " + type);
    }
}

[Serializable]
class MessageParseException(string msg) : Exception(msg) { }