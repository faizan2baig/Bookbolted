using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace bapp
{
    public partial class Form3 : Form
    {
        public int VerificationCode { get; private set; }
        public Form3()
        {
            InitializeComponent();
        }
        public static int vcode ;
        public static int GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
        public static string email;
        private void button3_Click(object sender, EventArgs e)
        { email= textBox2.Text;
            //
          int   verificationCode = GenerateVerificationCode();
            vcode = verificationCode;
            try
            {
                // Generate a 4-digit random number



                // Get the recipient email address from the textBox2
                string recipientEmail = textBox2.Text;

                // Your Gmail credentials and settings
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587; // Use port 587 for TLS
                string smtpUsername = "faizan2baig@gmail.com";
                string smtpPassword = "hate flrr lurm vpxu";

                // Create a MailMessage
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUsername);
                mail.To.Add(recipientEmail);
                mail.Subject = "Verification Code";
                mail.Body = $"Your verification code is: {verificationCode}";

                // Setup the SMTP client
                SmtpClient smtpClient = new SmtpClient(smtpServer);
                smtpClient.Port = smtpPort;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                // Send the email
                smtpClient.Send(mail);

                MessageBox.Show("Verification code sent successfully.");
                Form f = new Form2();
                this.Hide();
                f.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message);
            }

            //
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f = new Form1();
            f.Show();
        }
    }

}




