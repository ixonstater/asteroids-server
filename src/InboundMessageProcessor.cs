using AsteroidsServer.Src.Messages;
using AsteroidsServer.Src.Messages.Message;
using AsteroidsServer.Src.TrackedEntities;

namespace AsteroidsServer.Src
{
    class InboundMessageProcessor(GameState gameState)
    {
        private readonly GameState gameState = gameState;

        public GenericMessage? ProcessMessage(GenericMessage msg)
        {
            if (msg.StringMessages.First == null)
            {
                return null;
            }

            string messageCode = msg.StringMessages.First.Value.Data;

            return messageCode switch
            {
                MessageTypeCodes.join => ProcessJoinMessage(msg),
                _ => null,
            };
        }

        private GenericMessage? ProcessJoinMessage(GenericMessage msg)
        {
            Join join = (Join)new Join().FromRequest(msg);
            Ship ship = new()
            {
                color = join.shipColor
            };
            join.id = gameState.AddShip(ship);
            return join.ToResponse();
        }
    }
}