using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using czu_sokoban.Domain;
using czu_sokoban.BusinessLogic;

namespace czu_sokoban.BusinessLogic
{
    /// <summary>
    /// Manages game map objects including boxes, walls, player, final destinations, and textures.
    /// </summary>
    public class Maps
    {
        private List<Box> _boxes;
        private List<Wall> _walls;
        private List<FinalDestination> _finalDestinations;
        private Player _player;
        private List<Texture> _textures;

        public List<Box> Boxes
        {
            get { return _boxes; }
        }

        public List<Wall> Walls
        {
            get { return _walls; }
        }

        public List<FinalDestination> FinalDestinations
        {
            get { return _finalDestinations; }
        }

        public Player Player
        {
            get { return _player; }
        }

        public List<Texture> Textures
        {
            get { return _textures; }
        }

        public Maps()
        {
            _boxes = new List<Box>();
            _walls = new List<Wall>();
            _finalDestinations = new List<FinalDestination>();
            _textures = new List<Texture>();
            _player = null;
        }
        /// <summary>
        /// Adds a box to the map at the specified coordinates.
        /// </summary>
        public void AddBox(int x, int y)
        {
            Box newBox = new Box(x, y);
            _boxes.Add(newBox);
        }

        /// <summary>
        /// Adds a wall to the map at the specified coordinates.
        /// </summary>
        public void AddWall(int x, int y)
        {
            Wall newWall = new Wall(x, y);
            _walls.Add(newWall);
        }

        /// <summary>
        /// Adds a player to the map at the specified coordinates.
        /// </summary>
        public void AddPlayer(int x, int y)
        {
            _player = new Player(x, y);
        }

        /// <summary>
        /// Adds a final destination to the map at the specified coordinates.
        /// </summary>
        public void AddFinalDestination(int x, int y)
        {
            FinalDestination newDestination = new FinalDestination(x, y);
            _finalDestinations.Add(newDestination);
        }

        /// <summary>
        /// Adds a texture to the map at the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="isOutside">True if this is an outside texture, false for inside texture.</param>
        public void AddTexture(int x, int y, bool isOutside = false)
        {
            Texture newTexture = new Texture(x, y, isOutside);
            _textures.Add(newTexture);
        }

        /// <summary>
        /// Adds all map objects to the specified panel for display.
        /// Layer order from bottom to top: textures (ground), final destinations, player, boxes, walls.
        /// </summary>
        /// <param name="levelPanel">The panel to add controls to.</param>
        public void AddToControls(Panel levelPanel)
        {
            if (levelPanel == null)
            {
                return;
            }

            levelPanel.Controls.Clear();
            // Add in order: textures (ground) first (bottom layer)
            AddTexturesToPanel(levelPanel);
            // Then final destinations
            AddFinalDestinationsToPanel(levelPanel);
            // Then player
            AddPlayerToPanel(levelPanel);
            // Then boxes
            AddBoxesToPanel(levelPanel);
            // Then walls (top layer)
            AddWallsToPanel(levelPanel);
            
            // Bring to front in order to establish proper layering
            BringFinalDestinationsToFront(levelPanel);
            BringPlayerToFront(levelPanel);
            BringBoxesToFront(levelPanel);
            BringWallsToFront(levelPanel);
        }

        private void AddTexturesToPanel(Panel levelPanel)
        {
            foreach (var texture in _textures)
            {
                levelPanel.Controls.Add(texture);
            }
        }

        private void AddWallsToPanel(Panel levelPanel)
        {
            foreach (var wall in _walls)
            {
                levelPanel.Controls.Add(wall);
            }
        }

        private void AddBoxesToPanel(Panel levelPanel)
        {
            foreach (var box in _boxes)
            {
                Texture textureAtPosition = _textures?.FirstOrDefault(t => t.X == box.X && t.Y == box.Y);
                if (textureAtPosition != null)
                {
                    box.BackgroundImage = textureAtPosition.Image;
                }
                levelPanel.Controls.Add(box);
            }
        }

        private void AddPlayerToPanel(Panel levelPanel)
        {
            if (_player != null)
            {
                Texture textureAtPosition = _textures?.FirstOrDefault(t => t.X == _player.X && t.Y == _player.Y);
                if (textureAtPosition != null)
                {
                    _player.BackgroundImage = textureAtPosition.Image;
                }
                levelPanel.Controls.Add(_player);
            }
        }

        private void AddFinalDestinationsToPanel(Panel levelPanel)
        {
            foreach (var destination in _finalDestinations)
            {
                Texture textureAtPosition = _textures?.FirstOrDefault(t => t.X == destination.X && t.Y == destination.Y);
                if (textureAtPosition != null)
                {
                    destination.BackgroundImage = textureAtPosition.Image;
                }
                levelPanel.Controls.Add(destination);
            }
        }

