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

        public string AddShip(ShipEntity ship)
        {
            string id = Utils.GenerateRandomId();
            if (!_ships.TryAdd(id, ship))
            {
                Console.WriteLine("Warning: Failed to add ship with id " + id);
            }

            Console.WriteLine("Ship count: " + _ships.Count);

            return id;
        }

        public void DeleteShip(string id)
        {
            _ships.Remove(id);
        }

    }
}