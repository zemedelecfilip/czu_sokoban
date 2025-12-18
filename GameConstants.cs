namespace czu_sokoban.BusinessLogic
{
    /// <summary>
    /// Contains all game constants to avoid magic numbers in the code.
    /// </summary>
    public static class GameConstants
    {
        public const int DefaultGridSize = 8;
        public const int LargeGridSize = 10;
        public const int NumberOfLevels = 10;
        public const int NumberOfSaves = 3;
        public const int NumberOfRowsInLevelsScreen = 2;
        public const int LevelsPerRow = 5;
        
        public const int MapValueEmpty = 0;
        public const int MapValueWall = 1;
        public const int MapValueOutsideTexture = 2;
        public const int MapValuePlayer = 3;
        public const int MapValueBox = 4;
        public const int MapValueFinalDestination = 5;
        public const int MapValueInsideTexture = 6;
        public const int MapValueFinalDestinationWithWall = 7;
    }
}