        /// <summary>
        /// Brings all final destinations to the front to ensure they are displayed above ground textures.
        /// </summary>
        /// <param name="levelPanel">The panel containing the final destinations.</param>
        private void BringFinalDestinationsToFront(Panel levelPanel)
        {
            foreach (var destination in _finalDestinations)
            {
                destination.BringToFront();
            }
        }

        /// <summary>
        /// Brings the player to the front to ensure it is displayed above final destinations.
        /// </summary>
        /// <param name="levelPanel">The panel containing the player.</param>
        private void BringPlayerToFront(Panel levelPanel)
        {
            if (_player != null)
            {
                _player.BringToFront();
            }
        }

        /// <summary>
        /// Brings all boxes to the front to ensure they are displayed above the player.
        /// </summary>
        /// <param name="levelPanel">The panel containing the boxes.</param>
        private void BringBoxesToFront(Panel levelPanel)
        {
            foreach (var box in _boxes)
            {
                box.BringToFront();
            }
        }

        /// <summary>
        /// Brings all walls to the front to ensure they are displayed above all other objects (top layer).
        /// </summary>
        /// <param name="levelPanel">The panel containing the walls.</param>
        private void BringWallsToFront(Panel levelPanel)
        {
            foreach (var wall in _walls)
            {
                wall.BringToFront();
            }
        }

        /// <summary>
        /// Creates and adds all game objects to lists based on the map grid data.
        /// </summary>
        /// <param name="mapGrid">The 2D array representing the map layout.</param>
        /// <param name="arraySize">The size of the map grid (8 or 10).</param>
        public void AddObjectsToList(int[,] mapGrid, int arraySize)
        {
            ClearAllObjects();
            int topMargin = GetTopMargin(arraySize);
            int leftMargin = GetLeftMargin(arraySize);
            int tileSize = Storage.Size;

            for (int row = 0; row < arraySize; row++)
            {
                for (int column = 0; column < arraySize; column++)
                {
                    int x = column * tileSize + leftMargin;
                    int y = row * tileSize + topMargin;
                    ProcessMapCell(mapGrid[row, column], x, y);
                }
            }
        }

        private void ClearAllObjects()
        {
            _boxes.Clear();
            _finalDestinations.Clear();
            _walls.Clear();
            _textures.Clear();
            _player = null;
        }

        private int GetTopMargin(int arraySize)
        {
            return arraySize == GameConstants.LargeGridSize 
                ? Storage.TopMargin2 
                : Storage.TopMargin;
        }

        private int GetLeftMargin(int arraySize)
        {
            return arraySize == GameConstants.LargeGridSize 
                ? Storage.LeftMargin2 
                : Storage.LeftMargin;
        }

