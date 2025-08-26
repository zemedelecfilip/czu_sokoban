using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using static System.Windows.Forms.AxHost;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Drawing;
using System.Net;
//all textures from https://opengameart.org/content/sokoban-pack

namespace czu_sokoban
{
    partial class Form1
    {
        public int screenW = Storage.screenWidth;
        public int screenH = Storage.screenHeight;

        PeopleDatabase database = new PeopleDatabase();
        Maps maps = new Maps();
        Player player;
        List<Box> boxes;
        List<Wall> walls;
        List<FinalDestination> finalDest;
        // deklarování panelů pro různé obrazovky
        // Home, Levels, Level, Endlevel, profile, shop
        private Panel homePanel;
        private Panel levelsPanel;
        private Panel levelPanel;
        private Panel endLevelPanel;
        private Panel profilePanel;
        private Panel shopPanel;
        // labels inicialization
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label headerLabel;
        public PictureBox homeScreenRect;
        public Font btnFont = new Font("Segoe UI", 18, FontStyle.Bold);
        public Color btnColor = Color.FromArgb(174, 226, 255);
        public int stepsCount = 0;
        Stopwatch stopwatch = new Stopwatch();
        // Inicialization for a shop panel
        public string currLevelName = "";
        public int currSave = 1;
        private List<Button> saveButtons = new List<Button>();
        private List<Button> wallTextureList = new List<Button>();
        private string[] wallNames = {"Black", "Beige", "Brown", "Gray"};
        private List<Button> crateTextureList = new List<Button>();
        private string[] crateNames = {"Blue", "Beige", "Brown", "Red", "Yellow"};
        private List<Button> endPointTextureList = new List<Button>();
        string[] endPointNames = {"Purple", "Beige", "Black", "Blue", "Brown", "Gray", "Red", "Yellow"};
        private List<Button> texturesTextureList = new List<Button>();
        string[] texturesNames = {"Concrete", "Dirt", "Grass", "Sand"};
        //default wall texture
        int currWallSelected = 0;
        int currCrateSelected = 0;
        int currEndPointSelected = 0;
        int currTexturesSelected = 0;
        bool roundTexture = false;

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
            this.ClientSize = new Size(screenW, screenH);
            this.Name = "Form1";
            this.Text = "Sokoban Game";
            this.Load += Form1_Load;
            this.KeyDown += Form1_KeyDown;  // Add key handler
            this.KeyPreview = true;         // Enable key preview

            // Initialize panels (better to do this in constructor)
            InitializePanels();

