using System.Text;

namespace AsteroidsServer.Src.Messages.Message;

// Basic format for messages is (type | message | terminator)...
// Natural terminator and escape characters must be escaped
public class MessageUtils
{
    private readonly static byte terminator = Encoding.ASCII.GetBytes(";")[0];
    private readonly static byte escape = Encoding.ASCII.GetBytes("\\")[0];

    /// <summary>
    /// Messages are always serialized in a consistent format, string data first, float data second
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] Serialize(Message msg)
    {
        IEnumerable<byte> serialMsg = new LinkedList<byte>();

        foreach (var data in msg.StringMessages)
        {
            serialMsg = serialMsg.Concat(Serialize(data.Data));
        }

        foreach (var data in msg.FloatMessages)
        {
            serialMsg = serialMsg.Concat(Serialize(data.Data));
        }

        return [.. serialMsg];
    }

    /// <summary>
    /// Don't pass non-ascii characters here, you have been warned :)
    /// </summary>
    /// <param name="str">The string to serialize...</param>
    /// <returns></returns>
    private static LinkedList<byte> Serialize(string str)
    {
        var bytes = EscapeSpecialCharacters(Encoding.ASCII.GetBytes(str));
        bytes.AddFirst((byte)MessageSegmentType.STRING);
        bytes.AddLast(terminator);

        return bytes;
    }

    private static LinkedList<byte> Serialize(float num)
    {
        var bytes = EscapeSpecialCharacters(BitConverter.GetBytes(num));
        bytes.AddFirst((byte)MessageSegmentType.FLOAT);
        bytes.AddLast(terminator);

        return bytes;
    }

    private static LinkedList<byte> EscapeSpecialCharacters(byte[] bytes)
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

    public static Message Deserialize(byte[] msg)
    {
        Message message = new();
        MessageSegmentType? processingType = null;
        bool escapeActive = false;
        // I reckon 1kB is gud enuff fer now ¯\_(ツ)_/¯
        byte[] buffer = new byte[1000];
        int bufferCount = 0;

        foreach (var letter in msg)
        {
            if (processingType == null)
            {
                processingType = GetMessageTypeFromByte(letter);
                continue;
            }

            if (!escapeActive && letter == escape)
            {
                escapeActive = true;
                continue;
            }

            if (!escapeActive && letter == terminator)
            {
                AddDeserializedSegmentToMessage(message, (MessageSegmentType)processingType, buffer, bufferCount);
                bufferCount = 0;
                processingType = null;
                continue;
            }

            buffer[bufferCount++] = letter;

            escapeActive = false;
        }

        return message;
    }

    private static void AddDeserializedSegmentToMessage(Message msg, MessageSegmentType type, byte[] bytes, int byteCount)
    {
        if (type == MessageSegmentType.STRING)
        {
            msg.StringMessages.AddLast(new MessageSegment<string>(Encoding.ASCII.GetString(bytes, 0, byteCount)));
        }
        else if (type == MessageSegmentType.FLOAT)
        {
            msg.FloatMessages.AddLast(new MessageSegment<float>(BitConverter.ToSingle(bytes, 0)));
        }
    }

    private static MessageSegmentType GetMessageTypeFromByte(byte type)
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