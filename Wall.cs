using System.Drawing;
using System.Windows.Forms;

namespace czu_sokoban.Domain
{
    /// <summary>
    /// Represents a wall that blocks player and box movement in the Sokoban game.
    /// </summary>
    public class Wall : PictureBox
    {
        private readonly int _tileSize;
        private int _x;
        private int _y;
        private readonly string _imagePath;

        public int X
        {
            get { return _x; }
            private set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            private set { _y = value; }
        }

        public Wall(int x, int y)
        {
            _tileSize = Storage.Size;
            _x = x;
            _y = y;
            _imagePath = Storage.SelectedWall;

            this.Location = new Point(_x, _y);
            this.Size = new Size(_tileSize, _tileSize);
            this.BackColor = Color.Transparent;
            this.Image = Storage.GetImage(_imagePath);
            this.SizeMode = Storage.SizeMode;
            this.DoubleBuffered = true;
        }
    }
}