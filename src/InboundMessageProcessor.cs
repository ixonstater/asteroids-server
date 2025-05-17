using AsteroidsServer.Src.Messages;
using AsteroidsServer.Src.Messages.Message;
using AsteroidsServer.Src.Messages.Ship;
using AsteroidsServer.Src.TrackedEntities;

namespace AsteroidsServer.Src
{
    public class InboundMessageProcessor(GameState gameState)
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
                MessageTypeCodes.inboundShip => ProcessInboundShipMessage(msg),
                _ => null,
            };
        }

        private GenericMessage? ProcessJoinMessage(GenericMessage msg)
        {
            Join join = (Join)new Join().FromRequest(msg);
            ShipEntity ship = new()
            {
                color = join.shipColor
            };
            join.id = gameState.AddShip(ship);
            return join.ToResponse();
        }

        private GenericMessage? ProcessInboundShipMessage(GenericMessage msg)
        {
            InboundShipMessage shipMsg = (InboundShipMessage)new InboundShipMessage().FromRequest(msg);
            ShipEntity? ship = gameState.GetShip(shipMsg.id);

            if (ship == null)
            {
                Console.WriteLine("Warning: Tried to get null ship entity with id: " + shipMsg.id);
                return null;
            }

            ship.position.x = shipMsg.position.x;
            ship.position.y = shipMsg.position.y;
            ship.rotation.x = shipMsg.position.x;
            ship.rotation.y = shipMsg.rotation.y;

            return null;
        }
    }
}