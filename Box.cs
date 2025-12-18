using System.Drawing;
using System.Windows.Forms;

namespace czu_sokoban.Domain
{
    /// <summary>
    /// Represents a box (crate) that can be pushed by the player in the Sokoban game.
    /// </summary>
    public class Box : PictureBox
    {
        private const string CratePrefix = "Crate_";
        private const string CrateDarkPrefix = "CrateDark_";

        private readonly int _tileSize;
        private readonly int _boxSpeed;
        private int _x;
        private int _y;
        private bool _isOnDestination;
        private readonly string _baseImagePath;

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

        public bool IsOnDestination
        {
            get { return _isOnDestination; }
            set
            {
                _isOnDestination = value;
                UpdateBoxImage();
            }
        }

        public Box(int x, int y)
        {
            _tileSize = Storage.Size;
            _boxSpeed = Storage.PlayerSpeed;
            _x = x;
            _y = y;
            _isOnDestination = false;
            _baseImagePath = Storage.SelectedBox;

            this.BackColor = Color.Transparent;
            this.Location = new Point(_x, _y);
            this.Size = new Size(_tileSize, _tileSize);
            this.SizeMode = Storage.SizeMode;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.DoubleBuffered = true;
            UpdateBoxImage();
        }

        /// <summary>
        /// Moves the box to the left.
        /// </summary>
        public void MoveLeft()
        {
            this.Left -= _boxSpeed;
            _x = this.Left;
        }

        /// <summary>
        /// Moves the box to the right.
        /// </summary>
        public void MoveRight()
        {
            this.Left += _boxSpeed;
            _x = this.Left;
        }

        /// <summary>
        /// Moves the box up.
        /// </summary>
        public void MoveUp()
        {
            this.Top -= _boxSpeed;
            _y = this.Top;
        }

        /// <summary>
        /// Moves the box down.
        /// </summary>
        public void MoveDown()
        {
            this.Top += _boxSpeed;
            _y = this.Top;
        }

        private void UpdateBoxImage()
        {
            string imagePath = _isOnDestination 
                ? _baseImagePath.Replace(CratePrefix, CrateDarkPrefix) 
                : _baseImagePath;
            this.Image = Storage.GetImage(imagePath);
        }
    }
}