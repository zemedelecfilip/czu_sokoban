
public class Player: PictureBox
{
	public int a = 50;
	public int playerSpeed = 50;
	public int x, y;
	public Player(int x, int y)
	{
		this.x = x;
		this.y = y;
		this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.BackColor = Color.Red;
    }

	public void moveLeft()
	{
		this.Left -= playerSpeed;
		this.x = this.Left;
    }
	
	public void moveRight()
	{
		this.Left += playerSpeed;
		this.x = this.Left;
    }

	public void moveUp()
	{
		this.Top -= playerSpeed;
		this.y = this.Top;
    }

	public void moveDown()
	{
		this.Top += playerSpeed;
		this.y = this.Top;
	}
	public Point gridPos()
	{
        return new Point(this.x / a, this.y / a); //x, y
    }
}