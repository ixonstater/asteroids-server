using AsteroidsServer.Src.TrackedEntities;

namespace AsteroidsServer.Src
{
    public class GameState
    {
        private readonly Dictionary<string, ShipEntity> _ships = [];

        public Dictionary<string, ShipEntity> Ships
        {
            get => _ships;
        }

        public ShipEntity? GetShip(string id)
        {
            if (!_ships.TryGetValue(id, out ShipEntity? value))
            {
                return null;
            }

            return value;
        }

        public void AddShip(string shipId, ShipEntity ship)
        {
            if (!_ships.TryAdd(shipId, ship))
            {
                Console.WriteLine("Warning: Failed to add ship with id " + shipId);
            }
        }

        public void DeleteShip(string id)
        {
            _ships.Remove(id);
        }

    }
}