using AsteroidsServer.Src.Messages.Message;
using AsteroidsServer.Src.TrackedEntities;

namespace AsteroidsServer.Src.Messages.Ship
{
    class InboundShipMessage : IMessage
    {
        public Rotation rotation = new();
        public Position position = new();
        public string id = "";

        public IMessage FromRequest(GenericMessage message)
        {
            int shipPositionXIndex = 1;
            int shipPositionYIndex = 2;
            int shipRotationXIndex = 3;
            int shipRotationYIndex = 4;

            var currentFloat = message.FloatMessages.First;
            int floatIndex = 0;
            while (currentFloat != null)
            {
                if (floatIndex == shipPositionXIndex)
                {
                    position.x = currentFloat.Value.Data;
                }
                else if (floatIndex == shipPositionYIndex)
                {
                    position.y = currentFloat.Value.Data;
                }
                else if (floatIndex == shipRotationXIndex)
                {
                    rotation.x = currentFloat.Value.Data;
                }
                else if (floatIndex == shipRotationYIndex)
                {
                    rotation.y = currentFloat.Value.Data;
                }
                floatIndex++;
                currentFloat = currentFloat.Next;
            }

            int shipIdIndex = 1;

            var currentString = message.StringMessages.First;
            int stringIndex = 0;
            while (currentString != null)
            {
                if (stringIndex == shipIdIndex)
                {
                    id = currentString.Value.Data;
                }
                stringIndex++;
                currentString = currentString.Next;
            }

            return this;
        }

        public GenericMessage ToResponse()
        {
            throw new NotImplementedException("Ship is an inbound message and should never have a response.");
        }
    }
}