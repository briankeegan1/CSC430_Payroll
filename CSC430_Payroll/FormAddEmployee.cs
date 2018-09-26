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
    public partial class FormAddEmployee : Form
    {
        public FormAddEmployee()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

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
            string sqlquery = "INSERT INTO Employee(ID, [Last Name], [First Name], DOB, Address, ZIP) VALUES(@ID, @LastName,@FirstName,@DOB,@Address,@ZIP)";

            try
            {
                SqlCommand command = new SqlCommand(sqlquery, con);

                int numID = Int32.Parse(this.txtEmployeeID.Text);
                command.Parameters.AddWithValue("@ID", numID);
                command.Parameters.AddWithValue("@LastName", this.txtLastName.Text);
                command.Parameters.AddWithValue("@FirstName", this.txtFirstName.Text);
                command.Parameters.AddWithValue("@DOB", this.dateTimePicker1.Value); 
                command.Parameters.AddWithValue("@Address", this.txtAddress.Text);
                command.Parameters.AddWithValue("@ZIP", this.txtZipcode.Text);

                command.ExecuteNonQuery();
                MessageBox.Show("Employee Created.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
