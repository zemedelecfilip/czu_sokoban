using System.Drawing;
using System.Windows.Forms;

namespace czu_sokoban.Domain
{
    /// <summary>
    /// Represents a ground texture tile in the Sokoban game.
    /// </summary>
    public class Texture : PictureBox
    {
        private const string GroundPrefix = "Ground_";
        private const string GroundGravelPrefix = "GroundGravel_";

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

        public Texture(int x, int y, bool isOutside = false)
        {
            _tileSize = Storage.Size;
            _x = x;
            _y = y;

            string baseImagePath = Storage.SelectedTextures;
            _imagePath = isOutside 
                ? baseImagePath 
                : baseImagePath.Replace(GroundPrefix, GroundGravelPrefix);

            this.Location = new Point(_x, _y);
            this.Size = new Size(_tileSize, _tileSize);
            this.Image = Storage.GetImage(_imagePath);
            this.SizeMode = Storage.SizeMode;
        }
    }
}