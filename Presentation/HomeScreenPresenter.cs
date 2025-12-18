using System.Drawing;
using System.Windows.Forms;
using czu_sokoban.BusinessLogic;

namespace czu_sokoban.Presentation
{
    /// <summary>
    /// Handles presentation logic for the home screen.
    /// </summary>
    public class HomeScreenPresenter
    {
        private readonly Panel _homePanel;
        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private readonly Font _buttonFont;
        private readonly Color _buttonColor;

        public HomeScreenPresenter(Panel homePanel, int screenWidth, int screenHeight, Font buttonFont, Color buttonColor)
        {
            _homePanel = homePanel;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _buttonFont = buttonFont;
            _buttonColor = buttonColor;
        }

        public void InitializeHomeScreen(System.Action onPlayClick, System.Action onProfileClick, System.Action onShopClick, System.Action onExitClick)
        {
            Size buttonSize = new Size(_screenWidth / 5, _screenHeight / 10);
            int spacing = _screenHeight / 10;
            int startY = _screenHeight / 6;
            int centerX = (_screenWidth - buttonSize.Width) / 2;

            Button playButton = CreateButton("Play", buttonSize, new Point(centerX, startY), Color.LightGreen, onPlayClick);
            Button profileButton = CreateButton("Profile", buttonSize, new Point(centerX, 2 * startY), Color.LightSkyBlue, onProfileClick);
            Button shopButton = CreateButton("Shop", buttonSize, new Point(centerX, 3 * startY), Color.Khaki, onShopClick);
            Button exitButton = CreateButton("Exit", buttonSize, new Point(centerX, 4 * startY), Color.IndianRed, onExitClick);

            _homePanel.Controls.Add(playButton);
            _homePanel.Controls.Add(profileButton);
            _homePanel.Controls.Add(shopButton);
            _homePanel.Controls.Add(exitButton);
        }

        private Button CreateButton(string text, Size size, Point location, Color backColor, System.Action onClick)
        {
            Button button = new Button
            {
                Text = text,
                Font = _buttonFont,
                Size = size,
                Location = location,
                BackColor = backColor
            };
            button.Click += (s, e) => onClick();
            return button;
        }
    }
}

