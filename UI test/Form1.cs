using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Pipes;
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
        int playerSpeed = 50;
        //int horizontalSpeed = 5;
        //int verticalSpeed = 3;
        //int enemyOneSpeed = 5;
        //int enemyTwoSpeed = 3;
        int num = 0;
        Keys key;

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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (num % 2 == 0)
            {
                num += 1;
            }
            else
            {
                num += 1;
            }
            label1.Text = "Score: " + num;
        }
    }
}
