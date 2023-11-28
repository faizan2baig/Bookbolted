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
    public partial class Card : Form
    {
        public Card()
        {
            InitializeComponent();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private const string ConnectionString = @"Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string masterCardNumber = textBox1.Text.Trim();

            // Define a regular expression for MasterCard numbers
            string masterCardRegex = @"^5[1-5][0-9]{14}$";

            // Check if the input matches the MasterCard pattern
            if (Regex.IsMatch(masterCardNumber, masterCardRegex))
            {
                MessageBox.Show("Valid MasterCard number.", "Validation Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid MasterCard number. Please enter a valid MasterCard number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Set focus back to the TextBox for correction
                textBox1.Focus();
            }

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            ValidateCVC(textBox2.Text.Trim());

        }
        private void ValidateCVC(string cvc)
        {
            // Define a regular expression for a 3 or 4 digit CVC
            string cvcRegex = @"^[0-9]{3,4}$";

            // Check if the input matches the CVC pattern
            if (!Regex.IsMatch(cvc, cvcRegex))
            {
                MessageBox.Show("Invalid CVC. Please enter a valid CVC.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Set focus back to the TextBox for correction
                textBox2.Focus();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // Get data from controls
                string cardNumber = textBox1.Text;
                DateTime expiryDate = dateTimePicker1.Value;
                string cvv = textBox2.Text;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Exclude identity column (assuming CardID is an identity column)
                    string insertQuery = "INSERT INTO [bookbolted].[dbo].[MasterCard] (CardNumber, ExpiryDate, CVV, customer_email) " +
                                         "VALUES (@CardNumber, @ExpiryDate, @CVV, @CustomerEmail)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CardNumber", cardNumber);
                        command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                        command.Parameters.AddWithValue("@CVV", cvv);

                        // Assuming you have a customer_email variable, replace it with the actual value
                        string customerEmail = Form1.customer_email;
                        command.Parameters.AddWithValue("@CustomerEmail", customerEmail);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data inserted successfully.");
                audit();
                this.Hide();
                Form f = new CustomerTab();
                f.Show();

                // Refresh or perform any additional actions as needed
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }
        private void audit()
        {
            try
            {
                // Assuming x is the customer_email variable
                string x = Form1.customer_email;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Transfer rows to [booksAudit] table excluding the identity column
                    string transferQuery = "INSERT INTO [bookbolted].[dbo].[booksAudit] " +
                                           "(book_id, book_name, book_price, customer_email) " +
                                           "SELECT [book_id], [book_name], [book_price], [customer_email] " +
                                           "FROM [bookbolted].[dbo].[books] " +
                                           "WHERE [customer_email] = @CustomerEmail";

                    using (SqlCommand transferCommand = new SqlCommand(transferQuery, connection))
                    {
                        transferCommand.Parameters.AddWithValue("@CustomerEmail", x);

                        int rowsTransferred = transferCommand.ExecuteNonQuery();

                       // MessageBox.Show($"{rowsTransferred} row(s) transferred to booksAudit table.");

                        // Optional: You may want to delete the transferred rows from the [books] table.
                        // Uncomment the lines below if you want to do that.

                        string deleteQuery = "DELETE FROM [bookbolted].[dbo].[books] WHERE [customer_email] = @CustomerEmail";
                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@CustomerEmail", x);
                            deleteCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void InsertData(string cardNumber, DateTime expiryDate, string cvv)
        {

        }

        private void Card_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new CustomerTab();
            f.Show();
        }
    }
}
