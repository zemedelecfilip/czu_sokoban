namespace UI_test
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        const int windowWidth = 600;
        const int windowHeight = 600;
        Color gui_color = Color.Gray;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox2 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox3 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // pictureBox2 - player character
            // 
            pictureBox2.BackColor = Color.Red;
            pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            pictureBox2.Cursor = Cursors.AppStarting;
            pictureBox2.Location = new Point(300, 300);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(50, 50);
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            pictureBox2.Tag = "MyWarrior";
            pictureBox2.Click += pictureBox2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Consolas", 8F, FontStyle.Bold);
            label1.Location = new Point(5, 23);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(81, 19);
            label1.TabIndex = 0;
            label1.Text = "Score: 0";
            label1.TextAlign = ContentAlignment.TopRight;
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Consolas", 8F, FontStyle.Bold);
            label2.Location = new Point(5, 54);
            label2.Name = "label2";
            label2.Size = new Size(36, 19);
            label2.TabIndex = 3;
            label2.Text = "KEY";
            // 
            // pictureBox1 - gui box on top
            // 
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.BackColor = gui_color;
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(windowWidth, 100);
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // pictureBox3 - gameboard background
            // 
            pictureBox3.BackColor = SystemColors.Info;
            pictureBox3.Location = new Point(50, 150);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(500, 400);
            pictureBox3.TabIndex = 5;
            pictureBox3.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(18F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(600, 600);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(pictureBox3);
            Font = new Font("Consolas", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            KeyPreview = true;
            Margin = new Padding(5, 4, 5, 4);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Example: Display the key pressed in label2
            label2.Text = $"Key: {e.KeyCode}";

            if (e.KeyCode == Keys.Left && pictureBox2.Location.X > 50) // > 50, < WIndwoWidth - 50
            {
                //goLeft = true;
                pictureBox2.BackColor = Color.Blue;
                pictureBox2.Left -= playerSpeed;
            }
            if (e.KeyCode == Keys.Right && pictureBox2.Location.X < windowWidth - 100)
            {
                //goRight = true;
                pictureBox2.BackColor = Color.Green;
                pictureBox2.Left += playerSpeed;
            }
            if (e.KeyCode == Keys.Up && pictureBox2.Location.Y > pictureBox1.Size.Height + 50)
            {
                //goUp = true;
                pictureBox2.BackColor = Color.Yellow;
                pictureBox2.Top -= playerSpeed;
            }
            if (e.KeyCode == Keys.Down && pictureBox2.Location.Y < windowHeight - pictureBox1.Size.Height)
            {
                pictureBox2.BackColor = Color.Red;
                pictureBox2.Top += playerSpeed;
                //goDown = true;
            }
            if (e.KeyCode == Keys.Escape)
            {
                Form1.ActiveForm.Close();
            }
        }

        #endregion

        private Label label1;
        private PictureBox pictureBox2;
        private Label label2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox3;
    }
}
