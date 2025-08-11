public class FinalDestination : PictureBox
{
    public int x, y;
    public int a = Storage.size;
    private string imagePath = Storage.selectedEndPoint;
    public FinalDestination(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.BackColor = Color.Transparent;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.Image = Storage.getImage(imagePath);
        this.SizeMode = Storage.sizeMode;

    }

}