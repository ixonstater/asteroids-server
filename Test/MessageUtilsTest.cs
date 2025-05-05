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
        string test = "A test string with natural escapes\\ and \\;terminators \\ \\;";
        byte[] serialized = MessageUtils.Serialize(test);
        // TODO: Finish serializing here with type and terminator
        ReadableMessage msg = MessageUtils.Deserialize(serialized);
    }
}