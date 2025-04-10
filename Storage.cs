public static class Storage
{
    public const int size = 75;
    public const int playerSpeed = size;
    public static string scene = "menu";
    public static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
    public static int screenHeight = Screen.PrimaryScreen.Bounds.Height;
    public const int gridSize = 8;
    public static int leftMargin = screenWidth / 2 - (gridSize * size) / 2;
    public static int topMargin = screenHeight / 2 - (gridSize * size) / 2;
}