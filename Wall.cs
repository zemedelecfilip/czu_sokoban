public class Wall : PictureBox
{
    public int x, y;
    public int a = Storage.size;

    public Wall(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.BackColor = Color.Transparent;
        this.Image = Storage.getImage("Wall_Black.png");
        this.SizeMode = Storage.sizeMode;
    }

}