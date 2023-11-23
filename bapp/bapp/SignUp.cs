using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;

namespace bapp
{
    public partial class SignUp : Form
    {
        private
 Random random =
new
 Random();
        public SignUp()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private bool ContainsSpace(string text)
        {
            return text.Contains(" ");
        }

        private void button3_Click(object sender, EventArgs e)
        {/////////////////////////////////////////////////////////////////////////////////////////
            try
            {
                // Get data from text boxes and combo box
                string username = textBox1.Text;
                string password = textBox3.Text;
                string email = textBox2.Text;
                string role = comboBox1.SelectedItem?.ToString(); // Assuming the role is selected from a combo box

                // Validate if the username, password, and email are not empty
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Username, password, and email are required.");
                    return; // Exit the method without inserting data
                }

                // Check if any text box contains space
                if (ContainsSpace(username) || ContainsSpace(password) || ContainsSpace(email))
                {
                    MessageBox.Show("Username, password, and email cannot contain spaces.");
                    return; // Exit the method without inserting data
                }

                string ConnectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Check if the same email already exists
                    string checkEmailQuery = "SELECT COUNT(*) FROM [User] WHERE Email = @Email";

                    using (SqlCommand checkEmailCommand = new SqlCommand(checkEmailQuery, connection))
                    {
                        checkEmailCommand.Parameters.AddWithValue("@Email", email);

                        int emailCount = Convert.ToInt32(checkEmailCommand.ExecuteScalar());

                        if (emailCount > 0)
                        {
                            MessageBox.Show("Email already exists. Please use a different email address.");
                            return; // Exit the method without inserting duplicate data
                        }
                    }

                    // Check if the same username already exists (optional, you can skip this check if usernames can be duplicated)
                    string checkUsernameQuery = "SELECT COUNT(*) FROM [User] WHERE Username = @Username";

                    using (SqlCommand checkUsernameCommand = new SqlCommand(checkUsernameQuery, connection))
                    {
                        checkUsernameCommand.Parameters.AddWithValue("@Username", username);

                        int usernameCount = Convert.ToInt32(checkUsernameCommand.ExecuteScalar());

                        if (usernameCount > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.");
                            return; // Exit the method without inserting duplicate data
                        }
                    }

                    // Insert the new user into the User table
                    string insertQuery = "INSERT INTO [User] (Username, Password, Email, Role) " +
                                         "VALUES (@Username, @Password, @Email, @Role)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Role", role);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("User signed up successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }
        private string GenerateVerificationCode()
        {
            // Generate a random 4-digit number
            return random.Next(1000, 10000).ToString();
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);

            if (!regex.IsMatch(textBox2.Text))
            {
                e.Cancel = true; // Prevent the focus from leaving the textbox
                errorProvider1.SetError(textBox2, "Please enter a valid email address.");
            }
            else
            {
                errorProvider1.SetError(textBox2, ""); // Clear the error message
            }
        }

        private void SignUp_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string username = textBox1.Text;

            if (IsValidName(username))
            {
                // Valid name
               // MessageBox.Show("Valid name.");
            }
            else
            {
                // Invalid name
                MessageBox.Show("Invalid name. Names should not contain special characters or numbers.");
                textBox1.Focus();   
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            string password = textBox3.Text;

            if (HasOnlyFourNumbers(password))
            {
                // Valid password with only 4 numbers
               // MessageBox.Show("Valid password with only 4 numbers.");
            }
            else
            {
                // Invalid password
                MessageBox.Show("Invalid password. Password should have only 4 numbers.");
            }
        }
        private bool IsValidName(string name)
        {
            // Customize this method based on your name validation criteria
            // For example, allow only alphabets and spaces
            return !string.IsNullOrWhiteSpace(name) && name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }

       

        private bool HasOnlyFourNumbers(string password)
        {
            // Check if the password has only 4 numbers
            return password.Length == 4 && password.All(char.IsDigit);
        }
    }
}
