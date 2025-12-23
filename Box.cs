using System.Drawing;
using System.Windows.Forms;

namespace czu_sokoban.Domain
{
    /// Represents a box (crate) that can be pushed by the player in the Sokoban game.
    public class Box : PictureBox
    {
        private const string CratePrefix = "Crate_";
        private const string CrateDarkPrefix = "CrateDark_";

        private readonly int _boxSpeed;
        private bool _isOnDestination;
        private readonly string _baseImagePath;

        public int X => Left;
        public int Y => Top;

        public bool IsOnDestination
        {
            get => _isOnDestination;
            set
            {
                _isOnDestination = value;
                UpdateBoxImage();
            }
        }

        public Box(int x, int y)
        {
            _boxSpeed = Storage.PlayerSpeed;
            _isOnDestination = false;
            _baseImagePath = Storage.SelectedBox;

            int tileSize = Storage.Size;
            BackColor = Color.Transparent;
            Location = new Point(x, y);
            Size = new Size(tileSize, tileSize);
            SizeMode = Storage.SizeMode;
            BackgroundImageLayout = ImageLayout.Stretch;
            DoubleBuffered = true;
            UpdateBoxImage();
        }

        /// Moves the box to the left.
        public void MoveLeft() => Left -= _boxSpeed;

        /// Moves the box to the right.
        public void MoveRight() => Left += _boxSpeed;

        /// Moves the box up.
        public void MoveUp() => Top -= _boxSpeed;

        /// Moves the box down.
        public void MoveDown() => Top += _boxSpeed;

        private void UpdateBoxImage()
        {
            string imagePath = _isOnDestination 
                ? _baseImagePath.Replace(CratePrefix, CrateDarkPrefix) 
                : _baseImagePath;
            Image = Storage.GetImage(imagePath);
        }
    }
}