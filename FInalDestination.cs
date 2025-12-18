using System.Drawing;
using System.Windows.Forms;

namespace czu_sokoban.Domain
{
    /// <summary>
    /// Represents a final destination point where boxes must be placed to complete the level.
    /// </summary>
    public class FinalDestination : PictureBox
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

        public FinalDestination(int x, int y)
        {
            _tileSize = Storage.Size;
            _x = x;
            _y = y;
            _imagePath = Storage.SelectedEndPoint;

            this.BackColor = Color.Transparent;
            this.Location = new Point(_x, _y);
            this.Size = new Size(_tileSize, _tileSize);
            this.Image = Storage.GetImage(_imagePath);
            this.SizeMode = Storage.SizeMode;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.DoubleBuffered = true;
        }
    }
}