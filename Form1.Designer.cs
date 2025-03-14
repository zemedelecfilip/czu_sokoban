namespace czu_sokoban
{
    partial class Form1
    {
        
        private System.ComponentModel.IContainer components = null;
        public Player player;
        public Box box1;
        public Box box2;
        //Box [] arr = new Box[2];
        //Box[] arr = { box1, box2 };

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
            player = new Player(0, 0);
            box1 = new Box(100, 100);
            box2 = new Box(150, 100);
            ((System.ComponentModel.ISupportInitialize)box2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)box1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)player).BeginInit();
            SuspendLayout();
            //
            // box2
            //
            box2.BackColor = Color.Blue;
            box2.Location = new Point(100, 100);
            box2.Name = "box2";
            box2.Size = new Size(50, 50);
            //
            // box1
            //
            box1.BackColor = Color.Blue;
            box1.Location = new Point(100, 100);
            box1.Name = "box1";
            box1.Size = new Size(50, 50);
            box1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Control;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(470, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 32);
            label1.Text = "NASRAT";
            // 
            // player
            // 
            player.BackColor = Color.Red;
            player.Location = new Point(0, 0);
            player.Name = "player";
            player.Size = new Size(50, 50);
            player.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(470, 51);
            label2.Name = "label2";
            label2.Size = new Size(113, 32);
            label2.Text = "NASRAT2";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 600);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(player);
            Controls.Add(box1);
            Controls.Add(box2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            ResumeLayout(false);
            PerformLayout();
            ((System.ComponentModel.ISupportInitialize)box2).EndInit();
            ((System.ComponentModel.ISupportInitialize)player).EndInit();
            ((System.ComponentModel.ISupportInitialize)box1).EndInit();

        }

        // Correct Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            //Maps.drawMap(Form1);
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

            string gridPosStr = player.gridPos().ToString();
            string gridPosStr2 = box1.gridPos().ToString();
            label1.Text = gridPosStr;
            label2.Text = gridPosStr2;
        }

        #endregion

        private Label label1;
        private Label label2;
    }
}
