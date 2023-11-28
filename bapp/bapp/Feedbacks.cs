using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bapp
{
    public partial class Feedbacks : Form
    {
        private int selectedFeedbackID;
        private int selectedCustomerID;
        private string selectedEmail;
        public Feedbacks()
        {
            InitializeComponent();
           // dataGridView1.CellClick += DataGridView1_CellClick;
        }
        

        private void Feedbacks_Load(object sender, EventArgs e)
        {
            load();
            

        }
        private void load()
        {
            try
            {
                // Connection string for your SQL Server database
                string connectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL command to select top 1000 rows from the Feedback table
                    string sql = "SELECT TOP (1000) [FeedbackID], [CustomerID], [Message], [Email], [Response] FROM [bookbolted].[dbo].[Feedback]";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Use SqlDataAdapter to fill a DataSet
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet, "Feedback");

                        // Bind the DataSet to the DataGridView
                        dataGridView1.DataSource = dataSet.Tables["Feedback"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected row's data
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                selectedFeedbackID = Convert.ToInt32(selectedRow.Cells["FeedbackID"].Value);
                selectedCustomerID = Convert.ToInt32(selectedRow.Cells["CustomerID"].Value);
                selectedEmail = selectedRow.Cells["Email"].Value.ToString();

                // Display a message box with selected information
                MessageBox.Show($"Selected row: FeedbackID={selectedFeedbackID}, CustomerID={selectedCustomerID}, Email={selectedEmail}");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (selectedFeedbackID != 0)
            {
                // Get the feedback message from richTextBox1
                string feedbackMessage = richTextBox1.Text;

                // Check if feedback message is not empty
                if (!string.IsNullOrEmpty(feedbackMessage))
                {
                    try
                    {
                        // Configure the SMTP client
                        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                        {
                            Port = 587,
                            Credentials = new NetworkCredential("faizan2baig@gmail.com", "hate flrr lurm vpxu"),
                            EnableSsl = true,
                        };

                        // Create the email message
                        MailMessage mailMessage = new MailMessage
                        {
                            From = new MailAddress("faizan2baig@gmail.com"),
                            Subject = "Feedback from Your Application",
                            Body = $"Feedback from CustomerID {selectedCustomerID}:\n\n{feedbackMessage}",
                        };

                        // Add the recipient email address
                        mailMessage.To.Add(selectedEmail);

                        // Send the email
                        smtpClient.Send(mailMessage);

                        // Update the Response column in the database
                        UpdateResponseInDatabase(selectedFeedbackID, feedbackMessage);

                        MessageBox.Show($"Feedback added successfully and sent to {selectedEmail}!");
                        load();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error sending email: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Feedback message is empty. Please enter a message before adding feedback.");
                }
            }
            else
            {
                MessageBox.Show("Please select a row before adding feedback.");
            }

            // Function to update the Response column in the database
            
        }
        private void UpdateResponseInDatabase(int feedbackID, string response)
        {
            try
            {
                // Connection string for your SQL Server database
                string connectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL command to update the Response column
                    string sql = "UPDATE [bookbolted].[dbo].[Feedback] SET [Response] = @Response WHERE [FeedbackID] = @FeedbackID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Response", response);
                        command.Parameters.AddWithValue("@FeedbackID", feedbackID);

                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the update was successful
                        if (rowsAffected > 0)
                        {
                            // The Response column has been updated successfully
                        }
                        else
                        {
                            MessageBox.Show("Failed to update the Response column in the database.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the Response column: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new AdminTab();
            f.Show();
        }
    }
}
