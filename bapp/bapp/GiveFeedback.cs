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
    public partial class GiveFeedback : Form
    {
        public GiveFeedback()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            try
            {
                // Get the feedback message from richTextBox1
                string feedbackMessage = richTextBox1.Text;

                // Check if customerEmail is not empty before proceeding
                if (!string.IsNullOrEmpty(Form1.customer_email) && !string.IsNullOrEmpty(feedbackMessage))
                {
                    // Connection string for your SQL Server database
                    string connectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Check if the customer exists before proceeding
                        int customerID = GetCustomerId(connection, Form1.customer_email);

                        if (customerID > 0)
                        {
                            // SQL command to insert a new feedback record
                            string sql = "INSERT INTO [bookbolted].[dbo].[Feedback] ([CustomerID], [Message], [Email], [Response]) " +
                                         "VALUES (@CustomerID, @Message, @Email, NULL)";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@CustomerID", customerID);
                                command.Parameters.AddWithValue("@Message", feedbackMessage);
                                command.Parameters.AddWithValue("@Email", Form1.customer_email);

                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Feedback added successfully!");
                                    this.Hide();
                                    Form f = new CustomerTab();
                                    f.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to add feedback.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Customer not found. Make sure the customer exists before providing feedback.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Customer email or feedback message is empty.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); Form1.LogExceptionToDatabase(ex);
            }

            // Function to retrieve CustomerID based on email
            

        }
        private int GetCustomerId(SqlConnection connection, string email)
        {
            string query = "SELECT UserID FROM [bookbolted].[dbo].[User] WHERE Email = @Email";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                object result = command.ExecuteScalar();

                return (result != null) ? Convert.ToInt32(result) : 0;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new CustomerTab();
            f.Show();
        }
    }
}
