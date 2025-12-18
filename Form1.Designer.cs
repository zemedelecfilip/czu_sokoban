using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using czu_sokoban.BusinessLogic;
using czu_sokoban.Domain;
using czu_sokoban.Presentation;

namespace czu_sokoban
{
    partial class Form1
    {
        private readonly int _screenWidth = Storage.ScreenWidth;
        private readonly int _screenHeight = Storage.ScreenHeight;
        private readonly PeopleDatabase _database = new PeopleDatabase();
        private readonly Maps _maps = new Maps();
        private readonly GameState _gameState = new GameState();
        private GameController _gameController;
        private LevelScreenPresenter _levelScreenPresenter;

        private Panel _homePanel;
        private Panel _levelsPanel;
        private Panel _levelPanel;
        private Panel _endLevelPanel;
        private Panel _profilePanel;
        private Panel _shopPanel;

        private Label _stepsLabel;
        private Label _timeLabel;
        private Label _endLevelStepsLabel;
        private Label _endLevelTimeLabel;
        private Label _endLevelCongratulationLabel;
        private Label _headerLabel;

        private readonly Font _buttonFont = new Font("Segoe UI", 18, FontStyle.Bold);
        private readonly Color _buttonColor = Color.FromArgb(174, 226, 255);

        private readonly List<Button> _saveButtons = new List<Button>();
        private readonly List<Button> _wallTextureButtons = new List<Button>();
        private readonly string[] _wallNames = { "Black", "Beige", "Brown", "Gray" };
        private readonly List<Button> _crateTextureButtons = new List<Button>();
        private readonly string[] _crateNames = { "Blue", "Beige", "Brown", "Red", "Yellow" };
        private readonly List<Button> _endPointTextureButtons = new List<Button>();
        private readonly string[] _endPointNames = { "Purple", "Beige", "Black", "Blue", "Brown", "Gray", "Red", "Yellow" };
        private readonly List<Button> _textureButtons = new List<Button>();
        private readonly string[] _textureNames = { "Concrete", "Dirt", "Grass", "Sand" };

        private int _currentWallSelected = 0;
        private int _currentCrateSelected = 0;
        private int _currentEndPointSelected = 0;
        private int _currentTextureSelected = 0;
        private bool _roundTexture = false;