            this.ResumeLayout(false);

        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual; // Important!
            this.Location = new Point(0, 0);
            this.WindowState = FormWindowState.Maximized;
            this.ShowPanel(homePanel);
            //Console.WriteLine($"screenSize: {screenW}x{screenH}");
        }
        // adding global button to return to home to all panels except homePanel;
        // for every panel little bit different implementation of panels and with different variable changes
        private void AddGlobalButtonToPanel(Panel targetPanel)
        {
            if (targetPanel == homePanel) return;

            Button backToMenu = new Button
            {
                Text = "Menu",
                Font = btnFont,
                Size = new Size(200, 100),
                Location = new Point(screenW / 18, screenH / 7),
                BackColor = btnColor
            };

            Button resetLevel = new Button
            {
                Text = "RESTART",
                Font = btnFont,
                Size = new Size(200, 100),
                Location = new Point(0, 0),
                BackColor = btnColor
            };
            resetLevel.Location = new Point(16 * screenW / 18 - resetLevel.Width / 2, screenH / 7);

            if (targetPanel == levelPanel)
            {
                targetPanel.Controls.Add(resetLevel);
                resetLevel.Click += (s, e) => InitializeLevelScreen(currLevelName);
                resetLevel.Click += (s, e) => this.resetVars();

                backToMenu.Text = "Levels";
                backToMenu.Click += (s, e) => ShowPanel(levelsPanel);
                backToMenu.Click += (s, e) => this.resetVars();
            }
            else if (targetPanel == endLevelPanel)
            {
                backToMenu.Size = new Size(400, 100);
                backToMenu.Location = new Point(screenW / 2 - backToMenu.Width / 2, 2 * screenH / 3 - backToMenu.Height / 2);
                backToMenu.Click += (s, e) => InitializeLevelsScreen();
                backToMenu.Click += (s, e) => ShowPanel(levelsPanel);
                backToMenu.Click += (s, e) => this.resetVars();
                backToMenu.Text = "Back To Levels";
            }
            else if (targetPanel == shopPanel)
            {
                backToMenu.Location = new Point(screenW / 18, screenH / 10 - backToMenu.Height);
                backToMenu.Click += (s, e) =>
                {
                    Console.WriteLine($"[DEBUG TEXTURES] Crate_{crateNames[currCrateSelected]}.png");
                    Storage.selectedBox = $"Crate_{crateNames[currCrateSelected]}.png";
                    Storage.selectedWall = roundTexture ? $"WallRound_{wallNames[currWallSelected]}.png" : $"Wall_{wallNames[currWallSelected]}.png";
                    Storage.selectedEndPoint = $"EndPoint_{endPointNames[currEndPointSelected]}.png";
                    Storage.selectedTextures = $"Ground_{texturesNames[currTexturesSelected]}.png";
                    Console.WriteLine($"[TEXTURES]: box: Crate_{crateNames[currCrateSelected]}.png, wall: Wall_{wallNames[currWallSelected]}.png, EndPoint: EndPoint_{endPointNames[currEndPointSelected]}.png, Textures: Ground_{texturesNames[currTexturesSelected]}.png");
                };
                backToMenu.Click += (s, e) => ShowPanel(homePanel);
            }
            else
            {
                backToMenu.Click += (s, e) => ShowPanel(homePanel);
            }
            targetPanel.Controls.Add(backToMenu);
            backToMenu.BringToFront();
        }
        // process of making panels for different screens
        private void InitializePanels()
        {
            // Home, Levels, Level, Endlevel, profile, shop

            // Home Panel
            homePanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(homePanel);

            // Levels Panel
            levelsPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(levelsPanel);

            // Level Panel
            levelPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(levelPanel);

            // End Level Panel
            endLevelPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(endLevelPanel);

            // Profile Panel
            profilePanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(profilePanel);

            // Shop Panel
            shopPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(shopPanel);

            // Add controls to panels (// Home, Levels, Level, Endlevel, profile, shop)
            InitializeHomeScreen();
            //InitializeLevelsScreen();
            //InitializeLevelScreen();
            //InitializeEndLevelScreen();
            //InitializeProfileScreen();
            InitializeShopScreen();
            InitializeLabels();

            AddHeaderToPanel(homePanel);
            AddHeaderToPanel(levelPanel);
            AddHeaderToPanel(levelsPanel);
            AddHeaderToPanel(profilePanel);
            AddHeaderToPanel(shopPanel);
            AddHeaderToPanel(endLevelPanel);

        }
        // Initializetion of home screen with design and all components
        private void InitializeHomeScreen()
        {
            // Button styles
            Size buttonSize = new Size(screenW / 5, screenH / 10);
            int spacing = screenH / 10;
            int startY = screenH / 6;
            int centerX = (this.ClientSize.Width - buttonSize.Width) / 2; // 300 is button width

            // Play Button
            Button btnPlay = new Button
            {
                Text = "Play",
                Font = btnFont,
                Size = buttonSize,
                Location = new Point(centerX, startY),
                BackColor = Color.LightGreen
            };
            btnPlay.Click += (s, e) => InitializeLevelsScreen();
            btnPlay.Click += (s, e) => ShowPanel(levelsPanel);

            // Profile Button
            Button btnProfile = new Button
            {
                Text = "Profile",
                Font = btnFont,
                Size = buttonSize,
                Location = new Point(centerX, 2 * startY),
                BackColor = Color.LightSkyBlue
            };
            btnProfile.Click += (s, e) => InitializeProfileScreen();
            btnProfile.Click += (s, e) => ShowPanel(profilePanel);

            // Shop Button
            Button btnShop = new Button
            {
                Text = "Shop",
                Font = btnFont,
                Size = buttonSize,
                Location = new Point(centerX, 3 * startY),
                BackColor = Color.Khaki
            };
            btnShop.Click += (s, e) => ShowPanel(shopPanel);

            // Exit Button
            Button btnExit = new Button
            {
                Text = "Exit",
                Font = btnFont,
                Size = buttonSize,
                Location = new Point(centerX, 4 * startY),
                BackColor = Color.IndianRed
            };
            btnExit.Click += (s, e) => this.Close();

            // Add buttons to panel
            homePanel.Controls.Add(btnPlay);
            homePanel.Controls.Add(btnProfile);
            homePanel.Controls.Add(btnShop);
            homePanel.Controls.Add(btnExit);

            this.Controls.Add(homePanel);
        }
        // method to always show header panel for every panel with a label sokoban
        private void AddHeaderToPanel(Panel parentPanel)
        {

            // Create the header rectangle as a panel
            // Create the header panel
            Panel headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Width = screenW,
                Height = screenH / 10, // Adjust as needed
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(177, 240, 247)
            };

