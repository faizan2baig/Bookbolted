
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text.pdf.draw;
using System.Data.SqlClient;
using iTextSharp.text.pdf;
using iTextSharp.text;
using bapp;
using System.Data.Common;
using System.Xml.Linq;
using System.Globalization;
using System.Linq;



namespace bapp
{
    public partial class Sales : Form
    {
        public Sales()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to print this report?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "PDF Files|*.pdf";
                    saveFileDialog.Title = "Save Pdf File";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string savePath = saveFileDialog.FileName;
                        Document document = new Document();
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(savePath, FileMode.Create));
                        document.Open();
                        Paragraph title = new Paragraph("Book Sales History Report", GetTitleFont());
                        title.Alignment = Element.ALIGN_CENTER;
                        document.Add(title);
                        Paragraph subtitle = new Paragraph($"Generated on {DateTime.Now.ToString("dddd, MMMM dd, yyyy hh:mm tt")}", GetSubtitleFont());
                        subtitle.Alignment = Element.ALIGN_CENTER;
                        document.Add(subtitle);
                        document.Add(new Paragraph("\n"));

                        PdfPTable timeAndDayTable = new PdfPTable(2);
                        timeAndDayTable.WidthPercentage = 100;

                        document.Add(new Paragraph("\n"));

                        DataTable booksAuditData = FetchBooksAuditTableDataFromDatabase();
                        if (booksAuditData != null && booksAuditData.Rows.Count > 0)
                        {
                            PdfPTable table = new PdfPTable(booksAuditData.Columns.Count);
                            table.WidthPercentage = 100;

                            // Add headers with formatting
                            foreach (DataColumn column in booksAuditData.Columns)
                            {
                                PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, GetHeaderFont()));
                                headerCell.BackgroundColor = BaseColor.DARK_GRAY;
                                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(headerCell);
                            }

                            // Add data rows with formatting
                            foreach (DataRow row in booksAuditData.Rows)
                            {
                                foreach (DataColumn column in booksAuditData.Columns)
                                {
                                    PdfPCell dataCell = new PdfPCell(new Phrase(row[column].ToString(), GetDataFont()));
                                    dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    table.AddCell(dataCell);
                                }
                            }

                            // Calculate and add total for the book_price column
                            decimal totalBookPrice = booksAuditData.AsEnumerable().Sum(row => Convert.ToDecimal(row["book_price"]));
                            PdfPCell totalCell = new PdfPCell(new Phrase($"Total: {totalBookPrice:C}", GetTotalFont()));
                            totalCell.Colspan = booksAuditData.Columns.Count;
                            totalCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            table.AddCell(totalCell);

                            document.Add(table);
                            document.Close();
                            MessageBox.Show("Pdf Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No Data Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Form1.LogExceptionToDatabase(ex);
            }




        }

        private DataTable FetchBooksAuditTableDataFromDatabase()
        {
            var con = Configuration.getInstance().getConnection();
            DataTable booksAuditData = new DataTable();

            string query = "SELECT [id], [book_id], [book_name], [book_price], [customer_email] FROM [bookbolted].[dbo].[booksAudit]";
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(booksAuditData);
                return booksAuditData;
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error fetching data from the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Function to get font for the report title
        private Font GetTitleFont()
        {
            return FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 16, BaseColor.BLACK);
        }

        // Function to get font for the report subtitle
        private Font GetSubtitleFont()
        {
            return FontFactory.GetFont(BaseFont.HELVETICA, 12, BaseColor.DARK_GRAY);
        }

        // Function to get font for table headers
        private Font GetHeaderFont()
        {
            return FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 10, BaseColor.WHITE);
        }

        // Function to get font for table data
        private Font GetDataFont()
        {
            return FontFactory.GetFont(BaseFont.HELVETICA, 10);
        }

        // Function to get font for total
        private Font GetTotalFont()
        {
            return FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 10);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new AdminTab();
            f.Show();
        }

        private void Sales_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            string connectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Modify the SQL query to match your table structure
                string query = "SELECT TOP (1000) [id], [book_id], [book_name], [book_price], [customer_email] FROM [bookbolted].[dbo].[booksAudit]";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Create a DataTable to hold the data
                        DataTable dataTable = new DataTable();

                        // Fill the DataTable with the data from the query
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }

    }
}

