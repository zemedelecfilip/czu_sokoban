using System.Runtime.CompilerServices;

namespace czu_sokoban
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        Player player = new Player(0, 0);
        Box box1 = new Box(100, 100);
        Box box2 = new Box(200, 200);
        Maps map = new Maps(); 
        map.addToForm(this);

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
            label1.BackColor = SystemColors.Control;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(329, 0);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(53, 21);
            label1.TabIndex = 1;
            label1.Text = "nasrat";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(329, 31);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(53, 21);
            label2.TabIndex = 0;
            label2.Text = "nasrat";

            // 
            // player

            // box1

            // box2
            // 

            // Form1
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
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
            this.Controls.Add(player);
            this.Controls.Add(box1);
            this.Controls.Add(box2);
            map.drawMap(map.MapGrid);
        }

        // Handle Key Events
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                if ((player.gridPos().X) == box1.gridPos().X + 1 && (player.gridPos().Y == box1.gridPos().Y))
                {
                    box1.MoveLeft();
                }
                player.MoveLeft();
            }
            else if (e.KeyCode == Keys.Right)
            {
                if ((player.gridPos().X) == box1.gridPos().X - 1 && (player.gridPos().Y == box1.gridPos().Y))
                {
                    box1.MoveRight();
                }
                player.MoveRight();
            }
            else if (e.KeyCode == Keys.Up)
            {
                if ((player.gridPos().Y) == box1.gridPos().Y + 1 && (player.gridPos().X == box1.gridPos().X))
                {
                    box1.MoveUp();
                }
                player.MoveUp();
            }
            else if (e.KeyCode == Keys.Down)
            {
                if ((player.gridPos().Y) == box1.gridPos().Y - 1 && (player.gridPos().X == box1.gridPos().X))
                {
                    box1.MoveDown();
                }
                player.MoveDown();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            string playerPos = player.Location.ToString();
            string box1Pos = box1.Location.ToString();
            label1.Text = playerPos;
            label2.Text = box1Pos;
        }

        #endregion

        private Label label1;
        private Label label2;
    }
}