            // Create the label
            headerLabel = new System.Windows.Forms.Label
            {
                Text = "SOKOBAN",
                ForeColor = Color.Black,
                Font = new Font("Arial", 24, FontStyle.Bold), // adjust font as needed
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            // Add label to header, header to parent panel
            headerPanel.Controls.Add(headerLabel);
            parentPanel.Controls.Add(headerPanel);

            // Ensure header is at the front
            AddGlobalButtonToPanel(parentPanel);

        }
        // Initialization of levels using loop, buttons to select level, labels for best times and steps
        private void InitializeLevelsScreen()
        {
            // Clear previous controls (except the header)
            levelsPanel.Controls.Clear();
            AddHeaderToPanel(levelsPanel);

            int numLevels = 10;
            Size btnSize = new Size(screenW / 8, screenH / 8);
            int spacing = screenH / 8;
            int startX = screenH / 7;
            int[] rowY = { 7 * screenH / 20, 3 * screenH / 5 }; // Y positions for two rows


            for (int i = 0; i < numLevels; i++)
            {
                var results = database.GetLevelTimesAndStepsByPlayer(currSave, $"level{i+1}");
                Console.WriteLine($"[InicializeLevelsScreen] level {i + 1}: for save ID {currSave}, data {results[0]}");
                // Use the current save/profile
                int stepPb = results[0].Steps;
                double timePb = results[0].Time;

                int row = i < 5 ? 0 : 1; // First 5 buttons in row 0, next 5 in row 1
                int col = i % 5 + 1;     // Columns: 1 to 5 in each row

                Button levelBtn = new Button
                {
                    Text = $"LEVEL {i + 1}",
                    Font = btnFont,
                    Size = btnSize,
                    Location = new Point((startX + spacing) * col, rowY[row]),
                    BackColor = btnColor
                };

                Font labelFont = new Font("Segoe UI", 12, FontStyle.Bold);
                Color foreColor = Color.Black;

                // Create unique labels for each level
                var labelStep = new System.Windows.Forms.Label
                {
                    Text = $"Best step count: 0",
                    Font = labelFont,
                    ForeColor = foreColor,
                    AutoSize = true,
                    Location = new Point(levelBtn.Location.X, levelBtn.Location.Y + btnSize.Height)
                };
                levelsPanel.Controls.Add(labelStep);

                var labelTime = new System.Windows.Forms.Label
                {
                    Text = $"Best time: 0",
                    Font = labelFont,
                    ForeColor = foreColor,
                    AutoSize = true,
                    Location = new Point(levelBtn.Location.X, levelBtn.Location.Y + btnSize.Height + labelStep.Height)
                };
                levelsPanel.Controls.Add(labelTime);

                labelStep.Text = $"Best step count: {stepPb}";
                labelTime.Text = $"Best time: {timePb:F3} s"; // Format time to 3 decimal places
                string levelName = $"level{i + 1}";

                levelBtn.Click += (s, e) => InitializeLevelScreen(levelName);
                levelBtn.Click += (s, e) => ShowPanel(levelPanel);

                levelsPanel.Controls.Add(levelBtn);

                stepPb = 0;
                timePb = 0;
            }
        }
        // Main inicialization of the program, preparing level for use / movement / display
        private void InitializeLevelScreen(string level)
        {
            //Console.WriteLine($"Initializing Level Screen... with level: {level}");
            // get level
            currLevelName = level;
            this.prepareLevel(level);

        }
        // Inicialization of labels for end level screen
        private void InitializeLabels()
        {
            label3 = new System.Windows.Forms.Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.Black
            };
            endLevelPanel.Controls.Add(label3);