        private void ProcessMapCell(int cellValue, int x, int y)
        {
            switch (cellValue)
            {
                case GameConstants.MapValueWall:
                    AddWall(x, y);
                    break;

                case GameConstants.MapValueOutsideTexture:
                    AddTexture(x, y, true);
                    break;

                case GameConstants.MapValuePlayer:
                    AddPlayer(x, y);
                    AddTexture(x, y);
                    break;

                case GameConstants.MapValueBox:
                    AddBox(x, y);
                    AddTexture(x, y);
                    break;

                case GameConstants.MapValueFinalDestination:
                    AddTexture(x, y);
                    AddFinalDestination(x, y);
                    break;

                case GameConstants.MapValueInsideTexture:
                    AddTexture(x, y);
                    break;

                case GameConstants.MapValueFinalDestinationWithWall:
                    AddFinalDestination(x, y);
                    AddWall(x, y);
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// Checks if the player collides with any box.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <param name="boxes">The list of boxes to check against.</param>
        /// <returns>The collided box, or null if no collision.</returns>
        public Box CollidedPlayerBox(Player player, List<Box> boxes)
        {
            if (player == null)
            {
                return null;
            }

            Point playerGridPos = Storage.GridPos(player.X, player.Y);
            foreach (var box in boxes)
            {
                Point boxGridPos = Storage.GridPos(box.X, box.Y);
                if (playerGridPos.Equals(boxGridPos))
                {
                    return box;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if a box collides with another box.
        /// </summary>
        /// <param name="boxToCheck">The box to check.</param>
        /// <param name="boxes">The list of boxes to check against.</param>
        /// <returns>The collided box, or null if no collision.</returns>
        public Box CollidedBoxBox(Box boxToCheck, List<Box> boxes)
        {
            if (boxToCheck == null)
            {
                return null;
            }

            Point boxGridPos = Storage.GridPos(boxToCheck.X, boxToCheck.Y);
            foreach (var box in boxes)
            {
                Point otherBoxGridPos = Storage.GridPos(box.X, box.Y);
                if (boxGridPos == otherBoxGridPos && boxToCheck != box)
                {
                    return box;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if a box collides with any wall.
        /// </summary>
        /// <param name="box">The box to check.</param>
        /// <param name="walls">The list of walls to check against.</param>
        /// <returns>The collided box, or null if no collision.</returns>
        public Box CollidedBoxWall(Box box, List<Wall> walls)
        {
            if (box == null)
            {
                return null;
            }

            Point boxGridPos = Storage.GridPos(box.X, box.Y);
            foreach (var wall in walls)
            {
                Point wallGridPos = Storage.GridPos(wall.X, wall.Y);
                if (boxGridPos == wallGridPos)
                {
                    return box;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if the player collides with any wall.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <param name="walls">The list of walls to check against.</param>
        /// <returns>True if collision detected, false otherwise.</returns>
        public bool CollidedPlayerWall(Player player, List<Wall> walls)
        {
            if (player == null)
            {
                return false;
            }

            Point playerGridPos = Storage.GridPos(player.X, player.Y);
            foreach (var wall in walls)
            {
                Point wallGridPos = Storage.GridPos(wall.X, wall.Y);
                if (playerGridPos == wallGridPos)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if all boxes are placed on final destinations (win condition).
        /// </summary>
        /// <param name="boxes">The list of boxes to check.</param>
        /// <param name="finalDestinations">The list of final destinations.</param>
        /// <returns>True if all boxes are on destinations, false otherwise.</returns>
        public bool CheckWin(List<Box> boxes, List<FinalDestination> finalDestinations)
        {
            if (boxes == null || finalDestinations == null || boxes.Count != finalDestinations.Count)
            {
                return false;
            }

            // Track which destinations have been covered
            HashSet<Point> coveredDestinations = new HashSet<Point>();
            
            // Update box states and check if each box is on a destination
            foreach (var box in boxes)
            {
                bool boxPlaced = false;
                Point boxGridPos = Storage.GridPos(box.X, box.Y);
                
                foreach (var destination in finalDestinations)
                {
                    Point destGridPos = Storage.GridPos(destination.X, destination.Y);
                    if (boxGridPos == destGridPos)
                    {
                        box.IsOnDestination = true;
                        coveredDestinations.Add(destGridPos);
                        boxPlaced = true;
                        break;
                    }
                }

                if (!boxPlaced)
                {
                    box.IsOnDestination = false;
                }
            }

            // Win condition: all destinations must be covered and all boxes must be on destinations
            return coveredDestinations.Count == finalDestinations.Count && 
                   boxes.All(box => box.IsOnDestination);
        }

        /// <summary>
        /// Updates the background image of a PictureBox based on its current position.
        /// Maintains proper layer order: textures (bottom), final destinations, player, boxes, walls (top).
        /// </summary>
        /// <param name="pictureBox">The PictureBox to update.</param>
        public void UpdateBackgroundImage(PictureBox pictureBox)
        {
            if (pictureBox == null || _textures == null)
            {
                return;
            }

            Texture textureAtPosition = _textures.FirstOrDefault(t => t.X == pictureBox.Left && t.Y == pictureBox.Top);
            if (textureAtPosition != null)
            {
                pictureBox.BackgroundImage = textureAtPosition.Image;
            }
            
            // Maintain proper layer order after movement
            if (pictureBox is Player)
            {
                // Player layer: bring final destinations first, then player, then boxes, then walls
                foreach (var destination in _finalDestinations)
                {
                    destination.BringToFront();
                }
                pictureBox.BringToFront();
                foreach (var box in _boxes)
                {
                    box.BringToFront();
                }
                foreach (var wall in _walls)
                {
                    wall.BringToFront();
                }
            }
            else if (pictureBox is Box)
            {
                // Box layer: bring final destinations, then player, then ALL boxes, then walls
                // Important: bring ALL boxes to front, not just the moved one, to prevent boxes from disappearing
                foreach (var destination in _finalDestinations)
                {
                    destination.BringToFront();
                }
                if (_player != null)
                {
                    _player.BringToFront();
                }
                // Bring all boxes to front to ensure proper layering for all boxes
                foreach (var box in _boxes)
                {
                    box.BringToFront();
                }
                foreach (var wall in _walls)
                {
                    wall.BringToFront();
                }
            }
            else
            {
                pictureBox.BringToFront();
            }
        }
    }
}
