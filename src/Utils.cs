namespace AsteroidsServer.Src
{
    class Utils
    {
        private static readonly Random _random = new();
        private static readonly char[] idChars = [
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            '0','1','2','3','4','5','6','7','8','9'
        ];

        public static string GenerateRandomId(int length = 5)
        {
            char[] id = new char[length];
            for (int i = 0; i < length; i++)
            {
                id[i] = idChars[_random.Next(idChars.Length)];
            }
            return new string(id);
        }
    }
}