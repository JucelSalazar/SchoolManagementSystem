using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem
{
    public partial class FormResetPassword : Form
    {
        private TextBox txtEmail, txtToken, txtNewPassword, txtConfirmPassword;
        private Button btnResetPassword;
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=school_management;User=root;Password=root;";

        public FormResetPassword()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Reset Password";
            this.Size = new System.Drawing.Size(400, 350);

            txtEmail = new TextBox { Location = new System.Drawing.Point(50, 30), Width = 250, PlaceholderText = "Email" };
            txtToken = new TextBox { Location = new System.Drawing.Point(50, 70), Width = 250, PlaceholderText = "Reset Token" };
            txtNewPassword = new TextBox { Location = new System.Drawing.Point(50, 110), Width = 250, PlaceholderText = "New Password", UseSystemPasswordChar = true };
            txtConfirmPassword = new TextBox { Location = new System.Drawing.Point(50, 150), Width = 250, PlaceholderText = "Confirm Password", UseSystemPasswordChar = true };

            btnResetPassword = new Button { Text = "Reset Password", Location = new System.Drawing.Point(50, 200), Width = 150 };
            btnResetPassword.Click += btnResetPassword_Click;

            this.Controls.Add(txtEmail);
            this.Controls.Add(txtToken);
            this.Controls.Add(txtNewPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(btnResetPassword);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string token = txtToken.Text.Trim();
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string checkTokenQuery = "SELECT * FROM Users WHERE Email=@Email AND ResetToken=@Token";
                MySqlCommand checkCmd = new MySqlCommand(checkTokenQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);
                checkCmd.Parameters.AddWithValue("@Token", token);

                using (MySqlDataReader reader = checkCmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        MessageBox.Show("Invalid email or reset token.");
                        return;
                    }
                }

                string updateQuery = "UPDATE Users SET PasswordHash=SHA2(@NewPassword, 256), ResetToken=NULL WHERE Email=@Email";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                updateCmd.Parameters.AddWithValue("@Email", email);
                int rowsAffected = updateCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Password successfully reset.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to reset password.");
                }
            }
        }
    }
}
