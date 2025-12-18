using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace czu_sokoban
{
    /// <summary>
    /// Provides global configuration and utility methods for the Sokoban game.
    /// </summary>
    public static class Storage
    {
        private const int TileSizeDenominator = 25;
        private const int TileSizeMultiplier = 2;
        private const string TexturesFolder = "Textures";
        private const string WallImagePrefix = "Wall";
        private const string CharacterImagePrefix = "Character";
        private const string CrateImagePrefix = "Crate";
        private const string EndPointImagePrefix = "EndPoint";
        private const string GroundImagePrefix = "Ground";

        public static int ScreenWidth { get; } = Screen.PrimaryScreen.Bounds.Width;
        public static int ScreenHeight { get; } = Screen.PrimaryScreen.Bounds.Height;

        public static int Size { get; } = ScreenHeight * TileSizeMultiplier / TileSizeDenominator;
        public static int PlayerSpeed { get; } = Size;

        public const int GridSize = 8;
        public static int LeftMargin { get; } = ScreenWidth / 2 - (GridSize * Size) / 2;
        public static int TopMargin { get; } = ScreenHeight / 2 - (GridSize * Size) / 2;

        public const int GridSize2 = 10;
        public static int LeftMargin2 { get; } = ScreenWidth / 2 - (GridSize2 * Size) / 2;
        public static int TopMargin2 { get; } = ScreenHeight / 2 - (GridSize2 * Size) / 2;

        public static string SelectedWall { get; set; } = "Wall_Black.png";
        public static string SelectedBox { get; set; } = "Crate_Blue.png";
        public static string SelectedEndPoint { get; set; } = "EndPoint_Purple.png";
        public static string SelectedTextures { get; set; } = "Ground_Concrete.png";

        public static PictureBoxSizeMode SizeMode { get; } = PictureBoxSizeMode.StretchImage;

        /// <summary>
        /// Loads an image from the Textures folder. Returns a default colored square if the image is not found.
        /// </summary>
        /// <param name="filePath">The filename of the image to load.</param>
        /// <returns>The loaded image or a default colored square.</returns>
        public static Image GetImage(string filePath)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\..\{TexturesFolder}\{filePath}");
            try
            {
                if (!File.Exists(path))
                {
                    return CreateDefaultImage(filePath);
                }
                return Image.FromFile(path);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return CreateDefaultImage(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return CreateDefaultImage(filePath);
            }
        }

        private static Image CreateDefaultImage(string filePath)
        {
            if (filePath.Contains(WallImagePrefix))
            {
                return CreateColoredSquare(Size, Color.Black);
            }
            if (filePath.Contains(CharacterImagePrefix))
            {
                return CreateColoredSquare(Size, Color.Red);
            }
            if (filePath.Contains(CrateImagePrefix))
            {
                return CreateColoredSquare(Size, Color.Blue);
            }
            if (filePath.Contains(EndPointImagePrefix))
            {
                return CreateColoredSquare(Size, Color.Purple);
            }
            if (filePath.Contains(GroundImagePrefix))
            {
                return CreateColoredSquare(Size, Color.Gray);
            }
            return CreateColoredSquare(Size, Color.White);
        }

        /// <summary>
        /// Creates a colored square bitmap of the specified size and color.
        /// </summary>
        /// <param name="size">The size of the square in pixels.</param>
        /// <param name="color">The color to fill the square with.</param>
        /// <returns>A bitmap with the specified size and color.</returns>
        public static Bitmap CreateColoredSquare(int size, Color color)
        {
            Bitmap bitmap = new Bitmap(size, size);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (SolidBrush brush = new SolidBrush(color))
                {
                    graphics.FillRectangle(brush, 0, 0, size, size);
                }
            }
            return bitmap;
        }

        /// <summary>
        /// Converts pixel coordinates to grid coordinates.
        /// </summary>
        /// <param name="x">The X coordinate in pixels.</param>
        /// <param name="y">The Y coordinate in pixels.</param>
        /// <returns>A Point representing the grid position.</returns>
        public static Point GridPos(int x, int y)
        {
            return new Point(x / Size, y / Size);
        }
    }
}
