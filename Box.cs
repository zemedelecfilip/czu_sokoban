// Class for boxes objects in the game
public class Box : PictureBox
{
    public int x, y;
    public int a = Storage.size;
    public int boxSpeed = Storage.playerSpeed;
    public bool isThere;
    private string imagePath = Storage.selectedBox;
    public Box(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.BackColor = Color.Transparent;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.isThere = false;
        if (isThere)
        {
            imagePath = imagePath.Replace("Crate_", "CrateDark_");
        }
        this.Image = Storage.getImage(imagePath);
        this.SizeMode = Storage.sizeMode;
        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.DoubleBuffered = true;

    }
    // Box movement methods
    public void moveLeft()
    {
        this.Left -= boxSpeed;
        this.x = this.Left;
    }
    public void moveRight()
    {
        this.Left += boxSpeed;
        this.x = this.Left;
    }
    public void moveUp()
    {
        this.Top -= boxSpeed;
        this.y = this.Top;
    }
    public void moveDown()
    {
        this.Top += boxSpeed;
        this.y = this.Top;
    }
    
}