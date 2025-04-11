using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;
using System.Drawing.Drawing2D;
//all textures from https://opengameart.org/content/sokoban-pack

namespace czu_sokoban
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        Maps map = new Maps();
        Player player;
        List<Box> boxes;
        List<Wall> walls;
        List<FinalDestination> finalDest;
        public int[,] current_map;


        public int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        public int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        //menu, levels, level, my profile
        public string tabPage = Storage.tabPage;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            label3 = new Label();
            tabControl1 = new TabControl();
            menu = new TabPage();
            levels = new TabPage();
            button14 = new Button();
            button13 = new Button();
            button12 = new Button();
            button11 = new Button();
            button10 = new Button();
            button9 = new Button();
            button8 = new Button();
            button7 = new Button();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            level = new TabPage();
            stats = new TabPage();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabControl1.SuspendLayout();
            menu.SuspendLayout();
            levels.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Green;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 50);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            button1.Location = new Point(120, 108);
            button1.Name = "button1";
            button1.Size = new Size(84, 42);
            button1.TabIndex = 3;
            button1.Text = "PLAY";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            button2.Location = new Point(120, 166);
            button2.Name = "button2";
            button2.Size = new Size(84, 43);
            button2.TabIndex = 4;
            button2.Text = "STATS";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            button3.Location = new Point(120, 235);
            button3.Name = "button3";
            button3.Size = new Size(84, 53);
            button3.TabIndex = 5;
            button3.Text = "EXIT";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Green;
            label3.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(216, 54);
            label3.TabIndex = 0;
            label3.Text = "SOKOBAN";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(menu);
            tabControl1.Controls.Add(levels);
            tabControl1.Controls.Add(level);
            tabControl1.Controls.Add(stats);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1420, 621);
            tabControl1.TabIndex = 6;
            tabControl1.KeyDown += Form1_KeyDown;
            // 
            // menu
            // 
            menu.Controls.Add(button2);
            menu.Controls.Add(pictureBox1);
            menu.Controls.Add(label3);
            menu.Controls.Add(button3);
            menu.Controls.Add(button1);
            menu.Location = new Point(4, 24);
            menu.Name = "menu";
            menu.Padding = new Padding(3);
            menu.Size = new Size(1412, 593);
            menu.TabIndex = 0;
            menu.Text = "Menu";
            menu.UseVisualStyleBackColor = true;
            // 
            // levels
            // 
            levels.Controls.Add(button14);
            levels.Controls.Add(button13);
            levels.Controls.Add(button12);
            levels.Controls.Add(button11);
            levels.Controls.Add(button10);
            levels.Controls.Add(button9);
            levels.Controls.Add(button8);
            levels.Controls.Add(button7);
            levels.Controls.Add(button6);
            levels.Controls.Add(button5);
            levels.Controls.Add(button4);
            levels.Location = new Point(4, 24);
            levels.Name = "levels";
            levels.Padding = new Padding(3);
            levels.Size = new Size(1412, 593);
            levels.TabIndex = 1;
            levels.Text = "levels";
            levels.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            button14.Location = new Point(19, 22);
            button14.Name = "button14";
            button14.Size = new Size(106, 46);
            button14.TabIndex = 10;
            button14.Text = "back to menu";
            button14.UseVisualStyleBackColor = true;
            button14.Click += button14_Click;
            // 
            // button13
            // 
            button13.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            button13.Location = new Point(884, 302);
            button13.Name = "button13";
            button13.Size = new Size(115, 99);
            button13.TabIndex = 9;
            button13.Text = "LEVEL 10";
            button13.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            button12.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button12.Location = new Point(670, 302);
            button12.Name = "button12";
            button12.Size = new Size(115, 99);
            button12.TabIndex = 8;
            button12.Text = "LEVEL 9";
            button12.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            button11.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button11.Location = new Point(482, 302);
            button11.Name = "button11";
            button11.Size = new Size(115, 99);
            button11.TabIndex = 7;
            button11.Text = "LEVEL 8";
            button11.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            button10.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button10.Location = new Point(324, 302);
            button10.Name = "button10";
            button10.Size = new Size(115, 99);
            button10.TabIndex = 6;
            button10.Text = "LEVEL 7";
            button10.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            button9.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button9.Location = new Point(149, 302);
            button9.Name = "button9";
            button9.Size = new Size(115, 99);
            button9.TabIndex = 5;
            button9.Text = "LEVEL 6";
            button9.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button8.Location = new Point(884, 91);
            button8.Name = "button8";
            button8.Size = new Size(115, 99);
            button8.TabIndex = 4;
            button8.Text = "LEVEL 5";
            button8.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button7.Location = new Point(670, 91);
            button7.Name = "button7";
            button7.Size = new Size(115, 99);
            button7.TabIndex = 3;
            button7.Text = "LEVEL 4";
            button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button6.Location = new Point(482, 91);
            button6.Name = "button6";
            button6.Size = new Size(115, 99);
            button6.TabIndex = 2;
            button6.Text = "LEVEL 3";
            button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button5.Location = new Point(324, 91);
            button5.Name = "button5";
            button5.Size = new Size(115, 99);
            button5.TabIndex = 1;
            button5.Text = "LEVEL 2";
            button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button4.Location = new Point(149, 91);
            button4.Name = "button4";
            button4.Size = new Size(115, 99);
            button4.TabIndex = 0;
            button4.Text = "LEVEL 1";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // level
            // 
            level.Location = new Point(4, 24);
            level.Name = "level";
            level.Size = new Size(1412, 593);
            level.TabIndex = 2;
            level.Text = "level";
            level.UseVisualStyleBackColor = true;
            // 
            // stats
            // 
            stats.Location = new Point(4, 24);
            stats.Name = "stats";
            stats.Size = new Size(1412, 593);
            stats.TabIndex = 3;
            stats.Text = "stats";
            stats.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(1420, 623);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabControl1.ResumeLayout(false);
            menu.ResumeLayout(false);
            menu.PerformLayout();
            levels.ResumeLayout(false);
            ResumeLayout(false);

        }

        // Correct Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            this.calculate_components();
        }

        // Handle Key Events, Main for the game
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (tabControl1.SelectedTab == level)
            {
                this.game_movement(sender, e);
            }
        }
        //LEVEL 1
        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = level;
            current_map = map.MapGrid1;
            this.load_map();
        }
        public void game_movement(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                player.moveLeft();                          //player moved left
                if (map.collided_pw(player, walls))
                {
                    player.moveRight(true);                 //player moved back to the original position
                }
                Box a = map.collided_pb(player, boxes);     //check if player collided with box

                if (a != null)                              //yes they collided so they moved
                {
                    a.moveLeft();                           //box moved left
                    Box b = map.collided_bb(a, boxes);      //check if box collided with box
                    Box c = map.collided_bw(a, walls);      //check if box collided with wall
                    if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
                    {
                        a.moveRight();
                        player.moveRight(true);
                    }

                }
                map.checkWin(boxes, finalDest);
            }
            else if (e.KeyCode == Keys.Right)
            {
                player.moveRight();                         //player moved left
                if (map.collided_pw(player, walls))
                {
                    player.moveLeft(true);                  //player moved back to the original position
                }                                           //player moved right
                Box a = map.collided_pb(player, boxes);     //check if player collided with box

                if (a != null)                              //yes they collided so they moved
                {
                    a.moveRight();                          //box moved right
                    Box b = map.collided_bb(a, boxes);      //check if box collided with box
                    Box c = map.collided_bw(a, walls);      //check if box collided with wall
                    if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
                    {
                        a.moveLeft();
                        player.moveLeft(true);
                    }

                }
                map.checkWin(boxes, finalDest);
            }
            else if (e.KeyCode == Keys.Up)
            {
                player.moveUp();                            //player moved left
                if (map.collided_pw(player, walls))
                {
                    player.moveDown(true);                  //player moved back to the original position
                }                                           //player moved up
                Box a = map.collided_pb(player, boxes);     //check if player collided with box

                if (a != null)                              //yes they collided so they moved
                {
                    a.moveUp();                             //box moved left
                    Box b = map.collided_bb(a, boxes);      //check if box collided with box
                    Box c = map.collided_bw(a, walls);      //check if box collided with wall
                    if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
                    {
                        a.moveDown();
                        player.moveDown(true);
                    }

                }
                map.checkWin(boxes, finalDest);
            }
            else if (e.KeyCode == Keys.Down)
            {
                player.moveDown();                          //player moved left
                if (map.collided_pw(player, walls))
                {
                    player.moveUp(true);                    //player moved back to the original position
                }                                           //player moved down
                Box a = map.collided_pb(player, boxes);     //check if player collided with box

                if (a != null)                              //yes they collided so they moved
                {
                    a.moveDown();                           //box moved down
                    Box b = map.collided_bb(a, boxes);      //check if box collided with box
                    Box c = map.collided_bw(a, walls);      //check if box collided with wall
                    if (b != null || c != null)             //yes they collided (wall / box) so they moved back to the original position
                    {
                        a.moveUp();
                        player.moveUp(true);
                    }
                }
                map.checkWin(boxes, finalDest);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            map.checkWin(boxes, finalDest);
        }

        public void calculate_components()
        {
            //menu components
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Size = new Size(screenWidth, screenHeight / 10);

            label3.Size = new Size(screenWidth / 5, screenHeight / 6);
            label3.Location = new Point(screenWidth / 2 - label3.Width / 2, pictureBox1.Height / 2 - label3.Height / 2);

            ClientSize = new Size(screenWidth, screenHeight);

            tabControl1.Location = new Point(0, 0);
            tabControl1.Size = new Size(screenWidth, screenHeight);
            //
            // Buttons
            //
            int menuBtnWidth = screenWidth / 4;
            int menuBtnHeight = screenHeight / 7;
            int menuBtnX = screenWidth / 2 - menuBtnWidth / 2;
            //int menuBtnY = (screenHeight - pictureBox1.Height) / 4 - menuBtnHeight / 2;
            Button[] menuButtons = { button1, button2, button3 };

            
            for (int i = 1; i < menuButtons.Length + 1; i++)
            {
                menuButtons[i - 1].Size = new Size(menuBtnWidth, menuBtnHeight);
                menuButtons[i - 1].Location = new Point(menuBtnX, (screenHeight * i - pictureBox1.Height) / 4 - button1.Height / 2);
                //menuButtons[i - 1].FlatStyle = FlatStyle.Flat;
                menuButtons[i - 1].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            }

            Button[] levelsButtons = { button4, button5, button6, button7, button8, button9, button10, button11, button12, button13 };

            //AI GENERATED + small changes
            int columns = 5;
            int rows = 2;
            int totalButtons = levelsButtons.Length;

            int spacingX = screenWidth / (columns + 1); // Horizontal spacing
            int spacingY = screenHeight / (rows + 3);   // Vertical spacing (offset a bit for aesthetics)

            int buttonWidth = menuBtnWidth / 2;
            int buttonHeight = menuBtnHeight;

            for (int i = 0; i < totalButtons; i++)
            {
                int row = i / columns;     // 0 or 1
                int col = i % columns;     // 0 to 4

                levelsButtons[i].Size = new Size(buttonWidth, buttonHeight);

                int x = spacingX * (col + 1) - (buttonWidth / 2);
                int y = spacingY * (row + 2); // Adjust for vertical center

                levelsButtons[i].Location = new Point(x, y - screenHeight / 7);
                levelsButtons[i].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            }

        }

        public void load_map()
        {
            //tabControl1.Hide();
            this.prepare_gamecomponents();
            map.drawMap(map.MapGrid);
            map.AddToForm(level);
        }

        public void prepare_gamecomponents()
        {
            player = map.player;
            boxes = map.boxes;
            walls = map.walls;
            finalDest = map.finalDest;
        }

        #endregion

        private Label label3;
        private PictureBox pictureBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private TabControl tabControl1;
        private TabPage menu;
        private TabPage levels;
        private TabPage level;
        private TabPage stats;
        private Button button14;
        private Button button13;
        private Button button12;
        private Button button11;
        private Button button10;
        private Button button9;
        private Button button8;
        private Button button7;
        private Button button6;
        private Button button5;
        private Button button4;
    }
}
