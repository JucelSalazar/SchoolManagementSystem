using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem
{
    public partial class FormLogin : Form
    {
        private TextBox txtUsername, txtPassword;
        private Button btnLogin;
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=school_management;User=root;Password=root;";
        private Button btnForgotPassword, btnRegister;
        public FormLogin()
        {
            InitializeComponent();
        }
    
        private void InitializeComponent()
        {
            this.Text = "Login";
            this.Size = new System.Drawing.Size(400, 300);

            txtUsername = new TextBox { Location = new System.Drawing.Point(50, 50), Width = 250, PlaceholderText = "Username" };
            txtPassword = new TextBox { Location = new System.Drawing.Point(50, 100), Width = 250, PlaceholderText = "Password", UseSystemPasswordChar = true };

            btnLogin = new Button { Text = "Login", Location = new System.Drawing.Point(50, 150), Width = 100 };
            btnLogin.Click += btnLogin_Click;

            btnForgotPassword = new Button { Text = "Forgot Password?", Location = new System.Drawing.Point(170, 150), Width = 120 };
            btnForgotPassword.Click += btnForgotPassword_Click;

            btnRegister = new Button { Text = "Register", Location = new System.Drawing.Point(50, 200), Width = 100 };
            btnRegister.Click += btnRegister_Click;

            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnForgotPassword);
            this.Controls.Add(btnRegister);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username=@Username AND PasswordHash=SHA2(@Password, 256)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show("Login successful!");
                        this.Hide(); 

                        Form1 mainForm = new Form1();
                        mainForm.ShowDialog(); 
                        Application.Exit(); 
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                    }
                }
            }
        }
        
        private void btnRegister_Click(object sender, EventArgs e)
        {
            new FormRegister().ShowDialog(); 
        }

        private void btnForgotPassword_Click(object sender, EventArgs e)
        {
            new FormForgotPassword().ShowDialog(); 
        }
    }
}
