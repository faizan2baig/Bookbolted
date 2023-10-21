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

        private void button3_Click(object sender, EventArgs e)
        {/////////////////////////////////////////////////////////////////////////////////////////
          
           
                // Generate a 4-digit verification code
                string verificationCode = GenerateVerificationCode();

                // Get the email address from textBox2
                string recipientEmail = textBox2.Text.Trim();

                // Your email configuration
                string senderEmail = "faizan2baig@gmail.com"; // Replace with your email address
                string senderPassword = "nnkk qqdo xqmi shra"; // Replace with your email password
                string smtpServer = "smtp.gmail.com"; // Replace with your SMTP server

                try
                {
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer))
                    {
                        smtpClient.Port = 587; // Port for Gmail SMTP
                        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtpClient.EnableSsl = true;

                        using (MailMessage mail = new MailMessage(senderEmail, recipientEmail))
                        {
                            mail.Subject = "Verification Code";
                            mail.Body = $"Your verification code is: {verificationCode}";

                            smtpClient.Send(mail);
                        }

                        MessageBox.Show("Verification code sent successfully!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            

            
        
    

            ////////////////////////////////////////////////////////////////////////////////////////
            //
            this.Hide();
            Form f = new Form2();

            f.Show();
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
    }
}
