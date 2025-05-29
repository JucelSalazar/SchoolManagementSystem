using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace SchoolManagementSystem
{
    public partial class Form1 : Form
    {   
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=school_management;User=root;Password=root;";
        private DataGridView dataGridView1;
        private TextBox txtStudentID, txtFirstName, txtLastName;
        private DateTimePicker dtpBirthDate;
        private RadioButton rbMale, rbFemale;
        private ComboBox cmbDepartment;
        private Button btnAddStudent, btnUpdateStudent, btnDeleteStudent, btnExport;
        private DataTable studentsTable;
        private TextBox txtUsername, txtPassword, txtEmail;
        private DataGridView dataGridViewUsers;
        

        public Form1()
        {

            InitializeComponent();

            DatabaseHelper dbHelper = new DatabaseHelper();
            if (dbHelper.TestConnection())
            {
                MessageBox.Show("Database connected successfully!");
            }
            else
            {
                MessageBox.Show("Database connection failed.");
            }
            this.Load += Form1_Load;
        }

        private void InitializeComponent()
        {
            this.Text = "Student Management System";
            this.Size = new System.Drawing.Size(800, 600);

            dataGridView1 = new DataGridView { Location = new System.Drawing.Point(20, 300), Width = 740, Height = 200 };
            dataGridView1.CellClick += dataGridView1_CellClick;

            txtStudentID = new TextBox { Location = new System.Drawing.Point(20, 20), Width = 200, PlaceholderText = "Student ID" };
            txtFirstName = new TextBox { Location = new System.Drawing.Point(20, 50), Width = 200, PlaceholderText = "First Name" };
            txtLastName = new TextBox { Location = new System.Drawing.Point(20, 80), Width = 200, PlaceholderText = "Last Name" };

            dtpBirthDate = new DateTimePicker { Location = new System.Drawing.Point(20, 110), Width = 200 };

            GroupBox genderGroup = new GroupBox {Location = new System.Drawing.Point(20, 130), Width = 200, Height = 50 };

            rbMale = new RadioButton { Text = "Male", Location = new System.Drawing.Point(10, 20) };
            rbFemale = new RadioButton { Text = "Female", Location = new System.Drawing.Point(140, 20) };

            genderGroup.Controls.Add(rbMale);
            genderGroup.Controls.Add(rbFemale);

            cmbDepartment = new ComboBox { Location = new System.Drawing.Point(20, 190), Width = 200 };
            cmbDepartment.Items.AddRange(new string[] { "Computer Science", "Business", "Engineering", "Arts" });

            btnAddStudent = new Button { Text = "Add Student", Location = new System.Drawing.Point(20, 230), Width = 100 };
            btnAddStudent.Click += btnAddStudent_Click;

            btnUpdateStudent = new Button { Text = "Update Student", Location = new System.Drawing.Point(130, 230), Width = 100 };
            btnUpdateStudent.Click += btnUpdateStudent_Click;

            btnDeleteStudent = new Button { Text = "Delete Student", Location = new System.Drawing.Point(240, 230), Width = 100 };
            btnDeleteStudent.Click += btnDeleteStudent_Click;

            btnExport = new Button { Text = "Export to Excel", Location = new System.Drawing.Point(350, 230), Width = 150 };
            btnExport.Click += btnExport_Click;

            this.Controls.AddRange(new Control[]
            {
                txtStudentID, txtFirstName, txtLastName, dtpBirthDate,
                genderGroup,
                cmbDepartment,
                btnAddStudent, btnUpdateStudent, btnDeleteStudent, btnExport,
                dataGridView1
            });
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Students (FirstName, LastName, BirthDate, Gender, DepartmentID) VALUES (@FirstName, @LastName, @BirthDate, @Gender, @DepartmentID)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@BirthDate", dtpBirthDate.Value);
                cmd.Parameters.AddWithValue("@Gender", rbMale.Checked ? "M" : "F");
                cmd.Parameters.AddWithValue("@DepartmentID", cmbDepartment.SelectedIndex + 1);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student added successfully!");
                LoadStudents();
            }
        }

        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "UPDATE Students SET FirstName=@FirstName, LastName=@LastName, BirthDate=@BirthDate, Gender=@Gender, DepartmentID=@DepartmentID WHERE StudentID=@StudentID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", txtStudentID.Text);
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@BirthDate", dtpBirthDate.Value);
                cmd.Parameters.AddWithValue("@Gender", rbMale.Checked ? "M" : "F");
                cmd.Parameters.AddWithValue("@DepartmentID", cmbDepartment.SelectedIndex + 1);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student updated successfully!");
                LoadStudents();
            }
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM Students WHERE StudentID=@StudentID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", txtStudentID.Text);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student deleted successfully!");
                LoadStudents();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView1);
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

        private void LoadUsers()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, Email, Role FROM Users";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridViewUsers.DataSource = dt;
            }
        }

        private void ExportToExcel(DataGridView dgv)
        {
            try
            {
                ExcelPackage.License.SetNonCommercialOrganization("Jucel");
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Properties.Title = "Student Report";

                    ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("Students");

                    for (int col = 0; col < dgv.Columns.Count; col++)
                    {
                        sheet.Cells[1, col + 1].Value = dgv.Columns[col].HeaderText;
                    }

                    for (int row = 0; row < dgv.Rows.Count; row++)
                    {
                        for (int col = 0; col < dgv.Columns.Count; col++)
                        {
                            sheet.Cells[row + 2, col + 1].Value = dgv.Rows[row].Cells[col].Value?.ToString();
                        }
                    }

                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "Excel Files|*.xlsx";
                        sfd.FileName = "StudentReport.xlsx";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllBytes(sfd.FileName, excel.GetAsByteArray());
                            MessageBox.Show($"Excel report saved successfully at {sfd.FileName}!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                txtStudentID.Text = row.Cells[0].Value?.ToString();
                txtFirstName.Text = row.Cells[1].Value?.ToString();
                txtLastName.Text = row.Cells[2].Value?.ToString();
                dtpBirthDate.Value = DateTime.Parse(row.Cells[3].Value?.ToString() ?? DateTime.Now.ToString());
                rbMale.Checked = row.Cells[4].Value?.ToString() == "Male";
                rbFemale.Checked = row.Cells[4].Value?.ToString() == "Female";
                cmbDepartment.SelectedItem = row.Cells[5].Value?.ToString();
            }
        }

        private void ClearInputs()
        {
            txtStudentID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            dtpBirthDate.Value = DateTime.Now;
            rbMale.Checked = false;
            rbFemale.Checked = false;
            cmbDepartment.SelectedIndex = -1;
        }

        private void LoadStudents()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT StudentID, FirstName, LastName, BirthDate, Gender, DepartmentID FROM Students";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

    }
}
