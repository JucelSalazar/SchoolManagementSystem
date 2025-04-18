using System;
using System.Data;
using System.Windows.Forms;

namespace SchoolManagementSystem
{
    public class Form1 : Form
    {
        private DataGridView dataGridView1;
        private TextBox txtStudentID, txtFirstName, txtLastName;
        private DateTimePicker dtpBirthDate;
        private RadioButton rbMale, rbFemale;
        private ComboBox cmbDepartment;
        private Button btnAddStudent, btnUpdateStudent, btnDeleteStudent;

        private DataTable studentsTable;

        public Form1()
        {
            InitializeComponent();
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

            rbMale = new RadioButton { Text = "Male", Location = new System.Drawing.Point(20, 140) };
            rbFemale = new RadioButton { Text = "Female", Location = new System.Drawing.Point(80, 140) };

            cmbDepartment = new ComboBox { Location = new System.Drawing.Point(20, 170), Width = 200 };
            cmbDepartment.Items.AddRange(new string[] { "Computer Science", "Business", "Engineering", "Arts" });

            btnAddStudent = new Button { Text = "Add Student", Location = new System.Drawing.Point(20, 210), Width = 100 };
            btnAddStudent.Click += btnAddStudent_Click;

            btnUpdateStudent = new Button { Text = "Update Student", Location = new System.Drawing.Point(130, 210), Width = 100 };
            btnUpdateStudent.Click += btnUpdateStudent_Click;

            btnDeleteStudent = new Button { Text = "Delete Student", Location = new System.Drawing.Point(240, 210), Width = 100 };
            btnDeleteStudent.Click += btnDeleteStudent_Click;

            this.Controls.AddRange(new Control[]
            {
                txtStudentID, txtFirstName, txtLastName, dtpBirthDate,
                rbMale, rbFemale, cmbDepartment,
                btnAddStudent, btnUpdateStudent, btnDeleteStudent,
                dataGridView1
            });

            studentsTable = new DataTable();
            studentsTable.Columns.Add("ID");
            studentsTable.Columns.Add("First Name");
            studentsTable.Columns.Add("Last Name");
            studentsTable.Columns.Add("Birth Date");
            studentsTable.Columns.Add("Gender");
            studentsTable.Columns.Add("Department");

            dataGridView1.DataSource = studentsTable;
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            studentsTable.Rows.Add(
                txtStudentID.Text,
                txtFirstName.Text,
                txtLastName.Text,
                dtpBirthDate.Value.ToShortDateString(),
                rbMale.Checked ? "Male" : "Female",
                cmbDepartment.SelectedItem?.ToString()
            );

            ClearInputs();
        }

        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var row = dataGridView1.CurrentRow;
                row.Cells[0].Value = txtStudentID.Text;
                row.Cells[1].Value = txtFirstName.Text;
                row.Cells[2].Value = txtLastName.Text;
                row.Cells[3].Value = dtpBirthDate.Value.ToShortDateString();
                row.Cells[4].Value = rbMale.Checked ? "Male" : "Female";
                row.Cells[5].Value = cmbDepartment.SelectedItem?.ToString();

                ClearInputs();
            }
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                ClearInputs();
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
    }
}
