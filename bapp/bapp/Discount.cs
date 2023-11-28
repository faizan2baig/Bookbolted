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
    public partial class Discount : Form
    {
        public Discount()
        {
            InitializeComponent();
        }
                        //String sqlConnection = @"Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";
        private SqlConnection sqlConnection = new SqlConnection("Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True");

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string name = textBox1.Text;
                decimal percentage = Convert.ToDecimal(textBox2.Text);
                DateTime dateFrom = dateTimePicker1.Value;
                DateTime dateTo = dateTimePicker2.Value;

                // Insert data into the discounts table
                InsertData(name, percentage, dateFrom, dateTo);

                // Optionally, update UI or provide feedback to the user
                MessageBox.Show("Data added successfully!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }   
        }
        private void InsertData(string name, decimal percentage, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                // Open the connection if not already open
                if (sqlConnection.State != System.Data.ConnectionState.Open)
                    sqlConnection.Open();

                // Insert data into the discounts table
                string insertQuery = "INSERT INTO bookbolted.dbo.discounts (name, percentage, date_from, date_to) " +
                                     "VALUES (@Name, @Percentage, @DateFrom, @DateTo)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Percentage", percentage);
                    cmd.Parameters.AddWithValue("@DateFrom", dateFrom);
                    cmd.Parameters.AddWithValue("@DateTo", dateTo);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, e.g., log it or show an error message
                MessageBox.Show("Error inserting data: " + ex.Message);
            }
            finally
            {
                // Close the connection
                sqlConnection.Close();
            }
        }

        private void Discount_Load(object sender, EventArgs e)
        {

        }
    }
}
