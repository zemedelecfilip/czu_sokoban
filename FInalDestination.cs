public class FinalDestination : PictureBox
{
    public int x, y;
    public int a = 50;
    public FinalDestination(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.BackColor = Color.Green;
    }
}