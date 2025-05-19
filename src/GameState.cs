using AsteroidsServer.Src.TrackedEntities;

namespace AsteroidsServer.Src
{
    public class GameState
    {
        private readonly Dictionary<string, ShipEntity> _ships = [];

        public ShipEntity[] Ships
        {
            get => [.. _ships.Values];
        }

        public ShipEntity? GetShip(string id)
        {
            if (!_ships.TryGetValue(id, out ShipEntity? value))
            {
                return null;
            }

            return value;
        }

        public string AddShip(ShipEntity ship)
        {
            string id = Utils.GenerateRandomId();
            Console.WriteLine("ShipId: " + id);
            if (!_ships.TryAdd(id, ship))
            {
                Console.WriteLine("Warning: Failed to add ship with id " + id);
            }

            return id;
        }

        public void DeleteShip(string id)
        {
            _ships.Remove(id);
        }

    }
}