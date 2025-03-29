public class Wall : PictureBox
{
    public int x, y;
    public int a = 50;

    public Wall(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.Location = new Point(this.x, this.y);
        this.Size = new Size(a, a);
        this.getImage();
    }

    public Point gridPos()
    {
        return new Point(this.x / a, this.y / a);
    }

    public void getImage()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Textures\Wall_Black.png");
        try
        {
            if (!File.Exists(path))
            {
                this.BackColor = Color.Black;
            }

            this.Image = Image.FromFile(path);
            this.SizeMode = PictureBoxSizeMode.CenterImage;
        }
        catch (FileNotFoundException ex)
        {
            //MessageBox.Show($"Error: {ex.Message}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.BackColor = Color.Black;
        }
        catch (Exception ex)
        {
            //MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.BackColor = Color.Black;
        }
    }
}