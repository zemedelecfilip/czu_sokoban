using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_test
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, jumping, isGameOver;
        //int jumpSpeed;
        //int force;
        string score = "";
        int playerSpeed = 5;
        //int horizontalSpeed = 5;
        //int verticalSpeed = 3;
        //int enemyOneSpeed = 5;
        //int enemyTwoSpeed = 3;
        int num = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void key_listener(KeyEventArgs e)
        {
            string key = e.ToString();
            label2.Text = key;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int randomNumX = rand.Next(0, 450);
            int randomNumY = rand.Next(0, 450);
            if (num % 2 == 0)
            {
                pictureBox2.BackColor = Color.Blue;
                num += 1;
                label1.Text = "Score: " + num;
            }
            else
            {
                pictureBox2.BackColor = Color.Red;
                num += 1;
                label1.Text = "Score: " + num;


            }
            pictureBox2.Location = new Point(randomNumX, randomNumY);
        }
    }
}
