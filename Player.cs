// Class for a player object in the game
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
        this.BackColor = Color.Transparent;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.SizeMode = Storage.sizeMode;
        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.Image = Storage.getImage("Character_down.png");
        this.DoubleBuffered = true;

    }

    // Movement methods for the player
    public void moveLeft(bool change_dir = false)
    {
        this.Left -= playerSpeed;
        this.x = this.Left;
        this.direction = change_dir ? "right" : "left";
        this.Image = Storage.getImage($"Character_{this.direction}.png");
    }
    public void moveRight(bool change_dir = false)
	{
        this.Left += playerSpeed;
		this.x = this.Left;
        this.direction = change_dir ? "left" : "right";
        this.Image = Storage.getImage($"Character_{this.direction}.png");
    }
    public void moveUp(bool change_dir = false)
	{
		this.Top -= playerSpeed;
		this.y = this.Top;
        this.direction = change_dir ? "down" : "up";
        this.Image = Storage.getImage($"Character_{this.direction}.png");
    }
    public void moveDown(bool change_dir = false)
	{
        this.Top += playerSpeed;
		this.y = this.Top;
        this.direction = change_dir ? "up" : "down";
        this.Image = Storage.getImage($"Character_{this.direction}.png");
    }

}