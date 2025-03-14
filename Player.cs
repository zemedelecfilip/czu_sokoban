
public class Player: PictureBox
{
	public int playerSpeed = 50;
	public int x, y;
	public int size = 50;

	public Player(int x, int y)
	{
		this.x = x;
		this.y = y;
		this.BackColor = Color.Red;
    }

	public void MoveLeft()
	{
		this.Left -= playerSpeed;
	}

	public void MoveRight()
	{
		this.Left += playerSpeed;
	}

	public void MoveUp()
	{
		this.Top -= playerSpeed;
	}

	public void MoveDown()
	{
		this.Top += playerSpeed;
	}
	public Point gridPos()
	{
        return new Point(this.Left / size, this.Top / size); //x, y
    }
}