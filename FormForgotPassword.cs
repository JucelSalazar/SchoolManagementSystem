using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Net;


namespace SchoolManagementSystem
{
    public partial class FormForgotPassword : Form
    {
        private TextBox txtEmail;
        private Button btnSendReset;
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=school_management;User=root;Password=root;";

        public FormForgotPassword()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Reset Password";
            this.Size = new System.Drawing.Size(400, 200);

            txtEmail = new TextBox { Location = new System.Drawing.Point(50, 50), Width = 250, PlaceholderText = "Enter your email" };
            btnSendReset = new Button { Text = "Send Reset Link", Location = new System.Drawing.Point(50, 100), Width = 150 };
            btnSendReset.Click += btnSendReset_Click;

            this.Controls.Add(txtEmail);
            this.Controls.Add(btnSendReset);
        }

        private void btnSendReset_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string token = Guid.NewGuid().ToString();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "UPDATE Users SET ResetToken=@Token WHERE Email=@Email";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    try
                    {
                        SendResetEmail(email, token);
                        MessageBox.Show("Password reset link has been sent to your email.");

                        FormResetPassword resetForm = new FormResetPassword();
                        resetForm.ShowDialog();

                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error sending email: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Email not found.");
                }
            }
}

        private void SendResetEmail(string toEmail, string token)
        {
            string fromEmail = "jucelastig@gmail.com";
            string password = "fsti bzsy zjiy jymu";

            string subject = "Password Reset Request";
            string body = $"Use the following token to reset your password:\n\n{token}";

            MailMessage message = new MailMessage(fromEmail, toEmail, subject, body);

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            smtp.Send(message);
        }
    }
}