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
        //use for refresh grid
        private readonly formMain form1;

        public FormAddEmployee()
        {
            InitializeComponent();
        }
        //use for refresh grid
        public FormAddEmployee(formMain form)
        {
            InitializeComponent();
            form1 = form;
            txtZipcode.MaxLength = 10;
            this.AcceptButton = btnCreateEmployee;
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

        private void btnCreateEmployee_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection 
            con.Open();
            string sqlquery = "INSERT INTO Employee(ID, [Last Name], [First Name], DOB, Address, ZIP) VALUES(@ID, @LastName,@FirstName,@DOB,@Address,@ZIP)";
            string sqlquery1 = "SELECT COUNT(*) FROM [Employee] WHERE ([ID] = @ID)";
            
            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlCommand command1 = new SqlCommand(sqlquery1, con);

            try
            {
                int numID = Int32.Parse(this.txtEmployeeID.Text);
                command1.Parameters.AddWithValue("@ID", txtEmployeeID.Text);
                int checkID = (int)command1.ExecuteScalar();

                if (checkID > 0)
                {
                    MessageBox.Show("ID is already taken.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (numID < 0)
                {
                    MessageBox.Show("ID cannot be an integer less than 0. (ex: 0, 1, 2, 3)");
                }
                else
                {
                    command.Parameters.AddWithValue("@ID", numID);
                    command.Parameters.AddWithValue("@LastName", this.txtLastName.Text);
                    command.Parameters.AddWithValue("@FirstName", this.txtFirstName.Text);
                    command.Parameters.AddWithValue("@DOB", this.dateTimePicker1.Value);
                    command.Parameters.AddWithValue("@Address", this.txtAddress.Text);
                    command.Parameters.AddWithValue("@ZIP", this.txtZipcode.Text);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Employee Created.");
                    form1.resetPlacementValues();
                    //calling grid refresh function from FormMain
                    form1.gridRefresh();
                    this.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormAddEmployee_Load(object sender, EventArgs e)
        {

        }
    }
}