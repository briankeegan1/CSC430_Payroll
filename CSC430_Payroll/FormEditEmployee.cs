using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSC430_Payroll
{
    public partial class FormEditEmployee : Form
    {
        //use for refresh grid
        formMain form1 = Application.OpenForms.OfType<formMain>().Single();
        public string numID = "";

        public FormEditEmployee(string employeeID)
        {
            numID = employeeID;
            InitializeComponent();
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtZipcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmployeeID_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();
            //string sqlquery = "INSERT INTO Employee(ID, [Last Name], [First Name], DOB, Address, ZIP) VALUES(@ID, @LastName,@FirstName,@DOB,@Address,@ZIP)";
            string sqlquery = "UPDATE Employee SET ID = @ID, [Last Name] = @LastName, [First Name] = @FirstName, DOB = @DOB, Address = @Address, ZIP = @ZIP WHERE ID = " + numID;
            string sqlquery1 = "SELECT COUNT(*) FROM [Employee] WHERE ([ID] = @ID)";

            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlCommand command1 = new SqlCommand(sqlquery1, con);


            int numID1 = Int32.Parse(this.txtEmployeeID.Text);
            command1.Parameters.AddWithValue("@ID", txtEmployeeID.Text);
            int checkID = (int)command1.ExecuteScalar();


            if (checkID > 0 && numID1 != Int32.Parse(numID))
            {
                MessageBox.Show("ID is already taken.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                command.Parameters.AddWithValue("@ID", numID1);
                command.Parameters.AddWithValue("@LastName", this.txtLastName.Text);
                command.Parameters.AddWithValue("@FirstName", this.txtFirstName.Text);
                command.Parameters.AddWithValue("@DOB", this.dateTimePicker1.Value);
                command.Parameters.AddWithValue("@Address", this.txtAddress.Text);
                command.Parameters.AddWithValue("@ZIP", this.txtZipcode.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Employee information updated.");
                //calling grid refresh function from FormMain
                form1.gridRefresh();
                this.Close();
            }

        }

        private void FormEditEmployee_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();

            string sqlquery = "SELECT [Last Name], [First Name], DOB, Address, ZIP, Salary, Tax, Overtime, Deductions, GrossPay, NetPay FROM Employee WHERE ID = " + numID;
            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {

                this.txtLastName.Text = reader["Last Name"].ToString();
                this.txtFirstName.Text = reader["First Name"].ToString();
                this.txtEmployeeID.Text = numID;

                var dob = reader["DOB"].ToString();
                this.dateTimePicker1.Text = dob;

                this.txtAddress.Text = reader["Address"].ToString();
                this.txtZipcode.Text = reader["ZIP"].ToString();
                //this.txtSalary.Text = reader["Salary"].ToString();
                //this.txtTax.Text = reader["Tax"].ToString();
                //this.txtOvertime.Text = reader["Overtime"].ToString();
                //this.txtDeduction.Text = reader["Deductions"].ToString();
                //this.txtGrossPay.Text = reader["GrossPay"].ToString();
                //this.txtNetPay.Text = reader["NetPay"].ToString();
            }
            con.Close();
        }
    }
}
