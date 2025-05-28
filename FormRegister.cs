using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem
{
    public partial class FormRegister : Form
    {
        private TextBox txtUsername, txtPassword, txtEmail;
        private Button btnRegister;
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=school_management;User=root;Password=root;";

        public FormRegister()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Register";
            this.Size = new System.Drawing.Size(400, 300);

            txtUsername = new TextBox { Location = new System.Drawing.Point(50, 50), Width = 250, PlaceholderText = "Username" };
            txtPassword = new TextBox { Location = new System.Drawing.Point(50, 100), Width = 250, PlaceholderText = "Password", UseSystemPasswordChar = true };
            txtEmail = new TextBox { Location = new System.Drawing.Point(50, 150), Width = 250, PlaceholderText = "Email" };

            btnRegister = new Button { Text = "Register", Location = new System.Drawing.Point(50, 200), Width = 100 };
            btnRegister.Click += btnRegister_Click;

            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(txtEmail);
            this.Controls.Add(btnRegister);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Username, PasswordHash, Email, Role) VALUES (@Username, SHA2(@Password, 256), @Email, 'User')";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                conn.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Account registered successfully! You can now log in.");
                this.Close();
            }
        }
    }
}
