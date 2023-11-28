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
    public partial class AdminTab : Form
    {
        Discount discount;
        ManageCustomer customer;
        Form5 crud;
      //  Sales_History shistory;
        public AdminTab()
        {
            InitializeComponent();
            
        }
        private void btn1(object x)
        {
           
        }
        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void AdminTab_Load(object sender, EventArgs e)
        {

        }
       

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {this.Hide();
            Form f = new Form5();
            f.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Sales();
            f.Show();

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new ManageCustomer();
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Discount();
            f.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Feedbacks();
            f.Show();
        }
    }

}
