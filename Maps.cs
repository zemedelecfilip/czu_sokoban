//vytvoøení všech objektù na základì mapy
using System.Globalization;
using System.Security;

public class Maps
{
    // Constants for grid size
    public const int Size = 50;
    public const int Width = 8;
    public const int Height = 8;
    public List<Box> boxes;
    public List<Wall> walls;
    public List<FinalDestination> finalDest;
    public Player player;
    // 1 = wall, 0 = empty, 3 = player, 5 = box
    public int[,] MapGrid = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 3, 0, 0, 0, 0, 0, 1},
        {1, 0, 1, 4, 1, 1, 0, 1},
        {1, 1, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 1, 4, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 1},
        {1, 5, 0, 0, 1, 1, 5, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };


    // Constructor
    public Maps()
    {
        boxes = new List<Box>();
        walls = new List<Wall>();
        finalDest = new List<FinalDestination>();
    }
    public void AddBox(int x, int y) //FIXME add picture path
    {
        Box newBox = new Box(x, y);
        boxes.Add(newBox);
    }
    public void AddWall(int x, int y) //FIXME add picture path
    {
        Wall newBox = new Wall(x, y);
        walls.Add(newBox);
    }

    public void AddPlayer(int x, int y) //FIXME add picture path
    {
        Player player = new Player(x, y);
        this.player = player;
    }

    public void AddFinalDestination(int x, int y) //FIXME add picture path
    {
        FinalDestination newBox = new FinalDestination(x, y);
        finalDest.Add(newBox);
    }

    public void AddToForm(Form form)
    {
        foreach (var box in boxes)
        {
            form.Controls.Add(box);
        }
        foreach (var box in walls)
        {
            form.Controls.Add(box);
        }
        foreach (var box in finalDest)
        {
            form.Controls.Add(box);
        }
        form.Controls.Add(player);
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
    //method for checking collision between player and box
    //return value is box so it can be moved in main
    public Box collided_pb(Player player, List<Box> boxes)
    {
        if (player == null)
        {
            return null;
        }
        foreach (var box in boxes)
        {
            if (player.gridPos() == box.gridPos())
            {
                return box;
            }
        }
        return null;
    }
    //method for collision between box and box
    //return value is box so it can be moved in main
    //need for one more condition because box always collide with itself(same pos)

    public Box collided_bb(Box bob, List<Box> boxes)
    { 
        if (bob == null)
        {
            return null;
        }
        foreach (var box in boxes)
        {
            if (bob.gridPos() == box.gridPos() && bob != box)
            {
                return box;
            }
        }
        return null;
    }
    //method for collision between box and wall
    //return value is box so it can be moved in main
    public Box collided_bw(Box box, List<Wall> walls)
    {
        if (box == null)
        {
            return null;
        }
        foreach (var wall in walls)
        {
            if (box.gridPos() == wall.gridPos())
            {
                return box;
            }
        }
        return null;
    }

    //method for collision between player and wall
    //return value is true or false - no need for exact object to be returned
    public bool collided_pw(Player player, List<Wall> walls)
    {
        if (player == null)
        {
            return false;
        }
        foreach (var wall in walls)
        {
            if (player.gridPos() == wall.gridPos())
            {
                return true;
            }
        }
        return false;
    }

    public bool checkWin(List<Box> boxes, List<FinalDestination> finalDest)
    {
        int dests = finalDest.Count;
        foreach (var box in boxes)
        {
            foreach (var dest in finalDest)
            {
                if (box.gridPos() == dest.gridPos())
                {
                    dests--;
                }
            }
        }
        return dests == 0 ? true : false;
    }
}
