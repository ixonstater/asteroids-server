using AsteroidsServer.Src.Messages.Message;
using AsteroidsServer.Src;

namespace AsteroidsServer.Src.Messages
{
    /// <summary>
    /// Join Session Message
    /// Required Inputs: Ship color
    /// Required Outputs: Ship id
    /// Message Format: ShipColor(string)
    /// </summary>
    public class Join
    {
        public TrackedEntities.ShipColor shipColor;
        public string id = "";
        public Join FromRequest(GenericMessage message)
        {
            int colorIndex = 1;
            var current = message.StringMessages.First;
            int segmentCount = 0;

            while (current != null)
            {
                if (segmentCount == colorIndex)
                {
                    shipColor = TrackedEntities.ShipEntity.ShipColorFromCode(current.Value.Data);
                }
                current = current.Next;
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