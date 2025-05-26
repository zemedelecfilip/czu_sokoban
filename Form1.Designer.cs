using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using static System.Windows.Forms.AxHost;
//all textures from https://opengameart.org/content/sokoban-pack

namespace czu_sokoban
{
    partial class Form1
    {
        public int screenW = Storage.screenWidth;
        public int screenH = Storage.screenHeight;

        // deklarování panelů pro různé obrazovky
        // Home, Levels, Level, Endlevel, profile, shop
        private Panel homePanel;
        private Panel levelsPanel;
        private Panel levelPanel;
        private Panel endLevelPanel;
        private Panel profilePanel;
        private Panel shopPanel;
        private System.Windows.Forms.Label label1;
        public PictureBox homeScreenRect;
        public Font btnFont = new Font("Segoe UI", 18, FontStyle.Bold);
        public Color btnColor = Color.LightSkyBlue;

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {

            Button backToMenu = new Button
            {
                Text = "Back To Menu",
                Font = btnFont,
                Size = new Size(200, 100),
                Location = new Point(100, 125),
                BackColor = btnColor
            };
            backToMenu.Click += (s, e) => ShowPanel(homePanel);

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
        }

        private void AddGlobalButtonToPanel(Panel targetPanel)
        {
            // Avoid adding multiple times
           
                Button backToMenu = new Button
                {
                    Text = "Back To Menu",
                    Font = btnFont,
                    Size = new Size(200, 100),
                    Location = new Point(100, 125),
                    BackColor = btnColor
                };
                backToMenu.Click += (s, e) => ShowPanel(homePanel);
                targetPanel.Controls.Add(backToMenu);
                backToMenu.BringToFront();
        }
        private void InitializePanels()
        {
            Button backToMenu = new Button
            {
                Text = "Back To Menu",
                Font = btnFont,
                Size = new Size(200, 100),
                Location = new Point(100, 125),
                BackColor = btnColor
            };
            backToMenu.Click += (s, e) => ShowPanel(homePanel);


            // Home, Levels, Level, Endlevel, profile, shop

            // Home Panel
            homePanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.White,
                Visible = false
            };
            this.Controls.Add(homePanel);

            // Levels Panel
            levelsPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.White,
                Visible = false
            };
            this.Controls.Add(levelsPanel);

