using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bapp
{
    public partial class CustomerTab : Form
    {
        public CustomerTab()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Form6();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
           // Form f = new Cart();
           // f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Purchases();
            f.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new GiveFeedback();
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
