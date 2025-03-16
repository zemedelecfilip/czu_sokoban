//vytvoøení všech objektù na základì mapy
using System.Security;

public class Maps
{
    // Constants for grid size
    public const int Size = 50;
    public const int Width = 5;
    public const int Height = 5;
    public List<PictureBox> pictureBoxes = new List<PictureBox>();
    public PictureBox Wall = new PictureBox()
    {
        Size = new Size(Size, Size),
        BackColor = Color.Black
    };

    // 1 = wall, 0 = empty, 3 = player, 4+ = box
    public int[,] MapGrid = new int[Height, Width]
    {
        {1, 1, 1, 1, 1},
        {1, 3, 0, 5, 1},
        {1, 0, 4, 4, 1},
        {1, 5, 0, 0, 1},
        {1, 1, 1, 1, 1}
    };

    // Constructor
    public Maps()
    {
        pictureBoxes = new List<PictureBox>();
    }
    public void AddBox(int x, int y) //FIXME add picture path
    {
        Box newBox = new Box(x, y);
        pictureBoxes.Add(newBox);
    }
    public void AddWall(int x, int y) //FIXME add picture path
    {
        Wall newBox = new Wall(x, y);
        pictureBoxes.Add(newBox);
    }

    public void AddPlayer(int x, int y) //FIXME add picture path
    {
        Player newBox = new Player(x, y);
        pictureBoxes.Add(newBox);
    }

    public void AddFinalDestination(int x, int y) //FIXME add picture path
    {
        FinalDestination newBox = new FinalDestination(x, y);
        pictureBoxes.Add(newBox);
    }

    public void AddToForm(Form form)
    {
        foreach (var box in pictureBoxes)
        {
            form.Controls.Add(box);
        }
    }

    public void drawMap(int [,] MapGrid)
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                // 1 = wall, 0 = empty, 3 = player, 4 = box, 5 - final destination for the box
                switch (MapGrid[i, j])
                {
                    case 1:
                        AddWall((j * Size), (i * Size));
                        break;
                    case 3:
                        AddPlayer((j * Size), (i * Size)); 
                        break;
                    
                    case 4:
                        AddBox((j * Size), (i * Size));
                        break;

                    case 5:
                        AddFinalDestination((j * Size), (i * Size));
                        break;
                    default:
                        // Empty
                        break;
                }
            }
        }
    }
    
}
