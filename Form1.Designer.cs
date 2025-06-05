using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using static System.Windows.Forms.AxHost;
using System.Numerics;
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

        // adding global button to return to home to all panels except homePanel
        private void AddGlobalButtonToPanel(Panel targetPanel)
        {
            if (targetPanel == homePanel) return;

            Button backToMenu = new Button
            {
                Text = "Menu",
                Font = btnFont,
                Size = new Size(200, 100),
                Location = new Point(100, 125),
                BackColor = btnColor
            };
            backToMenu.Click += (s, e) => ShowPanel(homePanel);
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
            //InitializeLevelScreen();
            InitializeEndLevelScreen();
            InitializeProfileScreen();
            InitializeShopScreen();

            AddHeaderToPanel(homePanel);
            AddHeaderToPanel(levelPanel);
            AddHeaderToPanel(levelsPanel);
            AddHeaderToPanel(profilePanel);
            AddHeaderToPanel(shopPanel);

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
            }
            ;

            // Add label to header, header to parent panel
            parentPanel.Controls.Add(headerPanel);
            headerPanel.Controls.Add(headerLabel);
            // Ensure header is at the front
            headerPanel.BringToFront();
            headerLabel.BringToFront();
            AddGlobalButtonToPanel(parentPanel);

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
            level1.Click += (s, e) => InitializeLevelScreen(Text.ToLower());
            level1.Click += (s, e) => ShowPanel(levelPanel);

            Button level2 = new Button
            {
                Text = "LEVEL 2",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 2, 7 * screenH / 20),
                BackColor = btnColor
            };
            level1.Click += (s, e) => ShowPanel(levelPanel);
            level1.Click += (s, e) => InitializeLevelScreen(Text.ToLower());

            Button level3 = new Button
            {
                Text = "LEVEL 3",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 3, 7 * screenH / 20),
                BackColor = btnColor
            };
            level1.Click += (s, e) => ShowPanel(levelPanel);
            level1.Click += (s, e) => InitializeLevelScreen(Text.ToLower());

            Button level4 = new Button
            {
                Text = "LEVEL 4",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 4, 7 * screenH / 20),
                BackColor = btnColor
            };
            level1.Click += (s, e) => ShowPanel(levelPanel);
            level1.Click += (s, e) => InitializeLevelScreen(Text.ToLower());

            Button level5 = new Button
            {
                Text = "LEVEL 5",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 5, 7 * screenH / 20),
                BackColor = btnColor
            };
            level1.Click += (s, e) => ShowPanel(levelPanel);
            level1.Click += (s, e) => InitializeLevelScreen(Text.ToLower());

            Button level6 = new Button
            {
                Text = "LEVEL 6",
                Font = btnFont,
                Size = btnSize,
                // H / 2 + H / 10 = 3 H / 5
                Location = new Point((startX + spacing), 3 * screenH / 5),
                BackColor = btnColor
            };
            level1.Click += (s, e) => ShowPanel(levelPanel);
            level1.Click += (s, e) => InitializeLevelScreen(Text.ToLower());

            Button level7 = new Button
            {
                Text = "LEVEL 7",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 2, 3 * screenH / 5),
                BackColor = btnColor
            };
            level1.Click += (s, e) => ShowPanel(levelPanel);
            level1.Click += (s, e) => InitializeLevelScreen(Text.ToLower());

            Button level8 = new Button
            {
                Text = "LEVEL 8",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 3, 3 * screenH / 5),
                BackColor = btnColor
            };
            level1.Click += (s, e) => ShowPanel(levelPanel);
            level1.Click += (s, e) => InitializeLevelScreen(Text.ToLower());

            Button level9 = new Button
            {
                Text = "LEVEL 9",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 4, 3 * screenH / 5),
                BackColor = btnColor
            };
            level1.Click += (s, e) => ShowPanel(levelPanel);
            level1.Click += (s, e) => InitializeLevelScreen(Text.ToLower());

            Button level10 = new Button
            {
                Text = "LEVEL 10",
                Font = btnFont,
                Size = btnSize,
                Location = new Point((startX + spacing) * 5, 3 * screenH / 5),
                BackColor = btnColor
            };
            level1.Click += (s, e) => ShowPanel(levelPanel);
            level1.Click += (s, e) => InitializeLevelScreen(Text.ToLower());

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
            //levelsPanel.Controls.Add();
            this.Controls.Add(levelsPanel);
        }

        private void InitializeLevelScreen(string level)
        {
            Console.WriteLine("Initializing Level Screen...");
            // get level
            this.prepareLevel(level);

        }
        private void InitializeEndLevelScreen()
        {

        }
        private void InitializeProfileScreen()
        {

        }
        private void InitializeShopScreen()
        {

        }

        private void prepareLevel(string mapName)
        {
            // get level data from db
            int[,] arr = database.getLevel(mapName);
            // depends on level create objs
            maps.addObjToList(arr);
            // make them able to move
            maps.AddToControls(levelPanel);
            AddGlobalButtonToPanel(levelPanel);
            AddHeaderToPanel(levelPanel);
            // reset lists and prepare components
            boxes = maps.boxes;
            player = maps.player;
            walls = maps.walls;
            finalDest = maps.finalDest;
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

                if (e.KeyCode == Keys.Left)
                {
                    player.moveLeft();                          //player moved left
                    if (maps.collided_pw(player, walls))
                    {
                        player.moveRight(true);                 //player moved back to the original position
                    }
                    Box a = maps.collided_pb(player, boxes);     //check if player collided with box

                    if (a != null)                              //yes they collided so they moved
                    {
                        a.moveLeft();                           //box moved left
                        Box b = maps.collided_bb(a, boxes);      //check if box collided with box
                        Box c = maps.collided_bw(a, walls);      //check if box collided with wall
                        if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
                        {
                            a.moveRight();
                            player.moveRight(true);
                        }

                    }
                    maps.checkWin(boxes, finalDest);
                }
                else if (e.KeyCode == Keys.Right)
                {
                    player.moveRight();                         //player moved left
                    if (maps.collided_pw(player, walls))
                    {
                        player.moveLeft(true);                  //player moved back to the original position
                    }                                           //player moved right
                    Box a = maps.collided_pb(player, boxes);     //check if player collided with box

                    if (a != null)                              //yes they collided so they moved
                    {
                        a.moveRight();                          //box moved right
                        Box b = maps.collided_bb(a, boxes);      //check if box collided with box
                        Box c = maps.collided_bw(a, walls);      //check if box collided with wall
                        if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
                        {
                            a.moveLeft();
                            player.moveLeft(true);
                        }

                    }
                    maps.checkWin(boxes, finalDest);
                }
                else if (e.KeyCode == Keys.Up)
                {
                    player.moveUp();                            //player moved left
                    if (maps.collided_pw(player, walls))
                    {
                        player.moveDown(true);                  //player moved back to the original position
                    }                                           //player moved up
                    Box a = maps.collided_pb(player, boxes);     //check if player collided with box

                    if (a != null)                              //yes they collided so they moved
                    {
                        a.moveUp();                             //box moved left
                        Box b = maps.collided_bb(a, boxes);      //check if box collided with box
                        Box c = maps.collided_bw(a, walls);      //check if box collided with wall
                        if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
                        {
                            a.moveDown();
                            player.moveDown(true);
                        }

                    }
                    maps.checkWin(boxes, finalDest);
                }
                else if (e.KeyCode == Keys.Down)
                {
                    player.moveDown();                          //player moved left
                    if (maps.collided_pw(player, walls))
                    {
                        player.moveUp(true);                    //player moved back to the original position
                    }                                           //player moved down
                    Box a = maps.collided_pb(player, boxes);     //check if player collided with box

                    if (a != null)                              //yes they collided so they moved
                    {
                        a.moveDown();                           //box moved down
                        Box b = maps.collided_bb(a, boxes);      //check if box collided with box
                        Box c = maps.collided_bw(a, walls);      //check if box collided with wall
                        if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
                        {
                            a.moveUp();
                            player.moveUp(true);
                        }
                    }
                    maps.checkWin(boxes, finalDest);
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    Application.Exit();
                }

                maps.checkWin(boxes, finalDest);
            
            }
        }
    }
}