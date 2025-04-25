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
        
        //BACK TO LEVELS
        private void button15_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = levels;
            this.reset_map();
        }
    }
}
