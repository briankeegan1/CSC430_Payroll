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
