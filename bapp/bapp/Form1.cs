using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }
        public static void LogExceptionToDatabase(Exception ex)
        {
            string connectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";
            string insertQuery = "INSERT INTO [bookbolted].[dbo].[Log] ([ErrorDescription], [FileName], [FunctionName], [DateTime]) VALUES (@ErrorDescription, @FileName, @FunctionName, @DateTime)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ErrorDescription", ex.Message);
                    command.Parameters.AddWithValue("@FileName", "YourFileNameHere"); // Replace with actual file name
                    command.Parameters.AddWithValue("@FunctionName", "YourFunctionNameHere"); // Replace with actual function name
                    command.Parameters.AddWithValue("@DateTime", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }
        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new SignUp();

            f.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Form3();
            f.Show();
        }
        public static string customer_email;
        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                string email = textBox1.Text;
                string password = textBox2.Text;

                // Validate if the email and password are not empty
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Email and password are required.");
                    return; // Exit the method without attempting login
                }

                string ConnectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Check if the email and password match a user in the database
                    string loginQuery = "SELECT Role FROM [User] WHERE Email = @Email AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(loginQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);

                        object roleObj = command.ExecuteScalar();

                        if (roleObj != null)
                        {
                            string role = roleObj.ToString();

                            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {

                                customer_email = textBox1.Text;
                                this.Hide();
                                Form f = new AdminTab();
                                f.Show();
                            }
                            else if (role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show(customer_email);
                                customer_email = textBox1.Text;
                                this.Hide();
                                Form f = new CustomerTab();
                                f.Show();

                            }
                            else
                            {
                                MessageBox.Show("Unknown role: " + role);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid email or password.");
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogExceptionToDatabase(ex);
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);

            if (!regex.IsMatch(textBox1.Text))
            {
                e.Cancel = true; // Prevent the focus from leaving the textbox
                errorProvider1.SetError(textBox1, "Please enter a valid email address.");
            }
            else
            {
                errorProvider1.SetError(textBox1, ""); // Clear the error message
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
