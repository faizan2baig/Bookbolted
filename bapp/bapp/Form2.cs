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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox1.KeyPress += textBox1_KeyPress;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //textbox validation

            if (textBox1.Text.Length == 1)
            {
                textBox2.Focus();
            }








            // You can add similar event handlers for textBox4 if needed.

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 1)
            {
                textBox3.Focus();
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == 1)
            {
                textBox4.Focus();
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length == 1)
            {
                button3.Focus();
            }
        }
        private string em = Form3.email;
        private void Form2_Load(object sender, EventArgs e)
        {
            MessageBox.Show(em);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                // Suppress the Backspace key
                e.Handled = true;
            }

        }
        

        

        
        private int generatedVerificationCode= Form3.vcode;
        private void button3_Click(object sender, EventArgs e)
        {
            //
           
                String connectionString = @"Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

                try
                {
                    // Get the entered verification code from textBox1, textBox2, textBox3, and textBox4
                    string enteredCode = textBox1.Text + textBox2.Text + textBox3.Text + textBox4.Text;

                    // Convert the entered code to an integer
                    if (int.TryParse(enteredCode, out int enteredVerificationCode))
                    {
                        // Check if the entered code matches the generated code
                        if (enteredVerificationCode == generatedVerificationCode)
                        {
                            // Verification successful, fetch user information from the database
                            string userEmail = em; // Assuming 'em' is the user's email
                            string query = "SELECT [Username], [Email], [Password] FROM [bookbolted].[dbo].[User] WHERE [Email] = @UserEmail";

                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();

                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@UserEmail", userEmail);

                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            string username = reader["Username"].ToString();
                                            string email = reader["Email"].ToString();
                                            string password = reader["Password"].ToString();

                                            // Send the password to the user's email
                                            SendPasswordEmail(email, username, password);

                                            // Display a message indicating that the password has been sent to the email
                                            MessageBox.Show($"Verification successful. The password has been sent to {email}.");

                                            this.Hide(); // Close Form2 or navigate to another form
                                        Form f = new Form1();
                                        f.Show();


                                        }
                                        else
                                        {
                                            MessageBox.Show("User not found in the database.");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid verification code. Please try again.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid verification code format. Please enter a 4-digit code.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error verifying code: " + ex.Message);
                }
            

            
            //
        }
        private void SendPasswordEmail(string toEmail, string username, string password)
        {
            // Replace the placeholders with your actual email server details
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "faizan2baig@gmail.com";
            string smtpPassword = "hate flrr lurm vpxu";

            using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("faizan2baig@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = "Your Password Recovery";
                mail.Body = $"Hello {username},\n\nYour password is: {password}";

                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending email: " + ex.Message);
                }
            }
        }

    }
}
