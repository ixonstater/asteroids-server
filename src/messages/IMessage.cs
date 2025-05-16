using AsteroidsServer.Src.Messages.Message;

namespace AsteroidsServer.Src.Messages
{
    public interface IMessage
    {
        public IMessage FromRequest(GenericMessage message);

        public GenericMessage ToResponse();
    }

    public class MessageTypeCodes
    {
        public const string join = "j";
    }
}