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
    public partial class UserEditForm : Form
    {
        //use for refresh grid
        

        public UserEditForm()
        {
            
            InitializeComponent();
            this.AcceptButton = btnOK;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();
            //string sqlquery = "INSERT INTO Employee(ID, [Last Name], [First Name], DOB, Address, ZIP) VALUES(@ID, @LastName,@FirstName,@DOB,@Address,@ZIP)";
            string sqlquery = "UPDATE Employee [Last Name] = @LastName, [First Name] = @FirstName, DOB = @DOB, Address = @Address, ZIP = @ZIP";
            

            SqlCommand command = new SqlCommand(sqlquery, con);
          
            try
            {
                command.Parameters.AddWithValue("@LastName", this.txtLastName.Text);
                command.Parameters.AddWithValue("@FirstName", this.txtFirstName.Text);
                command.Parameters.AddWithValue("@DOB", this.dateTimePicker1.Value);
                command.Parameters.AddWithValue("@Address", this.txtAddress.Text);
                command.Parameters.AddWithValue("@ZIP", this.txtZipcode.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Information updated.");
               // form1.resetPlacementValues();
                //calling grid refresh function from FormMain
                //form1.gridRefresh();
                this.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormEditEmployee_Load(object sender, EventArgs e)
        {
            txtZipcode.MaxLength = 10;
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();

            string sqlquery = "SELECT [Last Name], [First Name], DOB, Address, ZIP, Salary, Tax, Overtime, Deductions, GrossPay, NetPay FROM Employee  ";
            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {

                this.txtLastName.Text = reader["Last Name"].ToString();
                this.txtFirstName.Text = reader["First Name"].ToString();
                

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

