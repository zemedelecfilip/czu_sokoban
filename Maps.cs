public class Maps
{
    // Constants for grid size
    public static int Size = Storage.size;
    public const int Width = Storage.gridSize;
    public const int Height = Storage.gridSize;
    public static int leftMargin = Storage.leftMargin;
    public static int topMargin = Storage.topMargin;
    public static int leftMargin2 = Storage.leftMargin;
    public static int topMargin2 = Storage.topMargin;
    public int screenWidth = Storage.screenWidth;
    public int screenHeight = Storage.screenHeight;

    public List<Box> boxes;
    public List<Wall> walls;
    public List<FinalDestination> finalDest;
    public Player player;
    public List<Texture> textures;

    //export skore - cas
    //menu - import progressu
    //10 level?

    // Constructor
    public Maps()
    {
        boxes = new List<Box>();
        walls = new List<Wall>();
        finalDest = new List<FinalDestination>();
        textures = new List<Texture>(); 
        player = null;
    }

    public void AddBox(int x, int y)
    {
        Box newBox = new Box(x, y);
        boxes.Add(newBox);
    }
    public void AddWall(int x, int y)
    {
        Wall newBox = new Wall(x, y);
        walls.Add(newBox);
    }

    public void AddPlayer(int x, int y)
    {
        Player player = new Player(x, y);
        this.player = player;
    }

    public void AddFinalDestination(int x, int y)
    {
        FinalDestination newBox = new FinalDestination(x, y);
        finalDest.Add(newBox);
    }

    public void AddTexture(int x, int y, bool outside = false)
    {
        Texture newBox = new Texture(x, y, outside);
        textures.Add(newBox);
    }

    //method for adding all objects to form, so can be moved / displayed
    //the order of adding is important, because the last added control is on top of the others
    public void AddToControls(Panel levelPanel)
    {
        // Clear previous controls;
        levelPanel.Controls.Clear();
        
        if (levelPanel == null)
        {
            //Console.WriteLine("level panel kdo??? - null");
            return;
        }

        if (walls != null)
        {
            foreach (var box in walls)
            {
                 levelPanel.Controls.Add(box);
            }
        }

        if (boxes != null)
        {
            foreach (var box in boxes)
            {
                 levelPanel.Controls.Add(box);
            }
        }

        if (player != null)
        {
            levelPanel.Controls.Add(player);
        }

        if (finalDest != null)
        {
            foreach (var box in finalDest)
            {
                 levelPanel.Controls.Add(box);
            }
        }

        if (textures != null)
        {
            foreach (var texture in textures)
            {
                levelPanel.Controls.Add(texture);
            }
        }

    }

    //create and add all objects to lists
    //FIXME na db verzi
    public void addObjToList(int [,] MapGrid, int arrSize)
    {
        //Console.WriteLine($"boxes: {boxes.Count}, walls: {walls.Count}, finalDest: {finalDest.Count}, player: {player}");
        // to nor oversize the lists
        boxes.Clear();
        finalDest.Clear();
        walls.Clear();
        textures.Clear();
        player = null;
        int topM = 0;
        int leftM = 0;

        
        if (arrSize == 10)
        {
            topM = Storage.topMargin2;
            leftM = Storage.leftMargin2;
        }
        else if (arrSize == 8)
        {
            topM = Storage.topMargin;
            leftM = Storage.leftMargin;
        }
        
        for (int i = 0; i < arrSize; i++)
        {
            for (int j = 0; j < arrSize; j++)
            {
                // 0 = empty, 1 - wall, 2 - outside texture, 3 = player, 4 = box, 5 - final destination, 6 - inside texture
                switch (MapGrid[i, j])
                {
                    case 1:
                        AddWall((j * Size + leftM), (i * Size + topM));
                        break;

                    case 2:
                        AddTexture((j * Size + leftM), (i * Size + topM), true);
                        break;
                    case 3:
                        AddPlayer((j * Size + leftM), (i * Size + topM));
                        AddTexture((j * Size + leftM), (i * Size + topM));
                        break;

                    case 4:
                        AddBox((j * Size + leftM), (i * Size + topM));
                        AddTexture((j * Size + leftM), (i * Size + topM));
                        break;

                    case 5:
                        AddTexture((j * Size + leftM), (i * Size + topM));
                        AddFinalDestination((j * Size + leftM), (i * Size + topM));
                        break;

                    case 6:
                        AddTexture((j * Size + leftM), (i * Size + topM));
                        break;

                    case 7:
                        AddFinalDestination((j * Size + leftM), (i * Size + topM));
                        AddWall((j * Size + leftM), (i * Size + topM));
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
            string path = Storage.selectedBox;
            path = box.isThere ? path.Replace("Crate_", "CrateDark_") : path;
            box.Image = Storage.getImage(path);
        }
        return dests == 0 ? true : false;
    }
}
