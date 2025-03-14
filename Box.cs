public class Box : PictureBox
{
    public int x, y;
    public int size = 50;
    public int boxSpeed = 50;
    public Box(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.BackColor = Color.Blue;
    }
    public void MoveLeft()
    {
        this.Left -= boxSpeed;
    }
    public void MoveRight()
    {
        this.Left += boxSpeed;
    }
    public void MoveUp()
    {
        this.Top -= boxSpeed;
    }
    public void MoveDown()
    {
        this.Top += boxSpeed;
    }
    public Point gridPos()
    {
        return new Point(this.Left / size, this.Top / size);
    }
}