        ////////////////////////////////////////////////////////////////////////
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {

            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.White;
            this.ClientSize = new Size(_screenWidth, _screenHeight);
            this.Name = "Form1";
            this.Text = "Sokoban Game";
            this.Load += Form1_Load;
            this.KeyDown += Form1_KeyDown;
            this.KeyPreview = true;

            InitializePanels();

            this.ResumeLayout(false);

        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.WindowState = FormWindowState.Maximized;
            _gameController = new GameController(_maps, _gameState, _database);
            _levelScreenPresenter = new LevelScreenPresenter(_levelPanel, _gameState, _gameController);
            ShowPanel(_homePanel);
        }
        /// <summary>
        /// Adds a global navigation button to panels (except home panel).
        /// </summary>
        private void AddGlobalButtonToPanel(Panel targetPanel)
        {
            if (targetPanel == _homePanel) return;

            Button backToMenu = new Button
            {
                Text = "Menu",
                Font = _buttonFont,
                Size = new Size(200, 100),
                Location = new Point(_screenWidth / 18, _screenHeight / 7),
                BackColor = _buttonColor
            };

            Button resetLevel = new Button
            {
                Text = "RESTART",
                Font = _buttonFont,
                Size = new Size(200, 100),
                Location = new Point(0, 0),
                BackColor = _buttonColor
            };
            resetLevel.Location = new Point(16 * _screenWidth / 18 - resetLevel.Width / 2, _screenHeight / 7);

            if (targetPanel == _levelPanel)
            {
                targetPanel.Controls.Add(resetLevel);
                resetLevel.Click += (s, e) => InitializeLevelScreen(_gameState.CurrentLevelName);
                resetLevel.Click += (s, e) => ResetVariables();

                backToMenu.Text = "Levels";
                backToMenu.Click += (s, e) => ShowPanel(_levelsPanel);
                backToMenu.Click += (s, e) => ResetVariables();
            }
            else if (targetPanel == _endLevelPanel)
            {
                backToMenu.Size = new Size(400, 100);
                backToMenu.Location = new Point(_screenWidth / 2 - backToMenu.Width / 2, 2 * _screenHeight / 3 - backToMenu.Height / 2);
                backToMenu.Click += (s, e) => InitializeLevelsScreen();
                backToMenu.Click += (s, e) => ShowPanel(_levelsPanel);
                backToMenu.Click += (s, e) => ResetVariables();
                backToMenu.Text = "Back To Levels";
            }
            else if (targetPanel == _shopPanel)
            {
                backToMenu.Location = new Point(_screenWidth / 18, _screenHeight / 10 - backToMenu.Height);
                backToMenu.Click += (s, e) =>
                {
                    Storage.SelectedBox = $"Crate_{_crateNames[_currentCrateSelected]}.png";
                    Storage.SelectedWall = _roundTexture ? $"WallRound_{_wallNames[_currentWallSelected]}.png" : $"Wall_{_wallNames[_currentWallSelected]}.png";
                    Storage.SelectedEndPoint = $"EndPoint_{_endPointNames[_currentEndPointSelected]}.png";
                    Storage.SelectedTextures = $"Ground_{_textureNames[_currentTextureSelected]}.png";
                };
                backToMenu.Click += (s, e) => ShowPanel(_homePanel);
            }
            else
            {
                backToMenu.Click += (s, e) => ShowPanel(_homePanel);
            }
            targetPanel.Controls.Add(backToMenu);
            backToMenu.BringToFront();
        }
        /// <summary>
        /// Initializes all panels for different screens.
        /// </summary>
        private void InitializePanels()
        {
            _homePanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(_homePanel);

            _levelsPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(_levelsPanel);

            _levelPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(_levelPanel);

            _endLevelPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(_endLevelPanel);

            _profilePanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(_profilePanel);

            _shopPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(_shopPanel);

            InitializeHomeScreen();
            InitializeShopScreen();
            InitializeLabels();

            AddHeaderToPanel(_homePanel);
            AddHeaderToPanel(_levelPanel);
            AddHeaderToPanel(_levelsPanel);
            AddHeaderToPanel(_profilePanel);
            AddHeaderToPanel(_shopPanel);
            AddHeaderToPanel(_endLevelPanel);
        }
        /// <summary>
        /// Initializes the home screen with all buttons and components.
        /// </summary>
        private void InitializeHomeScreen()
        {
            Size buttonSize = new Size(_screenWidth / 5, _screenHeight / 10);
            int spacing = _screenHeight / 10;
            int startY = _screenHeight / 6;
            int centerX = (this.ClientSize.Width - buttonSize.Width) / 2;

            Button playButton = new Button
            {
                Text = "Play",
                Font = _buttonFont,
                Size = buttonSize,
                Location = new Point(centerX, startY),
                BackColor = Color.LightGreen
            };
            playButton.Click += (s, e) => InitializeLevelsScreen();
            playButton.Click += (s, e) => ShowPanel(_levelsPanel);

            Button profileButton = new Button
            {
                Text = "Profile",
                Font = _buttonFont,
                Size = buttonSize,
                Location = new Point(centerX, 2 * startY),
                BackColor = Color.LightSkyBlue
            };
            profileButton.Click += (s, e) => InitializeProfileScreen();
            profileButton.Click += (s, e) => ShowPanel(_profilePanel);

            Button shopButton = new Button
            {
                Text = "Shop",
                Font = _buttonFont,
                Size = buttonSize,
                Location = new Point(centerX, 3 * startY),
                BackColor = Color.Khaki
            };
            shopButton.Click += (s, e) => ShowPanel(_shopPanel);

            Button exitButton = new Button
            {
                Text = "Exit",
                Font = _buttonFont,
                Size = buttonSize,
                Location = new Point(centerX, 4 * startY),
                BackColor = Color.IndianRed
            };
            exitButton.Click += (s, e) => this.Close();

            _homePanel.Controls.Add(playButton);
            _homePanel.Controls.Add(profileButton);
            _homePanel.Controls.Add(shopButton);
            _homePanel.Controls.Add(exitButton);
        }
        /// <summary>
        /// Adds a header panel with "SOKOBAN" label to the specified panel.
        /// </summary>
        private void AddHeaderToPanel(Panel parentPanel)
        {
            Panel headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Width = _screenWidth,
                Height = _screenHeight / 10,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(177, 240, 247)
            };