            // Level Panel
            levelPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.White,
                Visible = false
            };
            this.Controls.Add(levelPanel);

            // End Level Panel
            endLevelPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.White,
                Visible = false
            };
            this.Controls.Add(endLevelPanel);

            // Profile Panel
            profilePanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.White,
                Visible = false
            };
            this.Controls.Add(profilePanel);

            // Shop Panel
            shopPanel = new Panel
            {
                Size = this.ClientSize,
                BackColor = Color.White,
                Visible = false
            };
            this.Controls.Add(shopPanel);

            // Add controls to panels (// Home, Levels, Level, Endlevel, profile, shop)
            InitializeHomeScreen();
            InitializeLevelsScreen();
            InitializeLevelScreen();
            InitializeEndLevelScreen();
            InitializeProfileScreen();
            InitializeShopScreen();

            AddHeaderToPanel(homePanel);
            AddHeaderToPanel(levelsPanel);
            AddHeaderToPanel(profilePanel);
            AddHeaderToPanel(shopPanel);

            homePanel.Controls.Add(backToMenu);
            levelsPanel.Controls.Add(backToMenu);
            levelPanel.Controls.Add(backToMenu);
            endLevelPanel.Controls.Add(backToMenu);
            profilePanel.Controls.Add(backToMenu);
            shopPanel.Controls.Add(backToMenu);
        }

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

        private void AddHeaderToPanel(Panel parentPanel)
        {

            // Create the header rectangle as a panel
            Panel headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Width = screenW,
                Height = screenH / 10, // Adjust as needed
                Dock = DockStyle.Top,
                BackColor = Color.LightBlue // Choose your color
            };

            // Create the label
            System.Windows.Forms.Label headerLabel = new System.Windows.Forms.Label();
            {
                Text = "SOKOBAN";
                ForeColor = Color.Black;
                //Font = btnFont;
                AutoSize = false;
                //TextAlign = ContentAlignment.MiddleCenter;
                Dock = DockStyle.Fill;
            };

            // Add label to header, header to parent panel
            parentPanel.Controls.Add(headerPanel);
            headerPanel.Controls.Add(headerLabel);
            // Ensure header is at the front
            headerPanel.BringToFront();
            headerLabel.BringToFront();

        }

        private void InitializeLevelsScreen()
        {
            Size btnSize = new Size(screenW / 8, screenH / 8);
            int spacing = screenH / 8;
            int startX = screenH / 7;
            int centerX = (this.ClientSize.Width - btnSize.Width) / 2;

            Button level1 = new Button
            {
                Text = "LEVEL 1",
                Font = btnFont,
                Size = btnSize,
                // H / 4 + H / 10
                Location = new Point(startX + spacing, 7 * screenH / 20),
                BackColor = btnColor
            };
            level1.Click += (s, e) => ShowPanel(homePanel);

            Button level2 = new Button
            {
                Text = "LEVEL 2",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 2, 7 * screenH / 20),
                BackColor = btnColor
            };
            level2.Click += (s, e) => ShowPanel(homePanel);

            Button level3 = new Button
            {
                Text = "LEVEL 3",
                Font = btnFont,
                Size = btnSize  ,
                Location = new Point((startX + spacing) * 3, 7 * screenH / 20),
                BackColor = btnColor
            };
            level3.Click += (s, e) => ShowPanel(homePanel);

            Button level4 = new Button
            {
                Text = "LEVEL 4",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 4, 7 * screenH / 20),
                BackColor = btnColor
            };
            level4.Click += (s, e) => ShowPanel(homePanel);

            Button level5 = new Button
            {
                Text = "LEVEL 5",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 5, 7 * screenH / 20),
                BackColor = btnColor
            };
            level5.Click += (s, e) => ShowPanel(homePanel);

            Button level6 = new Button
            {
                Text = "LEVEL 6",
                Font = btnFont,
                Size = btnSize,
                // H / 2 + H / 10 = 3 H / 5
                Location = new Point((startX + spacing), 3 * screenH / 5),
                BackColor = btnColor
            };
            level6.Click += (s, e) => ShowPanel(homePanel);

            Button level7 = new Button
            {
                Text = "LEVEL 7",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 2, 3 * screenH / 5),
                BackColor = btnColor
            };
            level7.Click += (s, e) => ShowPanel(homePanel);

            Button level8 = new Button
            {
                Text = "LEVEL 8",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 3, 3 * screenH / 5),
                BackColor = btnColor
            };
            level8.Click += (s, e) => ShowPanel(homePanel);

            Button level9 = new Button
            {
                Text = "LEVEL 9",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 4, 3 * screenH / 5),
                BackColor = btnColor
            };
            level9.Click += (s, e) => ShowPanel(homePanel);

            Button level10 = new Button
            {
                Text = "LEVEL 10",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 5, 3 * screenH / 5),
                BackColor = btnColor
            };
            level10.Click += (s, e) => ShowPanel(homePanel);

            levelsPanel.Controls.Add(level1);
            levelsPanel.Controls.Add(level2);
            levelsPanel.Controls.Add(level3);
            levelsPanel.Controls.Add(level4);
            levelsPanel.Controls.Add(level5);
            levelsPanel.Controls.Add(level6);
            levelsPanel.Controls.Add(level7);
            levelsPanel.Controls.Add(level8);
            levelsPanel.Controls.Add(level9);
            levelsPanel.Controls.Add(level10);
            AddGlobalButtonToPanel(levelsPanel);
            //levelsPanel.Controls.Add();
            this.Controls.Add(levelsPanel);
        }

        private void InitializeLevelScreen()
        {
            AddGlobalButtonToPanel(levelPanel);
        }
        private void InitializeEndLevelScreen()
        {
            AddGlobalButtonToPanel(endLevelPanel);
        }
        private void InitializeProfileScreen()
        {
            AddGlobalButtonToPanel(profilePanel);
        }
        private void InitializeShopScreen()
        {
            AddGlobalButtonToPanel(shopPanel);
        }

        private void ShowPanel(Panel targetPanel)
        {
            foreach (Control c in this.Controls.OfType<Panel>())
                c.Visible = false;
            targetPanel.Visible = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (levelPanel.Visible)
            {
                // Handle game controls
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        // Move player left
                        break;
                    case Keys.Right:
                        // Move player right
                        break;
                }
            }
        }
    }
}
