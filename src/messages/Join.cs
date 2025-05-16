using AsteroidsServer.Src.Messages.Message;
using AsteroidsServer.Src.TrackedEntities;

namespace AsteroidsServer.Src.Messages
{
    /// <summary>
    /// Join Session Message
    /// Required Inputs: Ship color
    /// Required Outputs: Ship id
    /// Message Format: ShipColor(string)
    /// </summary>
    public class Join : IMessage
    {
        public ShipColor shipColor;
        public string id = "";
        public IMessage FromRequest(GenericMessage message)
        {
            int colorIndex = 0;
            int breakIndex = 0;
            var current = message.StringMessages.First;
            int segmentCount = 0;

            while (current?.Next != null && segmentCount <= breakIndex)
            {
                if (segmentCount == colorIndex)
                {
                    shipColor = Ship.ShipColorFromCode(current.Value.Data);
                }
                segmentCount++;
            }

            return this;
        }

        public GenericMessage ToResponse()
        {
            LinkedList<MessageSegment<string>> stringSegments = new();
            stringSegments.AddLast(new MessageSegment<string>(MessageTypeCodes.join));
            stringSegments.AddLast(new MessageSegment<string>(id));

            return new(stringSegments, null);
        }
    }
}