            label4 = new System.Windows.Forms.Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.Black
            };
            endLevelPanel.Controls.Add(label4);

            label5 = new System.Windows.Forms.Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 42, FontStyle.Bold),
                ForeColor = Color.Black
            };
            endLevelPanel.Controls.Add(label5);
        }
        // Inicialization of end level screen with final time, steps and congratulation
        private void InitializeEndLevelScreen(string levelName, double time, int stepsCount)
        {
            // Update label texts
            label3.Text = $"Final steps count: {stepsCount}";
            label4.Text = $"Final time: {time}";
            label5.Text = $"Congratulation for copleting {levelName}";

            // Recalculate positions after text update (Width changes)
            label3.Location = new Point((screenW - label3.Width) / 2, screenH / 3 + 2 * label3.Height);
            label4.Location = new Point((screenW - label4.Width) / 2, 2 * screenH / 5 + 2 * label4.Height);
            label5.Location = new Point((screenW - label5.Width) / 2, screenH / 5);
        }
        // Inicialization of profile screen - can change save to upload to different save - with 3 save buttons, changing color of current save
        private void InitializeProfileScreen()
        {
            bool firstLoad = true;
            for (var i = 0; i < 3; i++) 
            {
                int saveId = i + 1;
                Button profileBtn = new Button
                {
                    Text = $"Save{saveId}",
                    Font = btnFont,
                    Size = new Size(screenW / 5, screenH / 6),
                    BackColor = btnColor,
                    Tag = saveId
                };

                if (firstLoad && i == 0)
                {
                    profileBtn.BackColor = Color.LightGreen; // Set current save to the first one on first load
                    firstLoad = false;
                }

                saveButtons.Add(profileBtn);

                profileBtn.Location = new Point((saveId) * screenW / 4 - profileBtn.Width / 2, screenH / 2 - profileBtn.Height / 2);
                profileBtn.Click += (s, e) => currSave = saveId;
                profileBtn.Click += (s, e) => UpdateButtonColors();
                profileBtn.Click += (s, e) => Console.WriteLine($"Current save set to: {currSave}");

                profilePanel.Controls.Add(profileBtn);
            }
        }
        // Method to change save button colors
        private void UpdateButtonColors()
        {
            foreach (Button btn in saveButtons)
            {
                Console.WriteLine($"Updating button color for save ID: {btn.Tag}");
                int buttonSaveId = (int)btn.Tag;
                btn.BackColor = (buttonSaveId == currSave)
                    ? Color.LightGreen
                    : btnColor;
            }
        }
        // Method for updating shop buttons color
        private void UpdateShopButtons()
        {
            Color selectedBtn = Color.FromArgb(200 ,195, 255, 153);
            Color notSelectedBtn = Color.FromArgb(120, 255, 102, 102);


            foreach (Button btn in crateTextureList)
            {
                //0: Blue, 1: Beige, 2: Brown, 3: Red, 4: Yellow
                int buttonTextureID = (int)btn.Tag;
                btn.BackColor = (buttonTextureID == currCrateSelected)
                    ? selectedBtn
                    : notSelectedBtn;
            }
            foreach (Button btn in wallTextureList)
            {
                //0: Beige, 1: Black, 2: Brown, 3: Gray
                int buttonTextureID = (int)btn.Tag;
                btn.BackColor = (buttonTextureID == currWallSelected)
                    ? selectedBtn
                    : notSelectedBtn;
            }
            foreach (Button btn in endPointTextureList)
            {
                //0: Blue, 1: Beige, 2: Brown, 3: Red, 4: Yellow
                int buttonTextureID = (int)btn.Tag;
                btn.BackColor = (buttonTextureID == currEndPointSelected)
                    ? selectedBtn
                    : notSelectedBtn;
            }
            foreach (Button btn in texturesTextureList)
            {
                //0: Blue, 1: Beige, 2: Brown, 3: Red, 4: Yellow
                int buttonTextureID = (int)btn.Tag;
                btn.BackColor = (buttonTextureID == currTexturesSelected)
                    ? selectedBtn
                    : notSelectedBtn;
            }

        }
        // Method to update picture box in shop screen - to show what texture player selected
        private Image UpdatePreviewPictureBox(int size ,string textureName, bool round = true)
        {
            if (round)
            {
                roundTexture = true;
                textureName = textureName.Replace("Wall_", "WallRound_");
            }

            try 
            {
                Image textureImage = Storage.getImage(textureName);
                return textureImage;
            }
            catch (Exception ex)
            {
                // If the image is not found, log the error and return a default square
                Console.WriteLine($"Error loading texture: {ex.Message}");
                return Storage.CreateColoredSquare(size, Color.Black);
            }

        }
        // Longest inicialization of this program to show all textures in shop screen, with buttons to select texture, and picture box to show what texture player selected
        private void InitializeShopScreen()
        {
            // Example screen width, adjust as needed
            Size panelSize = new Size(screenW / 5, 4 * screenH / 5);
            int screenWconst = screenW / 5;
            int spacingX = screenW / 25 ;
            int screenHConst = screenH / 8;
            //$"Wall_{wallNames[i]}.png"
            //$"WallRound_{wallNames[i]}.png"

            // Wall panel section
            Panel wallsPanel = new Panel
            {
                Size = panelSize, // Increased height for buttons
                BorderStyle = BorderStyle.FixedSingle,
            };
            wallsPanel.Location = new Point(spacingX + screenWconst * 0, screenHConst);

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

            /////////////////////////////////////////////////////////////////////
            // Crate panel section
            Panel cratePanel = new Panel
            {
                Size = panelSize, // Increased height for buttons
                BorderStyle = BorderStyle.FixedSingle,
            };
            cratePanel.Location = new Point(wallsPanel.Right + spacingX, screenHConst);

            //VARS
            //buttonSize = 7 * buttonSize / 8;
            //$"Crate_{crateNames[i]}.png"
            //$"WallDark_{crateNames[i]}.png"
            int space = cratePanel.Width / 10; 
            int rows = 5;
            int columns = 2;

            for (int i = 0; i < 10; i++)
            {
                int buttonIndex = i / 2;
                // Determine column (0 or 1) and row (0 or 1)
                int col = i % 2;

                // X: 0 => 1/3 of buttonsize, 1 => 5/3 of button size
                int x = (int)((col == 0 ? 1f / 3f : 5f / 3f) * buttonSize);

                // Y: 0 => 2/5 of height, 1 => 4/5 of height
                int y = (int)(cratePanel.Height / 28 + (i - col) * (4 * buttonSize / 6));

                Button crateButton = new Button
                {
                    //Text = $"{i + 1}",
                    Font = btnFont,
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(x, y),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Image = i % 2 == 0 ? Storage.getImage($"Crate_{crateNames[buttonIndex]}.png") : Storage.getImage($"CrateDark_{crateNames[buttonIndex]}.png"),
                    Tag = buttonIndex,
                    FlatStyle = FlatStyle.Standard,

                };
                crateButton.Click += (s, e) => currCrateSelected = buttonIndex;
                crateButton.Click += (s, e) => UpdateShopButtons();
                crateButton.Click += (s, e) => Console.WriteLine($"currCrateSelected {currCrateSelected}, buttonIndex: {buttonIndex}");
                crateTextureList.Add(crateButton);

                cratePanel.Controls.Add(crateButton);

            }

            ////////////////////////////////////////////////////////////////////
            // EndPoint panel section
            Panel endPointPanel = new Panel
            {
                 Size = panelSize, // Increased height for buttons
                 BorderStyle = BorderStyle.FixedSingle,
            };
            endPointPanel.Location = new Point(cratePanel.Right + spacingX, screenHConst);

            for (int i = 0; i < 8; i++)
            {
                int buttonIndex = i;
                // Determine column (0 or 1) and row (0 or 1)
                int col = i % 2;

                // X: 0 => 1/3 of buttonsize, 1 => 5/3 of button size
                int x = (int)((col == 0 ? 1f / 3f : 5f / 3f) * buttonSize);

                // Y: 0 => 2/5 of height, 1 => 4/5 of height
                int y = (int)(cratePanel.Height / 8 + (i - col) * (4 * buttonSize / 6));

                Button endPointButton = new Button
                {
                    //Text = $"{i + 1}",
                    Font = btnFont,
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(x, y),
                    Image = Storage.getImage($"EndPoint_{endPointNames[buttonIndex]}.png"),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Tag = buttonIndex,
                    FlatStyle = FlatStyle.Standard,

                };
                endPointButton.Click += (s, e) => currEndPointSelected = buttonIndex;
                endPointButton.Click += (s, e) => UpdateShopButtons();
                endPointButton.Click += (s, e) => Console.WriteLine($"currEndPointSelected {currEndPointSelected}, buttonIndex: {buttonIndex}");
                endPointTextureList.Add(endPointButton);

                endPointPanel.Controls.Add(endPointButton);

            }

            ////////////////////////////////////////////////////////////////////
            // Texture panel section
            Panel texturePanel = new Panel
            {
                Size = panelSize, // Increased height for buttons
                BorderStyle = BorderStyle.FixedSingle,
            };
            texturePanel.Location = new Point(endPointPanel.Right + spacingX, screenHConst);

            for (int i = 0; i < 8; i++)
            {
                int buttonIndex = i / 2;
                // Determine column (0 or 1) and row (0 or 1)
                int col = i % 2;

                // X: 0 => 1/3 of buttonsize, 1 => 5/3 of button size
                int x = (int)((col == 0 ? 1f / 3f : 5f / 3f) * buttonSize);

                // Y: 0 => 2/5 of height, 1 => 4/5 of height
                int y = (int)(cratePanel.Height / 8 + (i - col) * (4 * buttonSize / 6));

                Button texturesButton = new Button
                {
                    //Text = $"{i + 1}",
                    Font = btnFont,
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(x, y),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Image = i % 2 == 0 ? Storage.getImage($"Ground_{texturesNames[buttonIndex]}.png") : Storage.getImage($"GroundGravel_{texturesNames[buttonIndex]}.png"),
                    Tag = buttonIndex,
                    FlatStyle = FlatStyle.Standard,

                };
                texturesButton.Click += (s, e) => currTexturesSelected = buttonIndex;
                texturesButton.Click += (s, e) => UpdateShopButtons();
                texturesButton.Click += (s, e) => Console.WriteLine($"currEndPointSelected {currEndPointSelected}, buttonIndex: {buttonIndex}");
                texturesTextureList.Add(texturesButton);

                texturePanel.Controls.Add(texturesButton);

            }

            // Add panels to shopPanel
            shopPanel.Controls.Add(wallsPanel);
            shopPanel.Controls.Add(cratePanel);
            shopPanel.Controls.Add(endPointPanel);
            shopPanel.Controls.Add(texturePanel);

            // Add shopPanel to the form
            this.Controls.Add(shopPanel);

            // need to update it at the first inicialization
            UpdateShopButtons();
        }
        // Method to prepare level, adding textures to controls, adding movement ability, preparing labels
        private void prepareLevel(string mapName)
        {
            // get level data from db
            int[,] arr = database.getLevel(mapName);
            // depends on level create objs
            maps.addObjToList(arr, arr.GetLength(0));
            // make them able to move
            maps.AddToControls(levelPanel);
            AddGlobalButtonToPanel(levelPanel);
            AddHeaderToPanel(levelPanel);
            // reset lists and prepare components
            boxes = maps.boxes;
            player = maps.player;
            walls = maps.walls;
            finalDest = maps.finalDest;
            // movement count label
            label1 = new System.Windows.Forms.Label
            {
                Text = "Počet kroků: 0",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(screenW / 18, screenH / 4),
                AutoSize = true,
                ForeColor = Color.Black
            };
            levelPanel.Controls.Add(label1);

            label2 = new System.Windows.Forms.Label
            {
                Text = "Počet kroků: 0.000",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(screenW / 18, 11 * screenH / 28),
                AutoSize = true,
                ForeColor = Color.Black
            };
            levelPanel.Controls.Add(label2);

        }
        // Method to show only one panel depending on application movement - buttons
        private void ShowPanel(Panel targetPanel)
        {
            foreach (Control c in this.Controls.OfType<Panel>())
                c.Visible = false;
            targetPanel.Visible = true;
        }
        // Method to handle key presses for player movement, collision detection, step counting, win checking
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (levelPanel.Visible)
            {
                int dif = 0;

                if (e.KeyCode == Keys.Left)
                {
                    //player moved left
                    player.moveLeft();
                    dif++;

                    if (maps.collided_pw(player, walls))
                    {
                        //player moved back to the original position
                        player.moveRight(true);
                        dif--;
                    }
                    //check if player collided with box
                    Box a = maps.collided_pb(player, boxes);     

                    //yes they collided so they moved
                    if (a != null)                              
                    {
                        //box moved left
                        a.moveLeft();
                        dif++;
                        //check if box collided with box
                        Box b = maps.collided_bb(a, boxes);      
                        //check if box collided with wall
                        Box c = maps.collided_bw(a, walls);      
                        //yes they collided (wall / box) so they moved back to the original position
                        if (b != null || c != null)             
                        {
                            a.moveRight();
                            player.moveRight(true);
                            dif--;
                        }
                        dif--;
                    }
                    // inceresement of steps count
                    stepsCount += dif;
                    

                }
                else if (e.KeyCode == Keys.Right)
                {
                    
                    player.moveRight();
                    dif++;

                    if (maps.collided_pw(player, walls))
                    {
                        player.moveLeft(true);
                        dif--;
                    }                                           
                    Box a = maps.collided_pb(player, boxes);     

                    if (a != null)                              
                    {
                        a.moveRight();
                        dif++;
                        Box b = maps.collided_bb(a, boxes);      
                        Box c = maps.collided_bw(a, walls);      
                        if (b != null || c != null)             
                        {
                            a.moveLeft();
                            player.moveLeft(true);
                            dif--;
                        }
                        dif--;

                    }
                    stepsCount += dif;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    player.moveUp();
                    dif++;

                    if (maps.collided_pw(player, walls))
                    {
                        player.moveDown(true);
                        dif--;
                    }
                    Box a = maps.collided_pb(player, boxes);   

                    if (a != null)                              
                    {
                        a.moveUp();
                        dif++;
                        Box b = maps.collided_bb(a, boxes);
                        Box c = maps.collided_bw(a, walls); 
                        if (b != null || c != null)          
                        {
                            a.moveDown();
                            player.moveDown(true);
                            dif--;
                        }
                        dif--;

                    }
                    stepsCount += dif;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    player.moveDown();
                    dif++;
                    if (maps.collided_pw(player, walls))
                    {
                        player.moveUp(true);
                        dif--;
                    }
                    Box a = maps.collided_pb(player, boxes);

                    if (a != null)
                    {
                        a.moveDown();
                        dif++;
                        Box b = maps.collided_bb(a, boxes);
                        Box c = maps.collided_bw(a, walls);
                        if (b != null || c != null)
                        {
                            a.moveUp();
                            player.moveUp(true);
                            dif--;
                        }
                        dif--;
                    }
                    stepsCount += dif;
                }

                if (stepsCount > 0) {
                    stopwatch.Start();
                }

                if (maps.checkWin(boxes, finalDest))
                {
                    stopwatch.Stop();

                    ShowPanel(endLevelPanel);
                    // get curr level best time to compare

                    var bestResult = database.GetLevelTimesAndStepsByPlayer(currSave, currLevelName);
                    double bestTime = bestResult[0].Time;
                    bestTime = Convert.ToDouble(bestTime.ToString("N3"));

                    var currTime = stopwatch.Elapsed.TotalSeconds;
                    currTime = Convert.ToDouble(currTime.ToString("N3"));

                    Console.WriteLine($"[checkWin] Current time for level {currLevelName} for save ID {currSave}: {currTime}");

                    // time is primary
                    Console.WriteLine($"[checkWin] Best result for level {currLevelName} for save ID {currSave}: {bestResult[0]}, bestTime {stopwatch.Elapsed.TotalSeconds}");
                    if (bestTime > currTime || bestTime == 0.0)
                    {
                        database.SetLevelTimeAndSteps(currSave, currLevelName, currTime, stepsCount);
                        Console.WriteLine($"[checkWin] Inserting: level: {currLevelName}, Time: {currTime}, steps: {stepsCount}");
                    }
                    else
                    {
                        Console.WriteLine($"[checkWin] Not inserting: level: {currLevelName}, Time: {currTime}, steps: {stepsCount} - better time already exists");
                    }

                        //string levelName, double time, int stepsCount
                        InitializeEndLevelScreen(currLevelName, currTime, stepsCount);
                    //this.resetVars();
                }

                label1.Text = $"Počet kroků: {stepsCount}";
                label2.Text = $"Čas: {stopwatch.Elapsed.TotalSeconds:F3} s";

            }
        }
        // Method to reset variables for new level start or restart
        private void resetVars()
        {
            stepsCount = 0;
            stopwatch.Reset();
            
        }
    }
}