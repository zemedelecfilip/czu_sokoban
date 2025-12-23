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

        private readonly string _imagePath;

        public int X => Left;
        public int Y => Top;

        public Texture(int x, int y, bool isOutside = false)
        {
            string baseImagePath = Storage.SelectedTextures;
            _imagePath = isOutside 
                ? baseImagePath 
                : baseImagePath.Replace(GroundPrefix, GroundGravelPrefix);

            int tileSize = Storage.Size;
            Location = new Point(x, y);
            Size = new Size(tileSize, tileSize);
            Image = Storage.GetImage(_imagePath);
            SizeMode = Storage.SizeMode;
            DoubleBuffered = true;
        }
    }
}