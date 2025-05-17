namespace AsteroidsServer.Src.TrackedEntities
{
    public class ShipEntity
    {
        public const string SHIP_RED = "R";
        public const string SHIP_GREEN = "G";
        public const string SHIP_FUCHSIA = "F";
        public const string SHIP_PINK = "P";
        public const string SHIP_YELLOW = "Y";
        public const string SHIP_BLUE = "B";

        public ShipColor color = ShipColor.RED;
        public Position position = new();
        public Rotation rotation = new();

        public static ShipColor ShipColorFromCode(string code)
        {
            return code switch
            {
                SHIP_RED => ShipColor.RED,
                SHIP_GREEN => ShipColor.GREEN,
                SHIP_FUCHSIA => ShipColor.FUCHSIA,
                SHIP_PINK => ShipColor.PINK,
                SHIP_YELLOW => ShipColor.YELLOW,
                SHIP_BLUE => ShipColor.BLUE,
                _ => ShipColor.RED
            };
        }
    }

    public enum ShipColor
    {
        RED,
        GREEN,
        FUCHSIA,
        PINK,
        YELLOW,
        BLUE
    }
}