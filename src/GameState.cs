using AsteroidsServer.Src.TrackedEntities;

namespace AsteroidsServer.Src
{
    public class GameState
    {
        private readonly Dictionary<string, Ship> _ships = [];

        public Ship[] Ships
        {
            get => [.. _ships.Values];
        }

        public string AddShip(Ship ship)
        {
            string id = Guid.NewGuid().ToString("n")[5..];
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