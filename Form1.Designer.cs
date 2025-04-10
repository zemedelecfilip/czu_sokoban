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
        
        public int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        public int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        //menu, levels, level, my profile
        public string scene = "level";
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
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.ButtonHighlight;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.TabIndex = 1;
            label1.Text = "zatím nic";
            label1.Location = new Point(0, 0);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.Control;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.TabIndex = 0;
            label2.Text = "No debug str";
            label2.Location = new Point(0, label1.Height * 2);
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Green;
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(135, 86);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "PLAY";
            button1.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(108, 140);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 4;
            button2.Text = "STATS";
            button2.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(108, 187);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 5;
            button3.Text = "EXIT";
            button3.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Size = new Size(200, 100);
            label3.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            label3.Location = new Point(screenWidth / 2 - label3.Width / 2, 0);
            label3.Name = "label3";
            label3.TabIndex = 0;
            label3.Text = "SOKOBAN";
            label3.BackColor = pictureBox1.BackColor;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(screenWidth, screenHeight);
            Controls.Add(label3);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        //PLAY
        private void button1_Click(object sender, EventArgs e)
        {
            this.hide_menu();
            Storage.scene = "level";
            map.drawMap(map.MapGrid);
            map.AddToForm(this);
            player = map.player;
            boxes = map.boxes;
            walls = map.walls;
            finalDest = map.finalDest;
        }

        //STATS
        private void button2_Click(object sender, EventArgs e)
        {

        }

        
        //EXIT
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Correct Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            //this.scene = "menu";
            //this.calculate_menu();
            //this.menu_draw();
            this.load_map();
        }

        // Handle Key Events, Main for the game
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            label1.Text = e.KeyCode.ToString(); //debug
            if (Storage.scene == "level")
            {
                this.game_movement(sender, e);
            }

            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }


            //map.checkWin(boxes, finalDest);

            string lab2Text = Storage.scene;
            string lab1Text = map.checkWin(boxes, finalDest) ? "Výhra" : "Bez výhry";
            label1.Text = lab1Text;
            label2.Text = lab2Text;

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

            string lab2Text = "Madar";
            string lab1Text = map.checkWin(boxes, finalDest) ? "Výhra" : "Bez výhry";
            label1.Text = lab1Text;
            label2.Text = lab2Text;

        }

        public void menu_draw()
        {
            if (this.scene == "menu") 
            {
                label1.BringToFront();
                label2.BringToFront();
                pictureBox1.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
            }
            else
            {
                pictureBox1.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }
        }

        public void calculate_menu()
        {
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Size = new Size(screenWidth, screenHeight / 10);

            button1.Size = new Size(screenWidth / 4, screenHeight / 7);
            button1.Location = new Point(screenWidth / 2 - button1.Width / 2, (screenHeight - pictureBox1.Height) / 4 - button1.Height / 2);
           
            button2.Size = new Size(screenWidth / 4, screenHeight / 7);
            button2.Location = new Point(screenWidth / 2 - button2.Width / 2, (screenHeight * 2 - pictureBox1.Height) / 4 - button2.Height / 2);

            button3.Size = new Size(screenWidth / 4, screenHeight / 7);
            button3.Location = new Point(screenWidth / 2 - button3.Width / 2, (screenHeight * 3 - pictureBox1.Height) / 4 - button3.Height / 2);


            label3.Size = new Size(screenWidth / 5, screenHeight / 6);
            label3.Location = new Point(screenWidth / 2 - label3.Width / 2, pictureBox1.Height / 2 - label3.Height / 2);

            ClientSize = new Size(screenWidth, screenHeight);

        }

        public void hide_menu()
        {
            pictureBox1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
        }

        public void load_map()
        {
            this.hide_menu();
            Storage.scene = "level";
            map.drawMap(map.MapGrid);
            map.AddToForm(this);
            this.prepare_gamecomponents();
        }

        public void prepare_gamecomponents()
        {
            player = map.player;
            boxes = map.boxes;
            walls = map.walls;
            finalDest = map.finalDest;
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private PictureBox pictureBox1;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}
