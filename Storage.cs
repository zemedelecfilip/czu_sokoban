using System.Drawing;
public class Storage
{
    public const int size = 75;
    public const int playerSpeed = size;
    //menu, levels, level, stats
    public static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
    public static int screenHeight = Screen.PrimaryScreen.Bounds.Height;
    public const int gridSize = 8;
    public static int leftMargin = screenWidth / 2 - (gridSize * size) / 2;
    public static int topMargin = screenHeight / 2 - (gridSize * size) / 2;

    // PictureBoxSizeMode.Zoom;
    // default: wall - black, player - red, box - blue / darkblue, final destination - purple, 
    // Bitmap redSquare = CreateColoredSquare(64, Color.Red);
    public static Image getImage(string filePath)
    {
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
            return Image.FromFile(path);

        }
        catch (FileNotFoundException ex)
        {
            //MessageBox.Show($"Error: {ex.Message}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        catch (Exception ex)
        {
            //MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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




/*
public void game_movement(object sender, KeyEventArgs e)
{
    if (e.KeyCode == Keys.Left)
    {
        player.moveLeft();                          //player moved left
        if (map.collided_pw(player, walls))
        {
            player.moveRight(true);                 //player moved back to the original position
        }
        Box a = map.collided_pb(player, boxes);     //check if player collided with box

        if (a != null)                              //yes they collided so they moved
        {
            a.moveLeft();                           //box moved left
            Box b = map.collided_bb(a, boxes);      //check if box collided with box
            Box c = map.collided_bw(a, walls);      //check if box collided with wall
            if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
            {
                a.moveRight();
                player.moveRight(true);
            }

        }
        map.checkWin(boxes, finalDest);
    }
    else if (e.KeyCode == Keys.Right)
    {
        player.moveRight();                         //player moved left
        if (map.collided_pw(player, walls))
        {
            player.moveLeft(true);                  //player moved back to the original position
        }                                           //player moved right
        Box a = map.collided_pb(player, boxes);     //check if player collided with box

        if (a != null)                              //yes they collided so they moved
        {
            a.moveRight();                          //box moved right
            Box b = map.collided_bb(a, boxes);      //check if box collided with box
            Box c = map.collided_bw(a, walls);      //check if box collided with wall
            if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
            {
                a.moveLeft();
                player.moveLeft(true);
            }

        }
        map.checkWin(boxes, finalDest);
    }
    else if (e.KeyCode == Keys.Up)
    {
        player.moveUp();                            //player moved left
        if (map.collided_pw(player, walls))
        {
            player.moveDown(true);                  //player moved back to the original position
        }                                           //player moved up
        Box a = map.collided_pb(player, boxes);     //check if player collided with box

        if (a != null)                              //yes they collided so they moved
        {
            a.moveUp();                             //box moved left
            Box b = map.collided_bb(a, boxes);      //check if box collided with box
            Box c = map.collided_bw(a, walls);      //check if box collided with wall
            if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
            {
                a.moveDown();
                player.moveDown(true);
            }

        }
        map.checkWin(boxes, finalDest);
    }
    else if (e.KeyCode == Keys.Down)
    {
        player.moveDown();                          //player moved left
        if (map.collided_pw(player, walls))
        {
            player.moveUp(true);                    //player moved back to the original position
        }                                           //player moved down
        Box a = map.collided_pb(player, boxes);     //check if player collided with box

        if (a != null)                              //yes they collided so they moved
        {
            a.moveDown();                           //box moved down
            Box b = map.collided_bb(a, boxes);      //check if box collided with box
            Box c = map.collided_bw(a, walls);      //check if box collided with wall
            if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
            {
                a.moveUp();
                player.moveUp(true);
            }
        }
        map.checkWin(boxes, finalDest);
    }
    else if (e.KeyCode == Keys.Escape)
    {
        Application.Exit();
    }

    map.checkWin(boxes, finalDest);
}
*/

/*
    label1.Size = new Size(homeScreenRect.Width / 2, homeScreenRect.Height);
    label1.Font = new Font("Segoe UI", 24, FontStyle.Bold);
    label1.Text = "Sokoban";

    label1.Location = new Point(
        (homeScreenRect.Width - label1.Width) / 2,
        (homeScreenRect.Height - label1.Height) / 2
    );
    label1.Parent = homeScreenRect;
*/

