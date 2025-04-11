namespace czu_sokoban
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
        }

        //PLAY
        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = levels;
        }

        //STATS
        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = stats;
        }

        //EXIT
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //BACK TO MENU
        private void button14_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = menu;
        }
        //LEVEL 2
        private void button5_Click(object sender, EventArgs e)
        {

        }
        //LEVEL 3
        private void button6_Click(object sender, EventArgs e)
        {

        }
        //LEVEL 4
        private void button7_Click(object sender, EventArgs e)
        {

        }
        //LEVEL 5
        private void button8_Click(object sender, EventArgs e)
        { 

        }

        //LEVEL 6
        private void button9_Click(object sender, EventArgs e)
        {

        }
        //LEVEL 7
        private void button10_Click(object sender, EventArgs e)
        {

        }
        //LEVEL 8
        private void button11_Click(object sender, EventArgs e)
        {

        }
        //LEVEL 9
        private void button12_Click(object sender, EventArgs e)
        {

        }
        //LEVEL 10
        private void button13_Click(object sender, EventArgs e)
        {

        }

    }
}
