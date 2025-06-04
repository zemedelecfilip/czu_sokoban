
public class Player: PictureBox
{
	public int a = Storage.size;
	public int playerSpeed = Storage.playerSpeed;
	public int x, y;
    public string direction = "down";
	public Player(int x, int y)
	{
		this.x = x;
		this.y = y;
		this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.getImage(direction);
        //this.BackColor = Color.Red;
    }

	public void moveLeft(bool change_dir = false)
	{
        this.Left -= playerSpeed;
		this.x = this.Left;
        this.direction = change_dir ? "right" : "left";
        this.getImage(direction);
    }
	
	public void moveRight(bool change_dir = false)
	{
        this.Left += playerSpeed;
		this.x = this.Left;
        this.direction = change_dir ? "left" : "right";
        this.getImage(direction);
    }

	public void moveUp(bool change_dir = false)
	{
		this.Top -= playerSpeed;
		this.y = this.Top;
        this.direction = change_dir ? "down" : "up";
        this.getImage(direction);
    }

    public void moveDown(bool change_dir = false)
	{
        this.Top += playerSpeed;
		this.y = this.Top;
        this.direction = change_dir ? "up" : "down";
        this.getImage(direction);
    }
    public Point gridPos()
	{
        return new Point(this.x / a, this.y / a); //x, y
    }

    // use the getImage method from a storage combined with a case switch 
    public void getImage(string dir)
	{
		string path;
		switch (dir)
		{
            case "left":
				path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Textures\Character_left.png");
                break;

            case "right":
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Textures\Character_right.png");
                break;

            case "up":
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Textures\Character_up.png");
                break;

            case "down":
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Textures\Character_down.png");
                break;

            default:
				path = null;
                break;
        }
        try
        {
            if (!File.Exists(path))
            {
                this.BackColor = Color.Red;
            }

            this.Image = Image.FromFile(path);
            if (Storage.size > 50)
            {
                this.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                this.SizeMode = PictureBoxSizeMode.CenterImage;
            }
        }
        catch (FileNotFoundException ex)
        {
            //MessageBox.Show($"Error: {ex.Message}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            //MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}