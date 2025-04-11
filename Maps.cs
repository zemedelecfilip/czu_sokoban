//vytvoøení všech objektù na základì mapy
using czu_sokoban;
using System.ComponentModel;
using System.Globalization;
using System.Security;

public class Maps
{
    // Constants for grid size
    public const int Size = Storage.size;
    public const int Width = Storage.gridSize;
    public const int Height = Storage.gridSize;
    public static int leftMargin = Storage.leftMargin;
    public static int topMargin = Storage.topMargin;
    public List<Box> boxes;
    public List<Wall> walls;
    public List<FinalDestination> finalDest;
    public Player player;

    public int screenWidth = Storage.screenWidth;
    public int screenHeight = Storage.screenHeight;


    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid2 = new int[Height, Width]
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
    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid = new int[Height, Width]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 0, 1, 1, 0, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 3, 4, 0, 5, 0, 1},
        {1, 0, 0, 4, 0, 5, 0, 1},
        {1, 1, 0, 0, 0, 0, 1, 1},
        {1, 1, 1, 0, 0, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };
    //0 = empty, 1 - wall, 3 = player, 4 = box, 5 - final destination
    public int[,] MapGrid1 = new int[Height, Width]
    {
        {0, 1, 1, 1, 1, 1, 0, 0},
        {0, 1, 0, 3, 0, 1, 1, 1},
        {1, 1, 4, 1, 4, 0, 0, 1},
        {1, 0, 5, 5, 0, 5, 0, 1},
        {1, 0, 0, 4, 4, 0, 1, 1},
        {1, 1, 1, 0, 1, 5, 1, 0},
        {0, 0, 1, 0, 0, 0, 1, 0},
        {0, 0, 1, 1, 1, 1, 1, 0}
    };
    //export skore - cas
    //menu - import progressu
    //10 level?

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
    //method for adding all objects to form, so can be moved / displayed
    //player is added first so it is on top of all other objects, 2nd are boxes for same reason
    public void AddToForm(Control container)
    {
        container.Controls.Add(player);

        foreach (var box in walls)
            container.Controls.Add(box);

        foreach (var box in boxes)
            container.Controls.Add(box);

        foreach (var box in finalDest)
            container.Controls.Add(box);
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
                        AddWall((j * Size + leftMargin), (i * Size + topMargin));
                        break;
                    case 3:
                        AddPlayer((j * Size + leftMargin), (i * Size + topMargin)); 
                        break;
                    
                    case 4:
                        AddBox((j * Size + leftMargin), (i * Size + topMargin));
                        break;

                    case 5:
                        AddFinalDestination((j * Size + leftMargin), (i * Size + topMargin));
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
                    box.isThere = true;
                    dests--;
                    break;
                }
                else
                {
                    box.isThere = false;
                }
            }
            box.getImage();
        }
        return dests == 0 ? true : false;
    }
}
