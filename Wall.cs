public class Wall : PictureBox
{
    public int x, y;
    public int a = Storage.size;
    private string imagePath = Storage.selectedWall;
    public Wall(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.BackColor = Color.Transparent;
        this.Image = Storage.getImage(imagePath);
        this.SizeMode = Storage.sizeMode;
        this.DoubleBuffered = true;

    }

}