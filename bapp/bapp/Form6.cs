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
        public Form6()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private string connectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

        private void button3_Click(object sender, EventArgs e)
        {
            //

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Inventory";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // You can use the 'reader' object to retrieve data and display it in the view
                            // For simplicity, let's assume you have a model class called InventoryItem
                            // and a corresponding view to display the data

                            // Example:
                            // List<InventoryItem> inventoryItems = new List<InventoryItem>();
                            // while (reader.Read())
                            // {
                            //     InventoryItem item = new InventoryItem
                            //     {
                            //         Id = reader.GetInt32(0),
                            //         Name = reader.GetString(1),
                            //         // Add other properties as needed
                            //     };
                            //     inventoryItems.Add(item);
                            // }

                            // return View(inventoryItems);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (log, display an error page, etc.)
               // return View("Error");
            }

           // return View();


            //
        }
    }
}
