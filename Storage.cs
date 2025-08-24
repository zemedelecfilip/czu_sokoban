public class Storage
{
    //menu, levels, level, stats
    public static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
    public static int screenHeight = Screen.PrimaryScreen.Bounds.Height;

    //headerpanel - H / 10
    //bottom space - H / 10
    // 8H / 100 - space between panel and empty space at the bottom
    // --> 2H / 25

    //size of the tiles in the game - bit smaller to make the 10x10 playable
    public static int size = screenHeight * 2 / 25;

    public static int playerSpeed = size;
    

    public const int gridSize = 8;
    public static int leftMargin = screenWidth / 2 - (gridSize * size) / 2;
    public static int topMargin = screenHeight / 2 - (gridSize * size) / 2;

    public const int gridSize2 = 10;
    public static int leftMargin2 = screenWidth / 2 - (gridSize2 * size) / 2;
    public static int topMargin2 = screenHeight / 2 - (gridSize2 * size) / 2;

    public static string selectedWall = "Wall_Black.png";
    public static string selectedBox = "Crate_Blue.png";
    public static string selectedEndPoint = "EndPoint_Purple.png";
    public static string selectedTextures = "Ground_Concrete.png";

    //public static int leftMargin2 = screenWidth / 2 - (gridSize2 * size) / 2;
    //public static int topMargin2 = screenHeight / 2 - (gridSize2 * size) / 2;

    //Normal - not in the middle, StretchImage - full win, mid, decent, CenterImage - just center, Zoom - full win, mid
    public static PictureBoxSizeMode sizeMode = PictureBoxSizeMode.StretchImage;
    public static System.Drawing.Color Transparent { get; }

    // PictureBoxSizeMode.Zoom;
    // default: wall - black, player - red, box - blue / darkblue, final destination - purple, 
    // Bitmap redSquare = CreateColoredSquare(64, Color.Red);
    public static Image getImage(string filePath)
    {
        //Console.WriteLine($"Loading image from: {filePath}");
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\..\Textures\{filePath}");
        try
        {
            if (!File.Exists(path))
            {
                if (filePath.Contains("Wall"))
                {
                    Bitmap defSquare = CreateColoredSquare(size, Color.Black);
                    return defSquare;
                }
                else if (filePath.Contains("Character"))
                {
                    Bitmap defSquare = CreateColoredSquare(size, Color.Red);
                    return defSquare;
                }
                else if (filePath.Contains("Crate"))
                {
                    Bitmap defSquare = CreateColoredSquare(size, Color.Blue);
                    return defSquare;
                }
                else if (filePath.Contains("EndPoint"))
                {
                    Bitmap defSquare = CreateColoredSquare(size, Color.Purple);
                    return defSquare;
                }
                else if (filePath.Contains("Ground"))
                {
                    Bitmap defSquare = CreateColoredSquare(size, Color.Gray);
                    return defSquare;
                }
            }
            Image image = Image.FromFile(path);
            return image;

        }
        catch (FileNotFoundException ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

    }
    public static Bitmap CreateColoredSquare(int size, Color color)
    {
        Bitmap bmp = new Bitmap(size, size);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, 0, 0, size, size);
            }
        }
        return bmp;
    }
    public static Point gridPos(int x, int y)
    {
        return new Point(x / size, y / size);
    }
}
