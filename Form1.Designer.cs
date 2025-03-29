using System.Runtime.CompilerServices;
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
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.Control;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(400, 30);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(101, 21);
            label2.TabIndex = 0;
            label2.Text = "No debug str";
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(600, 600);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            ResumeLayout(false);
            PerformLayout();

        }

        // Correct Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            map.drawMap(map.MapGrid);
            map.AddToForm(this);
            player = map.player;
            boxes = map.boxes;
            walls = map.walls;
            finalDest = map.finalDest;
        }

        // Handle Key Events, Main for the game
        private void Form1_KeyDown(object sender, KeyEventArgs e)
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
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            string lab1Text = map.checkWin(boxes, finalDest) ? "Výhra" : "Bez výhry";
            //string lab2Text = map.collided_pb(player, boxes)?.ToString() ?? "";
            //string box1Pos = box1.Location.ToString();
            label1.Text = lab1Text;
            //label2.Text = lab2Text;

        }

        #endregion

        private Label label1;
        private Label label2;
    }
}
