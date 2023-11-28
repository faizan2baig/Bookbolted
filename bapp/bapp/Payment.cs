
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


namespace bapp
{
    public partial class Payment : Form
    {
        private const string     connectionString = "Data Source=FAIZAN;Initial Catalog=bookbolted;Integrated Security=True";

        private SqlConnection sqlConnection;
        public Payment()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(connectionString);
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            string customerEmail = Form1.customer_email; // Change this to the desired email

            // Call the method to calculate and display the total price
            CalculateTotalPrice(customerEmail);
        }
        private void CalculateTotalPrice(string customerEmail)
        {
            try
            {
                // Open the connection
                sqlConnection.Open();

                // Create a SqlCommand to calculate the total price
                string query = "SELECT SUM([book_price]) FROM [bookbolted].[dbo].[books] WHERE [customer_email] = @customerEmail";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@customerEmail", customerEmail);

                // Execute the query and get the result
                object result = command.ExecuteScalar();
                //MessageBox.Show($" {Convert.ToDecimal(result):C}");
                // Check if the result is not null
                if (result != null && result != DBNull.Value)
                {
                    // Display the total in label1
                    textBox1.Text = $" {Convert.ToDecimal(result):C}";
                }
                else
                {
                    // Display a message if there is no data
                    label1.Text = "No data found for the customer.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total price: " + ex.Message);
            }
            finally
            {
                // Close the connection
                sqlConnection.Close();
            }
        }

        


        private void button2_Click(object sender, EventArgs e)
        {

            GenerateReceipt();
            

        }



        private void GenerateReceipt()
        {
            try
            {
                // Create a SaveFileDialog to choose the PDF file location
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Save PDF Receipt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog.FileName;

                    // Create a Document and PdfWriter
                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(savePath, FileMode.Create));

                    // Open the document for writing
                    document.Open();

                    // Add content to the PDF
                    AddContentToPdf(document);

                    // Close the document
                    document.Close();

                    MessageBox.Show("PDF Receipt Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF Receipt: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Form1.LogExceptionToDatabase(ex);
            }
        }

        private void AddContentToPdf(Document document)
        {
            // Add title
            Paragraph title = new Paragraph("Payment Receipt", GetTitleFont());
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            // Add customer information
            document.Add(new Paragraph($"Customer: {bapp.Form1.customer_email}", GetDataFont()));
            document.Add(new Paragraph($"Date: {DateTime.Now.ToString("MMMM dd, yyyy")}", GetDataFont()));
            document.Add(new Paragraph("\n"));

            // Add payment details
            document.Add(new Paragraph($"Total Amount: {textBox1.Text}", GetDataFont()));
            document.Add(new Paragraph($"Discount Code: {textBox2.Text}", GetDataFont()));
            document.Add(new Paragraph($"Discounted Amount: {textBox3.Text}", GetDataFont()));
            document.Add(new Paragraph("\n"));

            // Add a line for separation
            document.Add(new Chunk(new LineSeparator(0.0f, 100.0f, BaseColor.BLACK, Element.ALIGN_CENTER, -1)));
            document.Add(new Paragraph("\n"));

            // Add additional labels and their corresponding content
            

            // Add a thank you message
            document.Add(new Paragraph("\nThank you for your purchase!", GetSubtitleFont()));
        }

        private void AddLabelAndContent(Document document, string labelText, Font font)
        {
            document.Add(new Paragraph($"{labelText}: {GetLabelText(labelText)}", font));
        }

        private string GetLabelText(string labelText)
        {
            // Return corresponding content based on label text
            switch (labelText)
            {
                case "Label1":
                    return "Content1"; // Replace with the actual content for Label1
                case "Label2":
                    return "Content2"; // Replace with the actual content for Label2
                case "Label":
                    return "Content"; // Replace with the actual content for Label
                default:
                    return string.Empty;
            }
        }

        // ... (other methods)

        // Adjusted font for a professional look
        private Font GetTitleFont()
        {
            return FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 24, BaseColor.BLACK);
        }

        private Font GetSubtitleFont()
        {
            return FontFactory.GetFont(BaseFont.HELVETICA, 14, BaseColor.DARK_GRAY);
        }

        private Font GetDataFont()
        {
            return FontFactory.GetFont(BaseFont.HELVETICA, 12);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal totalAmount = ParseCurrency(textBox1.Text);
                string discountCode = textBox2.Text;

                // Check and calculate discounted amount
                decimal discountedAmount = CalculateDiscountedAmount(totalAmount, discountCode);

                // Display the result in textBox3
                textBox3.Text = FormatCurrency(discountedAmount); // Forma
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        private string FormatCurrency(decimal amount)
        {
            // Format the decimal as currency (e.g., "Rs144.75")
            return string.Format(CultureInfo.InvariantCulture, "Rs{0:N2}", amount);
        }
        private decimal ParseCurrency(string currencyText)
        {
            // Parse the currency value (e.g., "Rs144.75")
            if (decimal.TryParse(currencyText.Replace("Rs", ""), NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            else
            {
                // Handle invalid currency format
                MessageBox.Show("Invalid currency format!");
                return 0;
            }
        }
        private decimal CalculateDiscountedAmount(decimal totalAmount, string discountCode)
        {
            try
            {
                // Open the connection if not already open
                if (sqlConnection.State != System.Data.ConnectionState.Open)
                    sqlConnection.Open();

                // Check if the discount code is valid and get the percentage from the database
                string query = "SELECT percentage FROM bookbolted.dbo.discounts WHERE name = @DiscountCode AND GETDATE() BETWEEN date_from AND date_to";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@DiscountCode", discountCode);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        decimal discountPercentage = Convert.ToDecimal(result);
                        decimal discountAmount = totalAmount * (discountPercentage / 100);
                        return totalAmount - discountAmount;
                    }
                    else
                    {
                        // Invalid discount code
                        //MessageBox.Show("Invalid discount code!");
                        return totalAmount;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, e.g., log it or show an error message
                MessageBox.Show("Error calculating discounted amount: " + ex.Message);
                return totalAmount;
            }
            finally
            {
                // Close the connection
                sqlConnection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Card();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Form6();
            f.Show();
        }
    }
}
