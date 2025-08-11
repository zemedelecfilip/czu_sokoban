public class Texture : PictureBox
{
    public int x, y;
    public int a = Storage.size;
    //string path;
    private string imagePath = Storage.selectedTextures;
    public Texture(int x, int y, bool outside = false)
    {
        this.x = x;
        this.y = y;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        if (!outside)
        {
            imagePath = imagePath.Replace("Ground_", "GroundGravel_");
        }
        this.Image = Storage.getImage(imagePath);
        this.SizeMode = Storage.sizeMode;
    }

}