            _headerLabel = new Label
            {
                Text = "SOKOBAN",
                ForeColor = Color.Black,
                Font = new Font("Arial", 24, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            headerPanel.Controls.Add(_headerLabel);
            parentPanel.Controls.Add(headerPanel);
            AddGlobalButtonToPanel(parentPanel);
        }
        /// <summary>
        /// Initializes the levels screen with buttons for each level and displays best times/steps.
        /// </summary>
        private void InitializeLevelsScreen()
        {
            _levelsPanel.Controls.Clear();
            AddHeaderToPanel(_levelsPanel);

            Size buttonSize = new Size(_screenWidth / 8, _screenHeight / 8);
            int spacing = _screenHeight / 8;
            int startX = _screenHeight / 7;
            int[] rowYPositions = { 7 * _screenHeight / 20, 3 * _screenHeight / 5 };

            for (int levelIndex = 0; levelIndex < GameConstants.NumberOfLevels; levelIndex++)
            {
                string levelName = $"level{levelIndex + 1}";
                var results = _database.GetLevelTimesAndStepsByPlayer(_gameState.CurrentSaveId, levelName);
                
                int bestSteps = results.Count > 0 ? results[0].Steps : 0;
                double bestTime = results.Count > 0 ? results[0].Time : 0.0;

                int row = levelIndex < GameConstants.LevelsPerRow ? 0 : 1;
                int column = levelIndex % GameConstants.LevelsPerRow + 1;

                Button levelButton = CreateLevelButton(levelIndex + 1, buttonSize, startX, spacing, column, rowYPositions[row]);
                CreateLevelLabels(levelButton, buttonSize, bestSteps, bestTime, levelName);
            }
        }

        private Button CreateLevelButton(int levelNumber, Size buttonSize, int startX, int spacing, int column, int yPosition)
        {
            Button levelButton = new Button
            {
                Text = $"LEVEL {levelNumber}",
                Font = _buttonFont,
                Size = buttonSize,
                Location = new Point((startX + spacing) * column, yPosition),
                BackColor = _buttonColor
            };

            string levelName = $"level{levelNumber}";
            levelButton.Click += (s, e) => InitializeLevelScreen(levelName);
            levelButton.Click += (s, e) => ShowPanel(_levelPanel);

            _levelsPanel.Controls.Add(levelButton);
            return levelButton;
        }

        private void CreateLevelLabels(Button levelButton, Size buttonSize, int bestSteps, double bestTime, string levelName)
        {
            Font labelFont = new Font("Segoe UI", 12, FontStyle.Bold);

            Label stepsLabel = new Label
            {
                Text = $"Best step count: {bestSteps}",
                Font = labelFont,
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(levelButton.Location.X, levelButton.Location.Y + buttonSize.Height)
            };
            _levelsPanel.Controls.Add(stepsLabel);

            Label timeLabel = new Label
            {
                Text = $"Best time: {bestTime:F3} s",
                Font = labelFont,
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(levelButton.Location.X, levelButton.Location.Y + buttonSize.Height + stepsLabel.Height)
            };
            _levelsPanel.Controls.Add(timeLabel);
        }
        /// <summary>
        /// Initializes the level screen for the specified level.
        /// </summary>
        private void InitializeLevelScreen(string level)
        {
            _gameState.CurrentLevelName = level;
            _levelScreenPresenter?.InitializeLevelScreen(level, _maps);
        }
        /// <summary>
        /// Initializes labels for the end level screen.
        /// </summary>
        private void InitializeLabels()
        {
            _endLevelStepsLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.Black
            };
            _endLevelPanel.Controls.Add(_endLevelStepsLabel);

            _endLevelTimeLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.Black
            };
            _endLevelPanel.Controls.Add(_endLevelTimeLabel);

