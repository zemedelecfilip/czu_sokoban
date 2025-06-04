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
        // file path for wall image get from db (current skin)
        this.Image = Storage.getImage();
    }

    public Point gridPos()
    {
        return new Point(this.x / a, this.y / a);
    }
}