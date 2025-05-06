using AsteroidsServer.Test.Messages.Message;

namespace AsteroidsServer.Test;

public class RunTests
{
    public static void Run()
    {
        MessageUtilsTest.Test();
    }
}

public class TestingException(string msg) : Exception(msg) { }