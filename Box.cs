public class Box : PictureBox
{
    public int x, y;
    public int a = 50;
    public int boxSpeed = 50;

    public Box(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.getImage();
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
    public Point gridPos()
    {
        return new Point(this.x/ a, this.y / a);
    }

    public void getImage()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Textures\CrateDark_Blue.png");
        try
        {
            if (!File.Exists(path))
            {
                this.BackColor = Color.Blue;
            }

            this.Image = Image.FromFile(path);
            this.SizeMode = PictureBoxSizeMode.CenterImage;
        }
        catch (FileNotFoundException ex)
        {
            //MessageBox.Show($"Error: {ex.Message}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.BackColor = Color.Blue;
        }
        catch (Exception ex)
        {
            //MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.BackColor = Color.Blue;
        }
    }
}