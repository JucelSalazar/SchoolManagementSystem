using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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
                cmd.ExecuteNonQuery();

                MessageBox.Show("Password reset link has been sent to your email.");
                this.Close();
            }
        }
    }
}