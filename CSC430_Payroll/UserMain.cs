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
    public partial class UserMain : Form
    {

        public string Username = "";
        private List<string> Taxes = new List<string>();
        private List<string> Benefits = new List<string>();
        public UserMain()
        {
            InitializeComponent();
            
            //updateTaxes();
            //taxesListBoxRefresh();
            
            
        }
        /*public string TextBoxValue
        {
            get { return txtFirstName.Text; }
            set { txtFirstName.Text = value; }
            
        }*/
        public UserMain(string firstname, string lastname, string username)
        {
            InitializeComponent();
            txtFirstName.Text = firstname;
            txtLastName.Text = lastname;
            Username = username;
        }

        public UserMain(string firstname, string lastname, string address, string zip)
        {
            InitializeComponent();
            txtFirstName.Text = firstname;
            txtLastName.Text = lastname;
            txtAddress.Text = address;
            txtZIP.Text = zip;
           
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            this.Hide();
            UserEditForm useredit = new UserEditForm(this.txtEmployeeID.Text, this.Username);
            useredit.Show();

        }
      
        public void gridRefresh()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection 
            con.Open();

            SqlCommand query1 = new SqlCommand("SELECT ID, DOB, Address, ZIP, Hourly, HoursWorked, Tax, OvertimeWorked, Deductions, GrossPay, NetPay FROM Employee WHERE [Last Name]='" + txtLastName.Text + "'" + "AND [First Name]='" + txtFirstName.Text + "'", con);
            SqlDataReader reader = query1.ExecuteReader();

            while (reader.Read())
            {
                txtEmployeeID.Text = reader["ID"].ToString();
                var dob = reader["DOB"].ToString();
                dob = dob.Split()[0];
                txtDOB.Text = dob;
                txtAddress.Text = reader["Address"].ToString();
                txtZIP.Text = reader["ZIP"].ToString();
                this.txtHourlyPay.Text = reader["Hourly"].ToString();
                this.txtHoursWorked.Text = reader["HoursWorked"].ToString();
                this.txtOvertimeWorked.Text = reader["OvertimeWorked"].ToString();

            }

        }

        private void UserMain_Load(object sender, EventArgs e)
        {

            gridRefresh();
            updateTaxes();
            updateBenefits();
            taxesListBoxRefresh();
            benefitsListBoxRefresh();
            

        }
        public List<string> getTaxes()
        {
            return Taxes;
        }
        public List<string> getBenefits()
        {
            return Benefits;
        }
        public string getSelectionID()
        {
            return this.txtEmployeeID.Text;
        }


        private void taxesListBoxRefresh()
        {
            listBox1.Items.Clear();
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString);
            
            
            if (Taxes.Count != 0)
            {
                    con.Open();
                    for (int i = 0; i < Taxes.Count(); i++)
                    {

                    String sqlquery = "SELECT [" + Taxes[i] + "] FROM Employee WHERE ID = " + txtEmployeeID.Text;
                    SqlCommand command = new SqlCommand(sqlquery, con);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader[Taxes[i]].ToString() == "True")
                        {
                            string temp = Taxes[i];
                            temp = temp.Remove(0, 5);
                            listBox1.Items.Add(temp);

                        }
                    }
                    reader.Close();
                }
            con.Close();
            }

        }
        private void benefitsListBoxRefresh()
        {
            listBox2.Items.Clear();
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString);

            if (Benefits.Count != 0)
            {
                con.Open();
                for (int i = 0; i < Benefits.Count(); i++)
                {

                    String sqlquery = "SELECT [" + Benefits[i] + "] FROM Employee WHERE ID = " + txtEmployeeID.Text;
                    SqlCommand command = new SqlCommand(sqlquery, con);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader[Benefits[i]].ToString() == "True")
                        {
                            string temp = Benefits[i];
                            temp = temp.Remove(0, 5);
                            listBox2.Items.Add(temp);

                        }
                    }
                    reader.Close();
                }
                con.Close();
            }
        }
        private void updateTaxes()
        {
            Taxes.Clear();
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();
            String sqlquery = "SELECT column_name " +
                "FROM information_schema.columns " +
                "WHERE table_name = 'Employee'" +
                "AND column_name " +
                "LIKE 'Tax: %'";
            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Taxes.Add(reader["column_name"].ToString());
               
            }

            
            reader.Close();
            con.Close();
        }


        private void updateBenefits()
        {
            Benefits.Clear();
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();
            String sqlquery = "SELECT column_name " +
                "FROM information_schema.columns " +
                "WHERE table_name = 'Employee' " +
                "AND column_name " +
                "LIKE 'BFT: %'";
            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Benefits.Add(reader["column_name"].ToString());
            }
            reader.Close();
            con.Close();
        }
        private void frm_menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void btnPayChk_Click(object sender, EventArgs e)
        {
            UserPayChk userpayChk = new UserPayChk(txtFirstName.Text, txtLastName.Text);
            userpayChk.ShowDialog();
        }

        private void btnPayStub_Click(object sender, EventArgs e)
        {
            if (txtOvertimeWorked.Text == "")
            {
                txtOvertimeWorked.Text = 0.ToString();
            }
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
                SqlConnection con = new SqlConnection(connectionString); // making connection
                con.Open();

                List<string> TaxRates = new List<string>();
                List<string> BenefitRates = new List<string>();

                decimal taxRate = 0;
                decimal benefitRate = 0;
                decimal netPay = 0;

                decimal hoursWorked = Int32.Parse(txtHoursWorked.Text);
                decimal overtimeWorked = Int32.Parse(txtOvertimeWorked.Text);
                decimal hourlyPay = Int32.Parse(txtHourlyPay.Text);
                decimal grossPay = (hoursWorked * hourlyPay) + (overtimeWorked * hourlyPay);


                if (listBox1.Items.Count > 0)
                {
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        String sqlquery5 = "SELECT Rate FROM Taxes WHERE [Tax Name] = '" + listBox1.Items[i].ToString() + "'";
                        SqlCommand command5 = new SqlCommand(sqlquery5, con);
                        taxRate = taxRate + Convert.ToDecimal(command5.ExecuteScalar());
                        TaxRates.Add(Convert.ToString(command5.ExecuteScalar()));
                    }
                }

                /*if (listBox2.Items.Count > 0)
                {
                    for (int i = 0; i < listBox2.Items.Count; i++)
                    {
                        String sqlquery6 = "SELECT Rate FROM Benefits WHERE [Benefit Name] = '" + listBox2.Items[i].ToString() + "'";
                        SqlCommand command6 = new SqlCommand(sqlquery6, con);
                        benefitRate = benefitRate + Convert.ToDecimal(command6.ExecuteScalar());
                        BenefitRates.Add(Convert.ToString(command6.ExecuteScalar()));
                    }
                }*/

                if (benefitRate > 0 || taxRate > 0)
                {
                    netPay = grossPay - ((benefitRate + taxRate) * grossPay);
                }
                else
                    netPay = grossPay;

                String sqlquery = "UPDATE Employee SET HoursWorked = " + txtHoursWorked.Text + " WHERE ID = " + getSelectionID();
                String sqlquery1 = "UPDATE Employee SET OvertimeWorked = " + txtOvertimeWorked.Text + " WHERE ID = " + getSelectionID();
                String sqlquery2 = "UPDATE Employee SET Hourly = " + txtHourlyPay.Text + " WHERE ID = " + getSelectionID();
                String sqlquery3 = "UPDATE Employee SET GrossPay = " + grossPay.ToString() + " WHERE ID = " + getSelectionID();
                String sqlquery4 = "UPDATE Employee SET NetPay =  " + netPay.ToString();

                SqlCommand command = new SqlCommand(sqlquery, con);
                SqlCommand command1 = new SqlCommand(sqlquery1, con);
                SqlCommand command2 = new SqlCommand(sqlquery2, con);
                SqlCommand command3 = new SqlCommand(sqlquery3, con);
                SqlCommand command4 = new SqlCommand(sqlquery4, con);

                command.ExecuteNonQuery();
                command1.ExecuteNonQuery();
                command2.ExecuteNonQuery();
                command3.ExecuteNonQuery();
                command4.ExecuteNonQuery();

                con.Close();

                UserPayStub userpayStub = new UserPayStub(txtEmployeeID.Text, txtFirstName.Text, txtLastName.Text, txtAddress.Text, txtZIP.Text,
                txtDOB.Text, taxRate, benefitRate, TaxRates, BenefitRates);
                userpayStub.ShowDialog();
            }


            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        
    }
}
