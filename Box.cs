public class Box : PictureBox
{
    public int x, y;
    public int a = 50;
    public int boxSpeed = 50;

    public Box(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.BackColor = Color.Blue;
    }
    public void MoveLeft()
    {
        this.Left -= boxSpeed;
        this.x = this.Left;
    }
    public void MoveRight()
    {
        this.Left += boxSpeed;
        this.x = this.Left;
    }
    public void MoveUp()
    {
        this.Top -= boxSpeed;
        this.y = this.Top;
    }
    public void MoveDown()
    {
        this.Top += boxSpeed;
        this.y = this.Top;
    }
    public Point gridPos()
    {
        return new Point(this.x/ a, this.y / a);
    }
}