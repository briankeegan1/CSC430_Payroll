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
        UserMain form2 = Application.OpenForms.OfType<UserMain>().FirstOrDefault();

        public string numID = "";
        public string Username = "";
        private List<string> Benefits = new List<string>();
        private List<string> Taxes = new List<string>();
        public UserEditForm()
        {
            
            InitializeComponent();
            this.AcceptButton = btnOK;
        }
        public UserEditForm(string employeeID, string username)
        {
            numID =  employeeID;
            Username = username;
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
            string sqlquery = "UPDATE Employee SET [Last Name] = @LastName, [First Name] = @FirstName, DOB = @DOB, Address = @Address, ZIP = @ZIP WHERE ID = " + numID;
        
            //prevent sql injection by doing this, thts wat google said
            string sqlquery2 = "UPDATE [User] SET [LastName] = @LastName, [FirstName] = @FirstName WHERE Username = @Username";
            
            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlCommand command2 = new SqlCommand(sqlquery2, con);
            int numD1 = 0;
            
            try
            {
                numD1 = Int32.Parse(this.txtUserID.Text);
                command.Parameters.AddWithValue("@LastName", this.txtLastName.Text);
                command.Parameters.AddWithValue("@FirstName", this.txtFirstName.Text);
                command.Parameters.AddWithValue("@DOB", this.dateTimePicker1.Value);
                command.Parameters.AddWithValue("@Address", this.txtAddress.Text);
                command.Parameters.AddWithValue("@ZIP", this.txtZipcode.Text);
               
                command.ExecuteNonQuery();

                command2.Parameters.AddWithValue("@LastName", txtLastName.Text);
                command2.Parameters.AddWithValue("@FirstName",txtFirstName.Text);
                command2.Parameters.Add("@Username", SqlDbType.VarChar);
                command2.Parameters["@Username"].Value = Username;
                command2.ExecuteNonQuery();
                editTaxes(con, numD1);
                editBenefits(con, numD1);
                MessageBox.Show("Information updated.");

                
                UserMain usermain = new UserMain(this.txtFirstName.Text, this.txtLastName.Text,
                    this.txtAddress.Text,this.txtZipcode.Text);
                form2.gridRefresh();
                usermain.Show();
                this.Close();


                //another way of doing the above without sending a sht ton of parameters.. might have to make alot of function gets
                /*UserMain userMain = new UserMain();
                userMain.TextBoxValue = txtFirstName.Text;
               // userMain.TextBoxValue = txtLastName.Text;
                userMain.ShowDialog();
                this.Close();*/


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        private void refreshTaxesListBox()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            Taxes = form2.getTaxes();
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
        private void editBenefits(SqlConnection con, int id)
        {
            string sqlquery = "";
            for (int i = 0; i < Benefits.Count(); i++)
            {
                if (checkedListBox2.GetItemChecked(i) == true)
                {
                    sqlquery = "UPDATE Employee SET [" + Benefits[i] + "] = " + 1 + "WHERE ID = " + id;
                    SqlCommand command = new SqlCommand(sqlquery, con);
                    command.ExecuteNonQuery();
                }
                else
                {
                    sqlquery = "UPDATE Employee SET [" + Benefits[i] + "] = " + 0 + "WHERE ID = " + id;
                    SqlCommand command = new SqlCommand(sqlquery, con);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void refreshBenefitsListBox()
        {
            
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            Benefits = form2.getBenefits();
            con.Open();
            for (int i = 0; i < Benefits.Count(); i++)
            {
                checkedListBox2.Items.Add(Benefits[i]);
                String sqlquery = "SELECT [" + Benefits[i] + "] FROM Employee WHERE ID = " + numID;
                SqlCommand command = new SqlCommand(sqlquery, con);
                bool temp = (bool)command.ExecuteScalar();
                if (temp == true)
                {
                    checkedListBox2.SetItemChecked(i, true);
                }
            }
            con.Close();
        }
        private void UserEditForm_Load(object sender, EventArgs e)
        {
            txtZipcode.MaxLength = 10;
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();

            string sqlquery = "SELECT [Last Name], [First Name], DOB, Address, ZIP FROM Employee WHERE ID = " + numID;
            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                this.txtUserID.Text = numID;
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
        private void frm_menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}

