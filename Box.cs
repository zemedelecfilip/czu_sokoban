public class Box : PictureBox
{
    public int x, y;
    public int a = Storage.size;
    public int boxSpeed = Storage.playerSpeed;
    public bool isThere;
    public Box(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.isThere = false;
        this.Image = Storage.getImage(this.isThereBox() ? "CrateDark_Blue.png" : "Crate_Blue.png");
    }
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
    
    public bool isThereBox()
    {
        return this.isThere;
    }

}