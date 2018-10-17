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
    public partial class formMain : Form
    {
        private int currentPage = 0;
        public int pageX = 0;
        public int pageY = 0;
        private bool searching = false;
        private bool searchingForID = false;
        private bool searchingForLastName = false;
        private bool searchingForFirstName = false;
        private string searchQuery = null;
        public formMain()
        {
            InitializeComponent();
            currentPage = 1;
            pageX = 1;
            pageY = 50;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.AcceptButton = btnSearch;
            this.comboBox1.SelectedIndex = 3;
            this.txtSearch.Enabled = false;
            this.btnSearch.Enabled = false;
            this.btnPreviousPage.Enabled = false;
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
            string adapterString = "";

            if(searching == false)
            {
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE ID BETWEEN " + pageX + " AND " + pageY + " ORDER BY ID ASC";
            }
            else if (searchingForID == true)
            {
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE ID BETWEEN " + pageX + " AND " + pageY + " ORDER BY ID ASC";
            }
            else if (searchingForLastName == true)
            {
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE ID BETWEEN " + pageX + " AND " + pageY + "AND [Last Name] ='"+this.txtLastName.Text+"'ORDER BY ID ASC";
            }
            else if (searchingForFirstName == true)
            {
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE ID BETWEEN " + pageX + " AND " + pageY + "AND [First Name] ='"+this.txtFirstName.Text+"'ORDER BY ID ASC";
            }

            SqlDataAdapter sda = new SqlDataAdapter(adapterString, con);

            con.Open();
            
            var commandBuilder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = ds.Tables[0];

            checkNextPage(con);

            con.Close();
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
                        gridRefresh();
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
            FormEditEmployee popUpForm = new FormEditEmployee(this.txtEmployeeID.Text);
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

        public string getSelectionID()
        {
            return this.txtEmployeeID.Text;
        }

        private void btnBenefits_Click(object sender, EventArgs e)
        {
            FormBenefits popUpForm = new FormBenefits();
            popUpForm.ShowDialog();
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
            string caseValue = comboBox1.Text;
            switch (caseValue)
            {
                case "ID":
                    txtSearch.Enabled = true;
                    btnSearch.Enabled = true;
                    break;
                case "Last Name":
                    txtSearch.Enabled = true;
                    btnSearch.Enabled = true;
                    break;
                case "First Name":
                    txtSearch.Enabled = true;
                    btnSearch.Enabled = true;
                    break;
                case "Show All":
                    searching = false;
                    searchingForID = false;
                    pageX = 1;
                    pageY = 50;
                    currentPage = 1;
                    searchQuery = null;
                    txtSearch.Enabled = false;
                    btnSearch.Enabled = false;
                    txtSearch.Text = "";
                    gridRefresh();
                    break;
            }
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
                        searching = true;
                        searchingForID = true;
                        searchingForLastName = false;
                        searchingForFirstName = false;
                        int numID = Convert.ToInt32(searchValue);
                        sqlquery = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE ID = " + numID;
                        searchQuery = sqlquery;
                        break;
                    case "Last Name":
                        columnValue = 1;
                        searching = true;
                        searchingForID = false;
                        searchingForLastName = true;
                        searchingForFirstName = false;
                        sqlquery = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE [Last Name] ='"+searchValue+"' AND ID BETWEEN " + pageX + " AND " + pageY;
                        searchQuery = sqlquery;
                        break;
                    case "First Name":
                        columnValue = 2;
                        searching = true;
                        searchingForID = false;
                        searchingForLastName = false;
                        searchingForFirstName = true;
                        sqlquery = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE [First Name] ='"+searchValue+"' AND ID BETWEEN " + pageX + " AND " + pageY;
                        searchQuery = sqlquery;
                        break;
                    case "Show All":
                        searching = false;
                        searchingForID = false;
                        searchingForLastName = false;
                        searchingForFirstName = false;
                        pageX = 1;
                        pageY = 50;
                        currentPage = 1;
                        searchQuery = null;
                        gridRefresh();
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
                else if (searchValue == "Show All")
                {
                    //do nothing
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
                    else
                    {
                        checkNextPage(con);
                        con.Close();
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            pageX = 1;
            pageY = 50;
            currentPage = 1;
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

        private void button6_Click(object sender, EventArgs e)
        {
            FormTaxes popUpForm = new FormTaxes();
            popUpForm.ShowDialog();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void checkPreviousPage()
        {
            if (currentPage == 1)
            {
                btnPreviousPage.Enabled = false;
            }
            else
            {
                btnPreviousPage.Enabled = true;
            }
        }

        private void checkNextPage(SqlConnection con)
        {
            if (searching == false)
            {
                int temp = pageY;
                string sqlquery = "SELECT ID FROM Employee WHERE ID > " + temp;
                SqlCommand command = new SqlCommand(sqlquery, con);
                SqlDataReader reader = command.ExecuteReader();
                string checkID = null;

                while (reader.Read())
                {
                    checkID = reader["ID"].ToString();
                }
                if (checkID == null)
                {
                    this.btnNextPage.Enabled = false;
                }
                else
                {
                    this.btnNextPage.Enabled = true;
                }
            }
            else if(searchingForID == true)
            {
                this.btnNextPage.Enabled = false;
                this.btnPreviousPage.Enabled = false;
            }
            else if(searchingForLastName == true)
            {
                int temp = pageY;
                string sqlquery = "SELECT ID FROM Employee WHERE ID > '" + temp + "' AND [Last Name] = '" +this.txtSearch.Text+ "'";
                SqlCommand command = new SqlCommand(sqlquery, con);
                SqlDataReader reader = command.ExecuteReader();
                string checkID = null;

                while (reader.Read())
                {
                    checkID = reader["ID"].ToString();
                }
                if (checkID == null)
                {
                    this.btnNextPage.Enabled = false;
                }
                else
                {
                    this.btnNextPage.Enabled = true;
                }
            }
            else if (searchingForFirstName == true)
            {
                int temp = pageY;
                string sqlquery = "SELECT ID FROM Employee WHERE ID > '" + temp + "' AND [First Name] = '" + this.txtSearch.Text + "'";
                SqlCommand command = new SqlCommand(sqlquery, con);
                SqlDataReader reader = command.ExecuteReader();
                string checkID = null;

                while (reader.Read())
                {
                    checkID = reader["ID"].ToString();
                }
                if (checkID == null)
                {
                    this.btnNextPage.Enabled = false;
                }
                else
                {
                    this.btnNextPage.Enabled = true;
                }
            }
        }
        
        private void btnNextPage_Click(object sender, EventArgs e)
        {
                pageX += 50;
                pageY += 50;
                currentPage++;
                checkPreviousPage();
                this.labelPageNumber.Text = "Page " + currentPage;
                gridRefresh();
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
                pageX -= 50;
                pageY -= 50;
                currentPage--;
                checkPreviousPage();
                this.labelPageNumber.Text = "Page " + currentPage;
                gridRefresh();
        }
    }
}