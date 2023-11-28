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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private const string ConnectionString = @"Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //



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
                MessageBox.Show("Error: " + ex.Message);
            }

            //
        }
        private void LoadData()
        {
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
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Get data from text boxes
                string title = textBox2.Text;
                string author = textBox3.Text;
                string genre = textBox4.Text;
                string isbn = textBox5.Text;
                decimal price = Convert.ToDecimal(textBox6.Text);
                int quantityInStock = Convert.ToInt32(textBox7.Text);
                int threshold = Convert.ToInt32(textBox8.Text);

                // Check if any text box contains space
                if (ContainsSpace(title) || ContainsSpace(author) || ContainsSpace(genre) || ContainsSpace(isbn))
                {
                    MessageBox.Show("Text boxes cannot contain spaces.");
                    return; // Exit the method without inserting data
                }

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Check if the same data already exists
                    string checkQuery = "SELECT COUNT(*) FROM Inventory " +
                                        "WHERE Title = @Title AND Author = @Author AND Genre = @Genre AND ISBN = @ISBN";

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Title", title);
                        checkCommand.Parameters.AddWithValue("@Author", author);
                        checkCommand.Parameters.AddWithValue("@Genre", genre);
                        checkCommand.Parameters.AddWithValue("@ISBN", isbn);

                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Data already exists in the table.");
                            return; // Exit the method without inserting duplicate data
                        }
                    }

                    // Assuming BookID is an identity column and doesn't need to be specified during insert
                    string insertQuery = "INSERT INTO Inventory (Title, Author, Genre, ISBN, Price, QuantityInStock, Threshold) " +
                                         "VALUES (@Title, @Author, @Genre, @ISBN, @Price, @QuantityInStock, @Threshold)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Author", author);
                        command.Parameters.AddWithValue("@Genre", genre);
                        command.Parameters.AddWithValue("@ISBN", isbn);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@QuantityInStock", quantityInStock);
                        command.Parameters.AddWithValue("@Threshold", threshold);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data inserted successfully.");

                // Refresh the DataGridView
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Form1.LogExceptionToDatabase(ex);
            }

            // Helper function to check if a string contains space
           

        }
        private bool ContainsSpace(string input)
        {
            return input.Contains(" ");
        }
        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // Assuming the order of columns in the DataGridView matches the order in the SELECT query
                    int id = Convert.ToInt32(row.Cells["BookID"].Value);
                    string title = Convert.ToString(row.Cells["Title"].Value);
                    string author = Convert.ToString(row.Cells["Author"].Value);
                    string genre = Convert.ToString(row.Cells["Genre"].Value);
                    string isbn = Convert.ToString(row.Cells["ISBN"].Value);
                    decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                    int quantityInStock = Convert.ToInt32(row.Cells["QuantityInStock"].Value);
                    int threshold = Convert.ToInt32(row.Cells["Threshold"].Value);

                    // Fill the text boxes
                    textBox1.Text = id.ToString();
                    textBox2.Text = title;
                    textBox3.Text = author;
                    textBox4.Text = genre;
                    textBox5.Text = isbn;
                    textBox6.Text = price.ToString();
                    textBox7.Text = quantityInStock.ToString();
                    textBox8.Text = threshold.ToString();
                }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Get data from text boxes
                int bookID = Convert.ToInt32(textBox1.Text); // Assuming textBox1 contains the BookID for the record to be edited
                string title = textBox2.Text;
                string author = textBox3.Text;
                string genre = textBox4.Text;
                string isbn = textBox5.Text;
                decimal price = Convert.ToDecimal(textBox6.Text);
                int quantityInStock = Convert.ToInt32(textBox7.Text);
                int threshold = Convert.ToInt32(textBox8.Text);

                // Check if any text box contains space
                if (ContainsSpace(title) || ContainsSpace(author) || ContainsSpace(genre) || ContainsSpace(isbn))
                {
                    MessageBox.Show("Text boxes cannot contain spaces.");
                    return; // Exit the method without updating data
                }

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Check if the same data already exists excluding the current record
                    string checkQuery = "SELECT COUNT(*) FROM Inventory " +
                                        "WHERE Title = @Title AND Author = @Author AND Genre = @Genre AND ISBN = @ISBN AND BookID != @BookID";

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Title", title);
                        checkCommand.Parameters.AddWithValue("@Author", author);
                        checkCommand.Parameters.AddWithValue("@Genre", genre);
                        checkCommand.Parameters.AddWithValue("@ISBN", isbn);
                        checkCommand.Parameters.AddWithValue("@BookID", bookID);

                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Data already exists in the table.");
                            return; // Exit the method without updating duplicate data
                        }
                    }

                    string updateQuery = "UPDATE Inventory " +
                                         "SET Title = @Title, Author = @Author, Genre = @Genre, ISBN = @ISBN, " +
                                         "Price = @Price, QuantityInStock = @QuantityInStock, Threshold = @Threshold " +
                                         "WHERE BookID = @BookID";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", bookID);
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Author", author);
                        command.Parameters.AddWithValue("@Genre", genre);
                        command.Parameters.AddWithValue("@ISBN", isbn);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@QuantityInStock", quantityInStock);
                        command.Parameters.AddWithValue("@Threshold", threshold);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data updated successfully.");

                // Refresh the DataGridView
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); Form1.LogExceptionToDatabase(ex);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Get data from text box
                int bookIDToDelete = Convert.ToInt32(textBox1.Text);

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Get the data of the record to be deleted
                    string selectQuery = "SELECT TOP (1) [AuditID], [BookID], [Action], [DateTime] " +
                                         "FROM InventoryAudit " +
                                         "WHERE BookID = @BookID " +
                                         "ORDER BY DateTime DESC"; // Assuming you want the latest audit entry

                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@BookID", bookIDToDelete);

                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Read the data from the audit table
                                int auditID = Convert.ToInt32(reader["AuditID"]);
                                int bookID = Convert.ToInt32(reader["BookID"]);
                                string action = reader["Action"].ToString();
                                DateTime dateTime = Convert.ToDateTime(reader["DateTime"]);

                                // Insert the data into InventoryAudit
                                string auditQuery = "INSERT INTO InventoryAudit (AuditID, BookID, Action, DateTime) " +
                                                    "VALUES (@AuditID, @BookID, @Action, @DateTime)";

                                using (SqlCommand auditCommand = new SqlCommand(auditQuery, connection))
                                {
                                    auditCommand.Parameters.AddWithValue("@AuditID", auditID);
                                    auditCommand.Parameters.AddWithValue("@BookID", bookID);
                                    auditCommand.Parameters.AddWithValue("@Action", action);
                                    auditCommand.Parameters.AddWithValue("@DateTime", dateTime);

                                    auditCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    // Delete the record from the Inventory table
                    string deleteQuery = "DELETE FROM Inventory WHERE BookID = @BookID";

                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@BookID", bookIDToDelete);
                        deleteCommand.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data deleted successfully.");

                // Refresh the DataGridView
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); Form1.LogExceptionToDatabase(ex); Form1.LogExceptionToDatabase(ex);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ;
            this.Hide();
            Form f = new AdminTab();
            f.Show();
        }
    }
}
