using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace CSC430_Payroll
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.AcceptButton = btnSearch;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            gridRefresh();
            /*string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection   
            SqlDataAdapter sda = new SqlDataAdapter("SELECT ID, [Last Name], [First Name] FROM Employee", con);
            con.Open();

            var commandBuilder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);*/
        }

        public void gridRefresh()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection   
            SqlDataAdapter sda = new SqlDataAdapter("SELECT ID, [Last Name], [First Name] FROM Employee ORDER BY ID ASC", con);
            con.Open();

            var commandBuilder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = ds.Tables[0];
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        
        
        private void button3_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to delete this Employee's record?",
                "Confirm Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int selectedIndex = dataGridView1.SelectedRows[0].Index;
                    string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
                    SqlConnection con = new SqlConnection(connectionString); // making connection 
                    con.Open();
                    int rowID = int.Parse(dataGridView1[0, selectedIndex].Value.ToString());
                    string sqlquery = "DELETE FROM Employee WHERE ID = " + rowID;

                    try
                    {
                        SqlCommand command = new SqlCommand(sqlquery, con);
                        command.ExecuteNonQuery();
                        string cmdString = "SELECT ID, [Last Name], [First Name] FROM Employee";
                        SqlDataAdapter sda = new SqlDataAdapter(cmdString, con);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0].DefaultView;
                        dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                //do nothing
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAddEmployee popUpForm = new FormAddEmployee(this);
            popUpForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormEditEmployee popUpForm = new FormEditEmployee();
            popUpForm.ShowDialog();
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDateOfBirth_TextChanged(object sender, EventArgs e)
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

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection

            DataGridView dgv = sender as DataGridView;
            con.Open();
            if (dgv.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow row = dgv.SelectedRows[0];
                    var numID = row.Cells["ID"].Value;
                    var lastName = row.Cells["Last Name"].Value;
                    var firstName = row.Cells["First Name"].Value;
                    string sqlquery = "SELECT DOB, Address, ZIP, Salary, Tax, Overtime, Deductions, GrossPay, NetPay FROM Employee WHERE ID = " + numID;
                    SqlCommand command = new SqlCommand(sqlquery, con);
                    SqlDataReader reader = command.ExecuteReader();
                    

                    while (reader.Read())
                    {

                        this.txtLastName.Text = lastName.ToString();
                        this.txtFirstName.Text = firstName.ToString();
                        this.txtEmployeeID.Text = numID.ToString();

                        var dob = reader["DOB"].ToString();
                        dob = dob.Split()[0];
                        this.txtDateOfBirth.Text = dob;
              
                        this.txtAddress.Text = reader["Address"].ToString();
                        this.txtZipcode.Text = reader["ZIP"].ToString();
                        this.txtSalary.Text = reader["Salary"].ToString();
                        this.txtTax.Text = reader["Tax"].ToString();
                        this.txtOvertime.Text = reader["Overtime"].ToString();
                        this.txtDeduction.Text = reader["Deductions"].ToString();
                        this.txtGrossPay.Text = reader["GrossPay"].ToString();
                        this.txtNetPay.Text = reader["NetPay"].ToString();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnBenefits_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txtGrossPay_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void txtOvertime_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void txtNetPay_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSalary_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {

        }

        public void searchData(string searchValue, string caseValue, int columnValue)
        {
            string sqlquery = ""; 
            try
            {
                switch (caseValue)
                {
                    case "ID":
                        columnValue = 0;
                        int numID = Convert.ToInt32(searchValue);
                        sqlquery = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE ID = " + numID;
                        break;
                    case "Last Name":
                        columnValue = 1;
                        sqlquery = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE [Last Name] ='"+searchValue+"'";
                        break;
                    case "First Name":
                        columnValue = 2;
                        sqlquery = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE [First Name] ='"+searchValue+"'";
                        break;
                    case "":
                        columnValue = -1;
                        break;
                }
                if (columnValue == -1)
                {
                    MessageBox.Show("Please select what value you want to search by in the dropdown box.");
                }
                if (searchValue == "")
                {
                    MessageBox.Show("Please enter the value you want to search.");
                }
                else
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
                    SqlConnection con = new SqlConnection(connectionString); // making connection   
                    SqlDataAdapter sda = new SqlDataAdapter(sqlquery, con);
                    con.Open();

                    var commandBuilder = new SqlCommandBuilder(sda);
                    var ds = new DataSet();
                    sda.Fill(ds);
                    dataGridView1.ReadOnly = true;
                    dataGridView1.DataSource = ds.Tables[0];
                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("No results found.");
                        con.Close();
                    }
                    con.Close();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text;
            string caseValue = comboBox1.Text;
            int columnValue = 0;
            searchData(searchValue, caseValue, columnValue);
        }

        private void txtTax_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDeduction_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnShowAllEmployees_Click(object sender, EventArgs e)
        {
            gridRefresh();
        }
    }
}