using System.Drawing;
using System.Windows.Forms;

namespace czu_sokoban.Domain
{
    /// Represents a wall that blocks player and box movement in the Sokoban game.
    public class Wall : PictureBox
    {
        private readonly string _imagePath;

        public int X => Left;
        public int Y => Top;

        public Wall(int x, int y)
        {
            _imagePath = Storage.SelectedWall;

            int tileSize = Storage.Size;
            Location = new Point(x, y);
            Size = new Size(tileSize, tileSize);
            BackColor = Color.Transparent;
            Image = Storage.GetImage(_imagePath);
            SizeMode = Storage.SizeMode;
            DoubleBuffered = true;
        }
    }
}