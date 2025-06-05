//vytvoøení všech objektù na základì mapy
using czu_sokoban;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection.Emit;
using System.Security;
using System.Windows.Forms;
using System.Xml.Linq;

public class Maps
{
    // Constants for grid size
    public const int Size = Storage.size;
    public const int Width = Storage.gridSize;
    public const int Height = Storage.gridSize;
    public static int leftMargin = Storage.leftMargin;
    public static int topMargin = Storage.topMargin;
    public int screenWidth = Storage.screenWidth;
    public int screenHeight = Storage.screenHeight;

    public List<Box> boxes;
    public List<Wall> walls;
    public List<FinalDestination> finalDest;
    public Player player;

    //export skore - cas
    //menu - import progressu
    //10 level?

    // Constructor
    public Maps()
    {
        boxes = new List<Box>();
        walls = new List<Wall>();
        finalDest = new List<FinalDestination>();
        player = null;
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
    //FIXED na panel verzi
    public void AddToControls(Panel LevelPanel)
    {
        // Clear previous controls;
        LevelPanel.Controls.Clear();
        
        if (LevelPanel == null)
        {
            Console.WriteLine("level panel kdo??? - null");
            return;
        }
        Console.WriteLine("level panel nekdo??? - not null");


        if (player != null)
        {
            LevelPanel.Controls.Add(player);
        }

        if (walls != null)
        {
            foreach (var box in walls)
            {
                 LevelPanel.Controls.Add(box);
            }
        }

        if (boxes != null)
        {
            foreach (var box in boxes)
            {
                 LevelPanel.Controls.Add(box);
            }
        }

        if (finalDest != null)
        {
            foreach (var box in finalDest)
            {
                 LevelPanel.Controls.Add(box);
            }
        }
    }

    //create and add all objects to lists
    //FIXME na db verzi
    public void addObjToList(int [,] MapGrid)
    {
        Console.WriteLine($"boxes: {boxes.Count}, walls: {walls.Count}, finalDest: {finalDest.Count}, player: {player}");
        // to nor oversize the lists
        boxes.Clear();
        finalDest.Clear();
        walls.Clear();
        player = null;

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
            if (Storage.gridPos(player.x, player.y).Equals(Storage.gridPos(box.x, box.y)))
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
            if (Storage.gridPos(bob.x, bob.y) == Storage.gridPos(box.x, box.y) && bob != box)
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
            if (Storage.gridPos(box.x, box.y) == Storage.gridPos(wall.x, wall.y))
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
            if (Storage.gridPos(player.x, player.y) == Storage.gridPos(wall.x, wall.y))
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
                if (Storage.gridPos(box.x, box.y) == Storage.gridPos(dest.x, dest.y))
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
            box.Image = Storage.getImage(box.isThereBox() ? "CrateDark_Blue.png" : "Crate_Blue.png");
        }
        return dests == 0 ? true : false;
    }
}
