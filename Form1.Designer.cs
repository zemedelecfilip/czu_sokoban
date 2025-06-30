using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using static System.Windows.Forms.AxHost;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        public PictureBox homeScreenRect;
        public Font btnFont = new Font("Segoe UI", 18, FontStyle.Bold);
        public Color btnColor = Color.LightSkyBlue;
        public int stepsCount = 0;
        Stopwatch stopwatch = new Stopwatch();
        public string currLevelName = "";
        public int currSave = 1;
        private List<Button> saveButtons = new List<Button>();

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
            if (targetPanel == levelPanel)
            {
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


        private void InitializeLevelScreen(string level)
        {
            //Console.WriteLine($"Initializing Level Screen... with level: {level}");
            // get level
            currLevelName = level;
            this.prepareLevel(level);

        }

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
        private void InitializeEndLevelScreen(string levelName, double time, int stepsCount)
        {
            // Update label texts
            label3.Text = $"Finální počet kroků: {stepsCount}";
            label4.Text = $"Výsledný čas: {time}";
            label5.Text = $"Gratulace za dokončení {levelName}";

            // Recalculate positions after text update (Width changes)
            label3.Location = new Point((screenW - label3.Width) / 2, screenH / 3 + 2 * label3.Height);
            label4.Location = new Point((screenW - label4.Width) / 2, 2 * screenH / 5 + 2 * label4.Height);
            label5.Location = new Point((screenW - label5.Width) / 2, screenH / 5);
        }
        private void InitializeProfileScreen()
        {
            for (var i = 0; i < 3; i++) 
            {
                int saveId = i + 1;
                Console.WriteLine($"Adding level button {saveId} to profile panel");
                Button profileBtn = new Button
                {
                    Text = $"Save{saveId}",
                    Font = btnFont,
                    Size = new Size(screenW / 5, screenH / 6),
                    BackColor = btnColor,
                    Tag = saveId
                };

                profileBtn.Location = new Point((saveId) * screenW / 4 - profileBtn.Width / 2, screenH / 2 - profileBtn.Height / 2);
                profileBtn.Click += (s, e) => currSave = saveId;
                profileBtn.Click += (s, e) => UpdateButtonColors();
                profileBtn.Click += (s, e) => Console.WriteLine($"Current save set to: {currSave}");

                profilePanel.Controls.Add(profileBtn);
            }
        }
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

        private void InitializeShopScreen()
        {

        }
        private void prepareLevel(string mapName)
        {
            // get level data from db
            int[,] arr = database.getLevel(mapName);
            //Console.WriteLine($"Preparing level: {mapName} with data: {arr.GetLength(0)}x{arr.GetLength(1)}");
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

                    // time is primary
                    if (bestResult[0].Time < stopwatch.Elapsed.TotalSeconds)
                    {
                        database.SetLevelTimeAndSteps(currSave, currLevelName, stopwatch.Elapsed.TotalSeconds, stepsCount);
                        Console.WriteLine($"[checkWin] Inserting: level: {currLevelName}, Time: {stopwatch.Elapsed.TotalSeconds}, steps: {stepsCount}");
                    }
                    else
                    {
                        Console.WriteLine($"[checkWin] Not inserting: level: {currLevelName}, Time: {stopwatch.Elapsed.TotalSeconds}, steps: {stepsCount} - better time already exists");
                    }

                        //string levelName, double time, int stepsCount
                        InitializeEndLevelScreen(currLevelName, stopwatch.Elapsed.TotalSeconds, stepsCount);
                    //this.resetVars();
                }

                label1.Text = $"Počet kroků: {stepsCount}";
                label2.Text = $"Čas: {stopwatch.Elapsed.TotalSeconds:F3} s";

            }
        }

        private void resetVars()
        {
            stepsCount = 0;
            stopwatch.Reset();
            
        }
    }
}