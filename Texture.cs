public class Texture : PictureBox
{
    public int x, y;
    public int a = Storage.size;
    string path;

    public Texture(int x, int y, string path)
    {
        this.path = path;
        this.x = x;
        this.y = y;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        // file path for wall image get from db (current skin)
        this.Image = Storage.getImage(path);
        this.SizeMode = Storage.sizeMode;
    }

}