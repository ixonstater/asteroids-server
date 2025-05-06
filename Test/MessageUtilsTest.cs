using AsteroidsServer.Src.Messages.Message;

namespace AsteroidsServer.Test;

class MessageUtilsTest
{

    public static void Test()
    {
        TestStrings();
    }

    private static void TestStrings()
    {
        string first = "A test string with natural escapes\\ and \\;terminators;";
        string second = "\\; another test; \\\\";
        string third = "a final;\\;\\;;;";

        LinkedList<MessageSegment<string>> segments = new([new(first), new(second), new(third)]);
        Message original = new(segments, null);
        byte[] serialized = MessageUtils.Serialize(original);

        Message msg = MessageUtils.Deserialize(serialized);
        MessageSegment<string>[] msgSeg = [.. msg.StringMessages];

        if (msgSeg[0].Data != first || msgSeg[1].Data != second || msgSeg[2].Data != third)
        {
            throw new TestingException("String message serialization / deserialization failed.");
        }
    }
}