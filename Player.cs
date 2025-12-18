using System.Drawing;
using System.Windows.Forms;

namespace czu_sokoban.Domain
{
    /// <summary>
    /// Represents the player character in the Sokoban game.
    /// </summary>
    public class Player : PictureBox
    {
        private const string DefaultDirection = "down";
        private const string CharacterImagePrefix = "Character_";
        private const string ImageExtension = ".png";

        private readonly int _tileSize;
        private readonly int _playerSpeed;
        private int _x;
        private int _y;
        private string _direction;

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

        public string Direction
        {
            get { return _direction; }
            private set { _direction = value; }
        }

        public Player(int x, int y)
        {
            _tileSize = Storage.Size;
            _playerSpeed = Storage.PlayerSpeed;
            _x = x;
            _y = y;
            _direction = DefaultDirection;

            this.BackColor = Color.Transparent;
            this.Location = new Point(_x, _y);
            this.Size = new Size(_tileSize, _tileSize);
            this.SizeMode = Storage.SizeMode;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Image = Storage.GetImage($"{CharacterImagePrefix}{DefaultDirection}{ImageExtension}");
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Moves the player to the left.
        /// </summary>
        /// <param name="changeDirection">If true, changes direction to right (for collision reversal).</param>
        public void MoveLeft(bool changeDirection = false)
        {
            this.Left -= _playerSpeed;
            _x = this.Left;
            _direction = changeDirection ? "right" : "left";
            UpdatePlayerImage();
        }

        /// <summary>
        /// Moves the player to the right.
        /// </summary>
        /// <param name="changeDirection">If true, changes direction to left (for collision reversal).</param>
        public void MoveRight(bool changeDirection = false)
        {
            this.Left += _playerSpeed;
            _x = this.Left;
            _direction = changeDirection ? "left" : "right";
            UpdatePlayerImage();
        }

        /// <summary>
        /// Moves the player up.
        /// </summary>
        /// <param name="changeDirection">If true, changes direction to down (for collision reversal).</param>
        public void MoveUp(bool changeDirection = false)
        {
            this.Top -= _playerSpeed;
            _y = this.Top;
            _direction = changeDirection ? "down" : "up";
            UpdatePlayerImage();
        }

        /// <summary>
        /// Moves the player down.
        /// </summary>
        /// <param name="changeDirection">If true, changes direction to up (for collision reversal).</param>
        public void MoveDown(bool changeDirection = false)
        {
            this.Top += _playerSpeed;
            _y = this.Top;
            _direction = changeDirection ? "up" : "down";
            UpdatePlayerImage();
        }

        private void UpdatePlayerImage()
        {
            this.Image = Storage.GetImage($"{CharacterImagePrefix}{_direction}{ImageExtension}");
        }
    }
}