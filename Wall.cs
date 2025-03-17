public class Wall : PictureBox
{
    public int x, y;
    public int a = 50;

    public Wall(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.BackColor = Color.Black;
    }

    public Point gridPos()
    {
        return new Point(this.x / a, this.y / a);
    }
}