using AsteroidsServer.Src.Messages.Message;
using AsteroidsServer.Src.TrackedEntities;

namespace AsteroidsServer.Src.Messages.Ship
{
    /// <summary>
    /// Join Session Message
    /// Required Outputs: Ship Id, Ship Position, Ship Color
    /// Message Format Strings: [ShipId(string) | ShipColor(string)]
    /// Message Format Floats: [ShipPositionX(float) | ShipPositionY(float) | ShipRotationX(float) | ShipRotationY(float)]
    /// </summary>
    class Ships
    {
        public static GenericMessage ToResponse(Dictionary<string, ShipEntity> ships)
        {
            LinkedList<MessageSegment<string>> shipIds = new();
            LinkedList<MessageSegment<float>> shipPositions = new();

            foreach (var ship in ships)
            {
                shipIds.AddLast(new MessageSegment<string>(MessageTypeCodes.shipsUpdate));
                shipIds.AddLast(new MessageSegment<string>(ship.Key));
                shipIds.AddLast(new MessageSegment<string>(ShipEntity.ColorCodeFromShipColor(ship.Value.color)));

                shipPositions.AddLast(new MessageSegment<float>(ship.Value.position.x));
                shipPositions.AddLast(new MessageSegment<float>(ship.Value.position.y));
                shipPositions.AddLast(new MessageSegment<float>(ship.Value.rotation.x));
                shipPositions.AddLast(new MessageSegment<float>(ship.Value.rotation.y));
            }

            return new(shipIds, shipPositions);
        }
    }
}