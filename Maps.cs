public class Maps
{
    // Constants for grid size
    public const int Size = 50;
    public const int Width = 10;
    public const int Height = 10;

    // 2D array representing the map (1 = Wall, 0 = Empty space)
    public int[,] MapGrid = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };

    // Constructor
    public Maps() { }

    /// <summary>
    /// Draws the map by placing walls in the given form.
    /// </summary>
    public void drawMap(Form form)
    {
        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                if (MapGrid[row, col] == 1) // If it's a wall
                {
                    PictureBox wall = new PictureBox
                    {
                        BackColor = Color.Black,
                        Size = new Size(Size, Size),
                        Location = new Point(col * Size, row * Size)
                    };

                    form.Controls.Add(wall); // Add wall to the form
                }
            }
        }
    }
}
