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
    public partial class ManageCustomer : Form
    {
        public ManageCustomer()
        {
            InitializeComponent();
        }
        private const string ConnectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

        private void ManageCustomer_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }
        private void LoadUserData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT TOP (1000) [UserID], [Username], [Password], [Email], [Role] FROM [bookbolted].[dbo].[User]";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;

                    // Add Delete button column
                    if (dataGridView1.Columns["DeleteButton"] == null)
                    {
                        DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
                        deleteButtonColumn.Name = "DeleteButton";
                        deleteButtonColumn.Text = "Delete";
                        deleteButtonColumn.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Add(deleteButtonColumn);
                    }
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message);
                Form1.LogExceptionToDatabase(ex);
            }    
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["DeleteButton"].Index || e.RowIndex >= 0)
            {
                int userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["UserID"].Value);
                DeleteUser(userId);
                LoadUserData(); // Refresh the data after deletion
            }
        }
        private void AddToUserAudit(int userId)
        {
            try
            {
                using (SqlConnection connectionForAudit = new SqlConnection(ConnectionString))
                {
                    connectionForAudit.Open();

                    // Move the deleted user to UserAudit table
                    string auditQuery = $"INSERT INTO [bookbolted].[dbo].[UserAudit] ([UserID], [Action], [DateTime]) " +
                                        $"SELECT [UserID], 'DELETE', GETDATE() FROM [bookbolted].[dbo].[User] WHERE [UserID] = @UserId";

                    using (SqlCommand auditCommand = new SqlCommand(auditQuery, connectionForAudit))
                    {
                        auditCommand.Parameters.AddWithValue("@UserId", userId);

                        auditCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                LogError(ex.Message, "YourFileName", "AddToUserAudit");
            }
        }

        private void DeleteFromUser(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Delete the user from User table
                    string deleteQuery = $"DELETE FROM [bookbolted].[dbo].[User] WHERE [UserID] = @UserId";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@UserId", userId);

                        deleteCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                LogError(ex.Message, "YourFileName", "DeleteFromUser");
            }
        }

        private void LogError(string errorDescription, string fileName, string functionName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Log the error into the Log table
                    string logQuery = $"INSERT INTO [bookbolted].[dbo].[Log] " +
                                      "([ErrorDescription], [FileName], [FunctionName], [DateTime]) " +
                                      "VALUES (@ErrorDescription, @FileName, @FunctionName, GETDATE())";

                    using (SqlCommand logCommand = new SqlCommand(logQuery, connection))
                    {
                        logCommand.Parameters.AddWithValue("@ErrorDescription", errorDescription);
                        logCommand.Parameters.AddWithValue("@FileName", fileName);
                        logCommand.Parameters.AddWithValue("@FunctionName", functionName);

                        logCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, e.g., show a message or log it to another source
                MessageBox.Show($"Error logging: {ex.Message}");
            }
        }

        // Now you can use these functions as follows:

        private void DeleteUser(int userId)
        {
            try
            {
                AddToUserAudit(userId); // Add to UserAudit table first
                DeleteFromUser(userId); // Delete from User table
                MessageBox.Show("Deleted Successfully");
            }
            catch (Exception ex)
            {
                // Log the error
                LogError(ex.Message, "YourFileName", "DeleteUser");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new AdminTab();
            f.Show();
        }
    }
}
