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
    public partial class Form6 : Form
    {
        private SqlConnection sqlConnection;
        private const string connectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

        public Form6()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(connectionString);

            // Load data into the DataGridView
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                // Open the connection
                sqlConnection.Open();

                // Create a SqlDataAdapter and fill a DataTable with the data
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [bookbolted].[dbo].[books]", sqlConnection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Bind the DataTable to the DataGridView
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                // Close the connection
                sqlConnection.Close();
            }
        }
        string c_email = Form1.customer_email;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];

            // Get the book ID, name, price, and quantity
            int bookId = Convert.ToInt32(selectedRow.Cells[1].Value);
            string bookName = Convert.ToString(selectedRow.Cells[2].Value);
            decimal bookPrice = Convert.ToDecimal(selectedRow.Cells[6].Value);
            int quantity = Convert.ToInt32(selectedRow.Cells[7].Value);

            // Get the customer email
            string c_email = Form1.customer_email;

            // Check if quantity is greater than 0
            if (quantity > 0)
            {
                try
                {
                    // Open the connection
                    sqlConnection.Open();

                    // Create a SqlCommand to update the quantity in the Inventory table
                    string updateInventoryQuery = "UPDATE [bookbolted].[dbo].[Inventory] SET [QuantityInStock] = [QuantityInStock] - 1 WHERE [BookID] = @bookId";
                    SqlCommand updateInventoryCommand = new SqlCommand(updateInventoryQuery, sqlConnection);
                    updateInventoryCommand.Parameters.AddWithValue("@bookId", bookId);

                    // Execute the update in the Inventory table
                    updateInventoryCommand.ExecuteNonQuery();

                    // Create a SqlCommand to insert the book into the books table
                    string insertBooksQuery = "INSERT INTO [bookbolted].[dbo].[books] ([book_id], [book_name], [book_price], [customer_email]) VALUES (@bookId, @bookName, @bookPrice, @c_email)";
                    SqlCommand insertBooksCommand = new SqlCommand(insertBooksQuery, sqlConnection);
                    insertBooksCommand.Parameters.AddWithValue("@bookId", bookId);
                    insertBooksCommand.Parameters.AddWithValue("@bookName", bookName);
                    insertBooksCommand.Parameters.AddWithValue("@bookPrice", bookPrice);
                    insertBooksCommand.Parameters.AddWithValue("@c_email", c_email);

                    // Execute the insert into the books table
                    insertBooksCommand.ExecuteNonQuery();

                    // Display a success message
                    MessageBox.Show($"Book '{bookName}' added to the table for customer email: {c_email}. Remaining quantity: {quantity - 1}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating quantity or inserting into books table: " + ex.Message);
                }
                finally
                {
                    // Close the connection
                    sqlConnection.Close();

                    // Refresh the DataGridView
                    LoadData();
                }
            }
            else
            {
                // Display an error message
                MessageBox.Show("Error: Quantity is less than 1. Cannot add to the table.");
            }

            showbooks();

            //
        }
        private void showbooks()
        {
            String ConnectionString = @"Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";



            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Inventory", connection))
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Assuming dataGridView1 is the name of your DataGridView control
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); Form1.LogExceptionToDatabase(ex);
            }

        }
        private void Form6_Load(object sender, EventArgs e)
        {
            //
            showbooks();
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Payment();
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new CustomerTab();
            f.Show();
        }
    }
}
