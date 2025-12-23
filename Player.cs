using System.Drawing;
using System.Windows.Forms;

namespace czu_sokoban.Domain
{
    /// Represents the player character in the Sokoban game.
    public class Player : PictureBox
    {
        private const string DefaultDirection = "down";
        private const string CharacterImagePrefix = "Character_";
        private const string ImageExtension = ".png";

        private readonly int _playerSpeed;
        private string _direction;

        public int X => Left;
        public int Y => Top;

        public string Direction
        {
            get => _direction;
            private set => _direction = value;
        }

        public Player(int x, int y)
        {
            _playerSpeed = Storage.PlayerSpeed;
            _direction = DefaultDirection;

            int tileSize = Storage.Size;
            BackColor = Color.Transparent;
            Location = new Point(x, y);
            Size = new Size(tileSize, tileSize);
            SizeMode = Storage.SizeMode;
            BackgroundImageLayout = ImageLayout.Stretch;
            Image = Storage.GetImage($"{CharacterImagePrefix}{DefaultDirection}{ImageExtension}");
            DoubleBuffered = true;
        }

        /// Moves the player to the left.
        public void MoveLeft(bool changeDirection = false)
        {
            Left -= _playerSpeed;
            Direction = changeDirection ? "right" : "left";
            UpdatePlayerImage();
        }

        /// Moves the player to the right.
        public void MoveRight(bool changeDirection = false)
        {
            Left += _playerSpeed;
            Direction = changeDirection ? "left" : "right";
            UpdatePlayerImage();
        }

        /// Moves the player up.
        public void MoveUp(bool changeDirection = false)
        {
            Top -= _playerSpeed;
            Direction = changeDirection ? "down" : "up";
            UpdatePlayerImage();
        }

        /// Moves the player down.
        public void MoveDown(bool changeDirection = false)
        {
            Top += _playerSpeed;
            Direction = changeDirection ? "up" : "down";
            UpdatePlayerImage();
        }

        private void UpdatePlayerImage()
        {
            Image = Storage.GetImage($"{CharacterImagePrefix}{_direction}{ImageExtension}");
        }
    }
}