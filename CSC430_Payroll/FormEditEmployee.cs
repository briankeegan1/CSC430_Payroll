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

        private List<string> Benefits = new List<string>();
        private List<string> Taxes = new List<string>();

        public FormEditEmployee(string employeeID)
        {
            numID = employeeID;
            InitializeComponent();
            this.AcceptButton = btnOK;
            refreshTaxesListBox();
            refreshBenefitsListBox();
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
            string sqlquery = "UPDATE Employee SET ID = @ID, [Last Name] = @LastName, [First Name] = @FirstName, DOB = @DOB, Address = @Address, ZIP = @ZIP WHERE ID = " + numID;
            string sqlquery1 = "SELECT COUNT(*) FROM [Employee] WHERE ([ID] = @ID)";

            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlCommand command1 = new SqlCommand(sqlquery1, con);

            int numID1 = 0;
            int checkID = 0;

            try
            {
                numID1 = Int32.Parse(this.txtEmployeeID.Text);
                command1.Parameters.AddWithValue("@ID", txtEmployeeID.Text);
                checkID = (int)command1.ExecuteScalar();
                if (checkID > 0 && numID1 != Int32.Parse(numID))
                {
                    MessageBox.Show("ID is already taken.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (numID1 < 0)
                {
                    MessageBox.Show("ID cannot be an integer less than 0. (ex: 0, 1, 2, 3)");
                }
                else
                {
                    command.Parameters.AddWithValue("@ID", numID1);
                    command.Parameters.AddWithValue("@LastName", this.txtLastName.Text);
                    command.Parameters.AddWithValue("@FirstName", this.txtFirstName.Text);
                    command.Parameters.AddWithValue("@DOB", this.dateTimePicker1.Value);
                    command.Parameters.AddWithValue("@Address", this.txtAddress.Text);
                    command.Parameters.AddWithValue("@ZIP", this.txtZipcode.Text);
                    editTaxes(con, numID1);
                    editBenefits(con, numID1);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Employee information updated.");
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

        private void FormEditEmployee_Load(object sender, EventArgs e)
        {
            txtZipcode.MaxLength = 10;
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();

            string sqlquery = "SELECT [Last Name], [First Name], DOB, Address, ZIP, Salary, Tax, OvertimeWorked, Deductions, GrossPay, NetPay FROM Employee WHERE ID = " + numID;
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

        private void editTaxes(SqlConnection con, int id)
        {
            string sqlquery = "";
            for (int i = 0; i < Taxes.Count(); i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true)
                {
                    sqlquery = "UPDATE Employee SET [" + Taxes[i] + "] = " + 1 + "WHERE ID = " + id;
                    SqlCommand command = new SqlCommand(sqlquery, con);
                    command.ExecuteNonQuery();
                }
                else
                {
                    sqlquery = "UPDATE Employee SET [" + Taxes[i] + "] = " + 0 + "WHERE ID = " + id;
                    SqlCommand command = new SqlCommand(sqlquery, con);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void editBenefits(SqlConnection con, int id)
        {
            string sqlquery = "";
            string benefitsString = "";

            if (checkedListBox2.CheckedItems.Count > 0)
            {
                foreach (object Item in checkedListBox2.CheckedItems)
                {
                    benefitsString += Item.ToString();
                    benefitsString += ",";
                }
                benefitsString = benefitsString.Substring(0, (benefitsString.Length - 1));
            }
            else
                benefitsString = "";

            sqlquery = "UPDATE Employee SET Benefits =" +"'"+benefitsString+"'"+ " WHERE ID = " + id;
            SqlCommand command = new SqlCommand(sqlquery, con);
            command.ExecuteNonQuery();
        }

        private void refreshTaxesListBox()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            Taxes = form1.getTaxes();
            con.Open();
            for (int i = 0; i < Taxes.Count(); i++)
            {
                checkedListBox1.Items.Add(Taxes[i]);
                String sqlquery = "SELECT [" + Taxes[i] + "] FROM Employee WHERE ID = " + numID;
                SqlCommand command = new SqlCommand(sqlquery, con);
                bool temp = (bool)command.ExecuteScalar();
                if (temp == true)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            con.Close();
        }

        private void refreshBenefitsListBox()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            Benefits = form1.getBenefits();
            for (int i = 0; i < Benefits.Count(); i++)
            {
                con.Open();
                checkedListBox2.Items.Add(Benefits[i]);
                String sqlquery = "SELECT Benefits FROM Employee WHERE ID = " + numID;
                SqlCommand command = new SqlCommand(sqlquery, con);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Benefits"].ToString().Contains(Benefits[i]))
                    {
                        checkedListBox2.SetItemChecked(i, true);
                    }
                }
                con.Close();
            }
        }
    }
}
