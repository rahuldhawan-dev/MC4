namespace MapCallImporter.SampleValues
{
    public struct AberdeenMonmouthNJStreets
    {
        public struct IdlewildLane
        {
            public const int ID = 2962;
            public const string NAME = "IDLEWILD L";
            public const int SUFFIX_ID = StreetSuffixes.Lane.ID;
        }

        public struct ChurchStreet
        {
            public const int ID = 4733;
            public const string NAME = "CHURCH ST";
            public const int SUFFIX_ID = StreetSuffixes.Street.ID;
        }

        public struct SouthAtlanticAvenue
        {
            public const int ID = 6453;
            public const string NAME = "S ATLANTIC AVE";
            public const int PREFIX_ID = StreetPrefixes.South.ID;
            public const int SUFFIX_ID = StreetSuffixes.Avenue.ID;
        }
    }
}