using System.Drawing;
using System.Windows.Forms;

namespace czu_sokoban.Domain
{
    /// Represents a final destination point where boxes must be placed to complete the level.
    public class FinalDestination : PictureBox
    {
        private readonly string _imagePath;

        public int X => Left;
        public int Y => Top;

        public FinalDestination(int x, int y)
        {
            _imagePath = Storage.SelectedEndPoint;

            int tileSize = Storage.Size;
            BackColor = Color.Transparent;
            Location = new Point(x, y);
            Size = new Size(tileSize, tileSize);
            Image = Storage.GetImage(_imagePath);
            SizeMode = Storage.SizeMode;
            BackgroundImageLayout = ImageLayout.Stretch;
            DoubleBuffered = true;
        }
    }
}