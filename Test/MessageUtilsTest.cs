using AsteroidsServer.Src.Messages.Message;

namespace AsteroidsServer.Test.Messages.Message;

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
        byte[] serialized = [.. MessageUtils.Serialize(first), .. MessageUtils.Serialize(second), .. MessageUtils.Serialize(third)];
        // TODO: Finish serializing here with type and terminator
        ReadableMessage msg = MessageUtils.Deserialize(serialized);
        MessageSegment<string>[] msgSeg = [.. msg.StringMessages];
        if (msgSeg[0].Data != first || msgSeg[1].Data != second || msgSeg[2].Data != third)
        {
            throw new TestingException("String message serialization / deserialization failed.");
        }
    }
}