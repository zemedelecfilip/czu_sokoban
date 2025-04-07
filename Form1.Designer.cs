using System.Runtime.CompilerServices;
using System.Windows.Forms;
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
        public string scene = "menu";
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
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.ButtonHighlight;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(400, 0);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(72, 21);
            label1.TabIndex = 1;
            label1.Text = "zatím nic";
            label1.BringToFront();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.Control;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(2, 462);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(101, 21);
            label2.TabIndex = 0;
            label2.Text = "No debug str";
            label2.BringToFront();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "";
            pictureBox1.Size = new Size(screenWidth, screenHeight / 10);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(screenWidth, screenHeight);
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

        // Correct Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            map.drawMap(map.MapGrid);
            map.AddToForm(this);
            player = map.player;
            boxes = map.boxes;
            walls = map.walls;
            finalDest = map.finalDest;
            */
        }

        // Handle Key Events, Main for the game
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.scene == "level")
            {
                this.game_movement(sender, e);
            } 

            if (e.KeyCode == Keys.Escape)
            {

                Application.Exit();
            }

            //map.checkWin(boxes, finalDest);

            string lab2Text = "Madar";
            //string lab1Text = map.checkWin(boxes, finalDest) ? "Výhra" : "Bez výhry";
            //label1.Text = lab1Text;
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

        #endregion

        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
    }
}
