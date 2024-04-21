namespace EnergyEfficiencyBE
{
    public struct Constants
    {
        public const string SuperAdminName = "SuperAdmin";
        public const string SuperAdminEmail = "admin@mail.com";
        public const string SuperAdminPassword = "Admin1!";
        public const ulong AccessTokenLifetimeInMinutes = 60 * 24 * 3;
        public const ulong RefreshTokenLifetimeInMinutes = 60 * 6;

        public struct EnergyEfficiencyClass
        {
            public const string a = "a";
            public const string b = "b";
            public const string c = "c";
            public const string d = "d";
            public const string e = "e";
            public const string f = "f";
        };

        public struct BuildingType
        {
            public const string Private = "Private";
            public const int PrivateStoriesClasses = 4;

            public struct Public {
                public const string Common = "Common";
                public const int CommonStoriesClasses = 3;
                public const string Hotel = "Hotel";
                public const string Educational = "Educational";
                public const string Preschool = "Preschool";
                public const string Healthcare = "Healthcare";
                public const string Trading = "Trading";
            }
        };

        public enum TemperatureZone
        {
            First,
            Second
        }
    }
}