            _endLevelCongratulationLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 42, FontStyle.Bold),
                ForeColor = Color.Black
            };
            _endLevelPanel.Controls.Add(_endLevelCongratulationLabel);
        }
        /// <summary>
        /// Initializes the end level screen with final time, steps, and congratulation message.
        /// </summary>
        private void InitializeEndLevelScreen(string levelName, double time, int stepsCount)
        {
            _endLevelStepsLabel.Text = $"Final steps count: {stepsCount}";
            _endLevelTimeLabel.Text = $"Final time: {time:F3} s";
            _endLevelCongratulationLabel.Text = $"Congratulations for completing {levelName}";

            UpdateEndLevelLabelPositions();
        }

        private void UpdateEndLevelLabelPositions()
        {
            _endLevelStepsLabel.Location = new Point((_screenWidth - _endLevelStepsLabel.Width) / 2, _screenHeight / 3 + 2 * _endLevelStepsLabel.Height);
            _endLevelTimeLabel.Location = new Point((_screenWidth - _endLevelTimeLabel.Width) / 2, 2 * _screenHeight / 5 + 2 * _endLevelTimeLabel.Height);
            _endLevelCongratulationLabel.Location = new Point((_screenWidth - _endLevelCongratulationLabel.Width) / 2, _screenHeight / 5);
        }
        /// <summary>
        /// Initializes the profile screen with save selection buttons.
        /// </summary>
        private void InitializeProfileScreen()
        {
            for (int i = 0; i < GameConstants.NumberOfSaves; i++)
            {
                int saveId = i + 1;
                Button profileButton = CreateProfileButton(saveId);
                _saveButtons.Add(profileButton);
                _profilePanel.Controls.Add(profileButton);
            }

            if (_gameState.CurrentSaveId == 0)
            {
                _gameState.CurrentSaveId = 1;
            }
            UpdateButtonColors();
        }

        private Button CreateProfileButton(int saveId)
        {
            Button profileButton = new Button
            {
                Text = $"Save{saveId}",
                Font = _buttonFont,
                Size = new Size(_screenWidth / 5, _screenHeight / 6),
                BackColor = _buttonColor,
                Tag = saveId
            };

            profileButton.Location = new Point(saveId * _screenWidth / 4 - profileButton.Width / 2, _screenHeight / 2 - profileButton.Height / 2);
            profileButton.Click += (s, e) =>
            {
                _gameState.CurrentSaveId = saveId;
                UpdateButtonColors();
            };

            return profileButton;
        }

        /// <summary>
        /// Updates the colors of save buttons to indicate the current selection.
        /// </summary>
        private void UpdateButtonColors()
        {
            foreach (Button button in _saveButtons)
            {
                int buttonSaveId = (int)button.Tag;
                button.BackColor = (buttonSaveId == _gameState.CurrentSaveId)
                    ? Color.LightGreen
                    : _buttonColor;
            }
        }
        /// <summary>
        /// Updates the colors of shop texture selection buttons to indicate current selections.
        /// </summary>
        private void UpdateShopButtons()
        {
            const int selectedButtonAlpha = 200;
            const int notSelectedButtonAlpha = 120;
            Color selectedButtonColor = Color.FromArgb(selectedButtonAlpha, 195, 255, 153);
            Color notSelectedButtonColor = Color.FromArgb(notSelectedButtonAlpha, 255, 102, 102);

            UpdateTextureButtonColors(_crateTextureButtons, _currentCrateSelected, selectedButtonColor, notSelectedButtonColor);
            UpdateTextureButtonColors(_wallTextureButtons, _currentWallSelected, selectedButtonColor, notSelectedButtonColor);
            UpdateTextureButtonColors(_endPointTextureButtons, _currentEndPointSelected, selectedButtonColor, notSelectedButtonColor);
            UpdateTextureButtonColors(_textureButtons, _currentTextureSelected, selectedButtonColor, notSelectedButtonColor);
        }

        private void UpdateTextureButtonColors(List<Button> buttons, int selectedIndex, Color selectedColor, Color notSelectedColor)
        {
            foreach (Button button in buttons)
            {
                int buttonTextureId = (int)button.Tag;
                button.BackColor = (buttonTextureId == selectedIndex) ? selectedColor : notSelectedColor;
            }
        }
        /// <summary>
        /// Updates the preview picture box in the shop screen to show the selected texture.
        /// </summary>
        private Image UpdatePreviewPictureBox(int size, string textureName, bool isRound = true)
        {
            if (isRound)
            {
                _roundTexture = true;
                textureName = textureName.Replace("Wall_", "WallRound_");
            }

            try
            {
                return Storage.GetImage(textureName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading texture: {ex.Message}");
                return Storage.CreateColoredSquare(size, Color.Black);
            }
        }
        /// <summary>
        /// Initializes the shop screen with texture selection panels for walls, crates, endpoints, and ground textures.
        /// </summary>
        private void InitializeShopScreen()
        {
            Size panelSize = new Size(_screenWidth / 5, 4 * _screenHeight / 5);
            int panelWidth = _screenWidth / 5;
            int spacingX = _screenWidth / 25;
            int panelStartY = _screenHeight / 8;

            Panel wallsPanel = CreateWallsPanel(panelSize, spacingX, panelStartY);
            Panel cratePanel = CreateCratePanel(panelSize, wallsPanel.Right + spacingX, panelStartY);
            Panel endPointPanel = CreateEndPointPanel(panelSize, cratePanel.Right + spacingX, panelStartY);
            Panel texturePanel = CreateTexturePanel(panelSize, endPointPanel.Right + spacingX, panelStartY);

            _shopPanel.Controls.Add(wallsPanel);
            _shopPanel.Controls.Add(cratePanel);
            _shopPanel.Controls.Add(endPointPanel);
            _shopPanel.Controls.Add(texturePanel);

            UpdateShopButtons();
        }

        private Panel CreateWallsPanel(Size panelSize, int startX, int startY)
        {
            Panel wallsPanel = new Panel
            {
                Size = panelSize,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(startX, startY)
            };

            int buttonSize = panelSize.Width / 3;
            int previewTextureSize = 5 * buttonSize / 4;

            PictureBox wallTexturePreview = new PictureBox
            {
                Size = new Size(previewTextureSize, previewTextureSize),
                Location = new Point(wallsPanel.Width / 2 - previewTextureSize / 2, wallsPanel.Height / 15),
                Image = Storage.GetImage("Wall_Black.png"),
                BackgroundImageLayout = ImageLayout.Stretch,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            CheckBox wallRoundCheck = new CheckBox
            {
                Text = "SQUARE TEXTURE",
                Font = _buttonFont,
                BackColor = _buttonColor,
                Size = new Size(buttonSize, buttonSize),
                Location = new Point(wallsPanel.Width / 2 - buttonSize / 2, 3 * wallsPanel.Height / 4),
                Appearance = Appearance.Button,
                AutoCheck = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            for (int i = 0; i < _wallNames.Length; i++)
            {
                Button wallButton = CreateWallButton(i, buttonSize, wallsPanel);
                wallButton.Click += (s, e) =>
                {
                    _currentWallSelected = i;
                    UpdateShopButtons();
                    wallTexturePreview.Image = UpdatePreviewPictureBox(previewTextureSize, $"Wall_{_wallNames[_currentWallSelected]}.png", wallRoundCheck.Checked);
                };
                wallsPanel.Controls.Add(wallButton);
                _wallTextureButtons.Add(wallButton);
            }

            wallRoundCheck.CheckedChanged += (s, e) =>
            {
                wallRoundCheck.Text = wallRoundCheck.Checked ? "ROUND TEXTURE" : "SQUARE TEXTURE";
                wallTexturePreview.Image = UpdatePreviewPictureBox(previewTextureSize, $"Wall_{_wallNames[_currentWallSelected]}.png", wallRoundCheck.Checked);
            };

            wallsPanel.Controls.Add(wallRoundCheck);
            wallsPanel.Controls.Add(wallTexturePreview);
            return wallsPanel;
        }

        private Button CreateWallButton(int index, int buttonSize, Panel parentPanel)
        {
            int column = index % 2;
            int row = index / 2;
            int x = (int)((column == 0 ? 1f / 3f : 5f / 3f) * buttonSize);
            int y = (int)((row == 0 ? 2f / 5f : 3f / 5f) * parentPanel.Height - buttonSize / 2);

            return new Button
            {
                Font = _buttonFont,
                Size = new Size(buttonSize, buttonSize),
                Location = new Point(x, y),
                BackgroundImageLayout = ImageLayout.Stretch,
                Image = Storage.GetImage($"Wall_{_wallNames[index]}.png"),
                Tag = index,
                FlatStyle = FlatStyle.Standard
            };
        }

            // Add buttons to wallsPanel
            int buttonSize = panelSize.Width / 3;

            //Picture box on top to show what texture player selected
            int previewTextureSize = 5 * buttonSize / 4;

            PictureBox wallTexturePreview = new PictureBox
            {
                Size = new Size(previewTextureSize, previewTextureSize),
                Location = new Point(wallsPanel.Width / 2 - previewTextureSize / 2, wallsPanel.Height / 15),
                //+BackColor = Color.Red,
                Image = Storage.getImage($"Wall_Black.png"),
                BackgroundImageLayout = ImageLayout.Stretch,
                SizeMode = PictureBoxSizeMode.StretchImage,

            };

            CheckBox wallRoundCheck = new CheckBox
            {
                Text = "SQUARE TEXTURE",
                Font = btnFont,
                BackColor = btnColor,
                Size = new Size(buttonSize, buttonSize),
                Location = new Point(wallsPanel.Width / 2 - buttonSize / 2, 3 * wallsPanel.Height / 4),
                Appearance = Appearance.Button,
                AutoCheck = true, // Enable user interaction
            };

            //form of the loop from ai, other stuff from my design
            for (int i = 0; i < 4; i++)
            {
                int buttonIndex = i;
                // Determine column (0 or 1) and row (0 or 1)
                int col = i % 2;
                int row = i / 2;

                // X: 0 => 1/3 of buttonsize, 1 => 5/3 of button size
                int x = (int)((col == 0 ? 1f / 3f : 5f / 3f) * buttonSize);
            
                // Y: 0 => 2/5 of height, 1 => 4/5 of height
                int y = (int)((row == 0 ? 2f / 5f : 3f / 5f) * wallsPanel.Height - buttonSize / 2);

                Button wallButton = new Button
                {
                    //Text = $"{i + 1}",
                    Font = btnFont,
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(x, y),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Image = Storage.getImage($"Wall_{wallNames[i]}.png"),
                    Tag = buttonIndex,
                    FlatStyle = FlatStyle.Standard,
                    
                };
                //0: Beige, 1: Black, 2: Brown, 3: Gray
                wallButton.Click += (s, e) => currWallSelected = buttonIndex;
                wallButton.Click += (s, e) => UpdateShopButtons();
                wallButton.Click += (s, e) =>
                {
                    wallTexturePreview.Image = UpdatePreviewPictureBox(previewTextureSize, $"Wall_{wallNames[currWallSelected]}.png", wallRoundCheck.Checked);
                };
                wallsPanel.Controls.Add(wallButton);
                wallTextureList.Add(wallButton);

            }
            wallRoundCheck.TextAlign = ContentAlignment.MiddleCenter;

            wallRoundCheck.CheckedChanged += (s, e) =>
            {
                wallRoundCheck.Text = wallRoundCheck.Checked ? "ROUND TEXTURE" : "SQUARE TEXTURE";
                wallTexturePreview.Image = UpdatePreviewPictureBox(previewTextureSize, $"Wall_{wallNames[currWallSelected]}.png", wallRoundCheck.Checked);
            };

            wallsPanel.Controls.Add(wallRoundCheck);
            wallsPanel.Controls.Add(wallTexturePreview);

        private Panel CreateCratePanel(Size panelSize, int startX, int startY)
        {
            Panel cratePanel = new Panel
            {
                Size = panelSize,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(startX, startY)
            };

            int buttonSize = panelSize.Width / 3;
            const int totalCrateButtons = 10;

            for (int i = 0; i < totalCrateButtons; i++)
            {
                int buttonIndex = i / 2;
                int column = i % 2;
                int x = (int)((column == 0 ? 1f / 3f : 5f / 3f) * buttonSize);
                int y = (int)(cratePanel.Height / 28 + (i - column) * (4 * buttonSize / 6));

                bool isDark = i % 2 != 0;
                string imageName = isDark 
                    ? $"CrateDark_{_crateNames[buttonIndex]}.png" 
                    : $"Crate_{_crateNames[buttonIndex]}.png";

                Button crateButton = new Button
                {
                    Font = _buttonFont,
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(x, y),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Image = Storage.GetImage(imageName),
                    Tag = buttonIndex,
                    FlatStyle = FlatStyle.Standard
                };

                crateButton.Click += (s, e) =>
                {
                    _currentCrateSelected = buttonIndex;
                    UpdateShopButtons();
                };

                cratePanel.Controls.Add(crateButton);
                _crateTextureButtons.Add(crateButton);
            }

            return cratePanel;
        }

        private Panel CreateEndPointPanel(Size panelSize, int startX, int startY)
        {
            Panel endPointPanel = new Panel
            {
                Size = panelSize,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(startX, startY)
            };

            int buttonSize = panelSize.Width / 3;

            for (int i = 0; i < _endPointNames.Length; i++)
            {
                int column = i % 2;
                int x = (int)((column == 0 ? 1f / 3f : 5f / 3f) * buttonSize);
                int y = (int)(endPointPanel.Height / 8 + (i - column) * (4 * buttonSize / 6));

                Button endPointButton = new Button
                {
                    Font = _buttonFont,
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(x, y),
                    Image = Storage.GetImage($"EndPoint_{_endPointNames[i]}.png"),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Tag = i,
                    FlatStyle = FlatStyle.Standard
                };

                int buttonIndex = i;
                endPointButton.Click += (s, e) =>
                {
                    _currentEndPointSelected = buttonIndex;
                    UpdateShopButtons();
                };

                endPointPanel.Controls.Add(endPointButton);
                _endPointTextureButtons.Add(endPointButton);
            }

            return endPointPanel;
        }

        private Panel CreateTexturePanel(Size panelSize, int startX, int startY)
        {
            Panel texturePanel = new Panel
            {
                Size = panelSize,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(startX, startY)
            };

            int buttonSize = panelSize.Width / 3;
            const int totalTextureButtons = 8;

            for (int i = 0; i < totalTextureButtons; i++)
            {
                int buttonIndex = i / 2;
                int column = i % 2;
                int x = (int)((column == 0 ? 1f / 3f : 5f / 3f) * buttonSize);
                int y = (int)(texturePanel.Height / 8 + (i - column) * (4 * buttonSize / 6));

                bool isGravel = i % 2 != 0;
                string imageName = isGravel
                    ? $"GroundGravel_{_textureNames[buttonIndex]}.png"
                    : $"Ground_{_textureNames[buttonIndex]}.png";

                Button textureButton = new Button
                {
                    Font = _buttonFont,
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(x, y),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Image = Storage.GetImage(imageName),
                    Tag = buttonIndex,
                    FlatStyle = FlatStyle.Standard
                };

                textureButton.Click += (s, e) =>
                {
                    _currentTextureSelected = buttonIndex;
                    UpdateShopButtons();
                };

                texturePanel.Controls.Add(textureButton);
                _textureButtons.Add(textureButton);
            }

            return texturePanel;
        }
        /// <summary>
        /// Shows only the specified panel, hiding all others.
        /// </summary>
        private void ShowPanel(Panel targetPanel)
        {
            foreach (Control control in this.Controls.OfType<Panel>())
            {
                control.Visible = false;
            }
            targetPanel.Visible = true;
        }
        /// <summary>
        /// Handles key presses for player movement. Delegates game logic to GameController.
        /// </summary>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (_levelPanel.Visible && _gameController != null)
            {
                bool hasWon = _gameController.ProcessPlayerMovement(e.KeyCode);
                
                if (hasWon)
                {
                    ShowPanel(_endLevelPanel);
                    double currentTime = Convert.ToDouble(_gameController.Stopwatch.Elapsed.TotalSeconds.ToString("N3"));
                    InitializeEndLevelScreen(_gameState.CurrentLevelName, currentTime, _gameState.StepsCount);
                }

                _levelScreenPresenter?.UpdateLabels();
            }
        }

        /// <summary>
        /// Resets game variables for a new level start or restart.
        /// </summary>
        private void ResetVariables()
        {
            _gameController?.ResetGame();
        }
    }
}