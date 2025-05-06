using AsteroidsServer.Src.Messages.Message;

namespace AsteroidsServer.Test;

class MessageUtilsTest
{

    public static void Test()
    {
        TestStrings();
        TestFloats();
        TestFloatsWithStrings();
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

    private static void TestFloats()
    {
        float first = 100.0010F;
        float second = 0.000003f;
        float third = 43;

        LinkedList<MessageSegment<float>> segments = new([new(first), new(second), new(third)]);
        Message original = new(null, segments);
        byte[] serialized = MessageUtils.Serialize(original);

        Message msg = MessageUtils.Deserialize(serialized);
        MessageSegment<float>[] msgSeg = [.. msg.FloatMessages];

        if (msgSeg[0].Data != first || msgSeg[1].Data != second || msgSeg[2].Data != third)
        {
            throw new TestingException("Float message serialization / deserialization failed.");
        }
    }

    private static void TestFloatsWithStrings()
    {
        float first = 100.0010F;
        float second = 0.000003f;
        float third = 43;
        LinkedList<MessageSegment<float>> segments = new([new(first), new(second), new(third)]);


        string firstStr = "A test string with natural escapes\\ and \\;terminators;";
        string secondStr = "\\; another test; \\\\";
        string thirdStr = "a final;\\;\\;;;";
        LinkedList<MessageSegment<string>> segmentsStr = new([new(firstStr), new(secondStr), new(thirdStr)]);

        Message original = new(segmentsStr, segments);
        byte[] serialized = MessageUtils.Serialize(original);

        Message msg = MessageUtils.Deserialize(serialized);
        MessageSegment<string>[] msgSegStr = [.. msg.StringMessages];
        MessageSegment<float>[] msgSeg = [.. msg.FloatMessages];

        if (msgSeg[0].Data != first || msgSeg[1].Data != second || msgSeg[2].Data != third)
        {
            throw new TestingException("Combined message serialization / deserialization failed float check.");
        }

        if (msgSegStr[0].Data != firstStr || msgSegStr[1].Data != secondStr || msgSegStr[2].Data != thirdStr)
        {
            throw new TestingException("Combined message serialization / deserialization failed string check.");
        }
    }
}