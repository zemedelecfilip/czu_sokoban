
using System.Runtime.CompilerServices;

public class Player: PictureBox
{
	public int a = Storage.size;
	public int playerSpeed = Storage.playerSpeed;
	public int x, y;
    public string direction = "down";
    //public bool firstLoead = true;
    public Player(int x, int y)
	{
		this.x = x;
		this.y = y;
		this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.SizeMode = Storage.sizeMode;
        this.Image = Storage.getImage("Character_down.png");
    }

    public string getImagePath(string direction)
    {
        switch (direction)
        {
            case "up":
                return "Character_up.png";
            case "down":
                return "Character_down.png";
            case "left":
                return "Character_left.png";
            case "right":
                return "Character_right.png";
            default:
                return null;

        }
    }
    
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

    // use the getImage method from a storage combined with a case switch 

}