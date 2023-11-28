using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bapp
{
    public partial class Purchases : Form
    {
        public Purchases()
        {
            InitializeComponent();
        }
        public string customer_email = Form1.customer_email;
        private void Purchases_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            string ConnectionString = @"Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

            try
            {
                // Assuming x is the variable containing the email
                string email = Form1.customer_email; // Replace with your actual email variable

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT TOP (1000) [id], [book_id], [book_name], [book_price], [customer_email] FROM [bookbolted].[dbo].[booksAudit] WHERE [customer_email] = @Email";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Assuming dataGridView1 is the name of your DataGridView control
                        dataGridView1.DataSource = dataTable;

                        // Auto-size columns for better alignment
                        dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        // Set column headers text alignment
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Form1.LogExceptionToDatabase(ex);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new CustomerTab();
            f.Show();
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
