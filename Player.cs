
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

	public void MoveLeft()
	{
		this.Left -= playerSpeed;
		this.x = this.Left;
    }

	public void MoveRight()
	{
		this.Left += playerSpeed;
		this.x = this.Left;
    }

	public void MoveUp()
	{
		this.Top -= playerSpeed;
		this.y = this.Top;
    }

	public void MoveDown()
	{
		this.Top += playerSpeed;
		this.y = this.Top;
	}
	public Point gridPos()
	{
        return new Point(this.x / a, this.y / a); //x, y
    }
}