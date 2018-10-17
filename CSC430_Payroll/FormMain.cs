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
        //the program always knows what it is searching for. these conditions are important
        //in order to access the right SQL commands.
        private bool searching = false;
        private bool searchingForID = false;
        private bool searchingForLastName = false;
        private bool searchingForFirstName = false;
        private string searchValue = null;

        //needed to help us keep track of what's being displayed on the datagrid.
        //this is essential to controlling the page system.
        private int currentPage = 1;
        private int pageX = 0;
        private int displayCount = 0;
        private Int32 queryCount = 0;
        private Stack<int> previousDisplayCounts = new Stack<int>();
        private bool nextButtonClicked = false;
        private bool previousButtonClicked = false;

        //checks whether it's the first time running the program or clicking the search button
        //used often in gridRefresh()
        private bool initialRun = true;

        public formMain()
        {
            InitializeComponent();
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.AcceptButton = btnSearch;
            this.comboBox1.SelectedIndex = 3;
            this.txtSearch.Enabled = false;
            this.btnSearch.Enabled = false;
            this.btnPreviousPage.Enabled = false;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            gridRefresh();
        }

        public void gridRefresh()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection  
            string adapterString = "";


            if (searching == false)
            {
                SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM Employee", con);
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee ORDER BY ID ASC";
                con.Open();
                queryCount = (Int32)countCommand.ExecuteScalar();
            }
            else if (searchingForID == true)
            {
                int numID = Int32.Parse(searchValue);
                SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE ID = " + numID, con);
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE ID = " + numID + " ORDER BY ID ASC";
                con.Open();
                queryCount = (Int32)countCommand.ExecuteScalar();
            }
            else if (searchingForLastName == true)
            {
                SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE [Last Name] ='" + searchValue + "'", con);
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE[Last Name] = '" + searchValue + "' ORDER BY ID ASC";
                con.Open();
                queryCount = (Int32)countCommand.ExecuteScalar();
            }
            else if (searchingForFirstName == true)
            {
                SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE [First Name] ='" + searchValue + "'", con);
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE[First Name] = '" + searchValue + "' ORDER BY ID ASC";
                con.Open();
                queryCount = (Int32)countCommand.ExecuteScalar();
            }

            SqlDataAdapter sda = new SqlDataAdapter(adapterString, con);

            var commandBuilder = new SqlCommandBuilder(sda);
            var ds = new DataSet();

            ds.Tables.Add("Employee");
            sda.Fill(ds, pageX, 50, "Employee");

            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = ds.Tables[0];

            if (nextButtonClicked == true)
            {
                displayCount += ds.Tables["Employee"].Rows.Count;
                previousDisplayCounts.Push(displayCount);
            }
            else if (previousButtonClicked == true)
            {
                previousDisplayCounts.Pop();
                displayCount = previousDisplayCounts.Pop();
                previousDisplayCounts.Push(displayCount);
            }

            if (initialRun == true)
            {
                displayCount = ds.Tables["Employee"].Rows.Count;
                previousDisplayCounts.Push(displayCount);
                initialRun = false;
            }

            if (displayCount < queryCount)
            {
                btnNextPage.Enabled = true;
            }
            else if (displayCount >= queryCount)
            {
                btnNextPage.Enabled = false;

            }
            nextButtonClicked = false;
            previousButtonClicked = false;


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
            if (e.KeyCode == Keys.Right && btnNextPage.Enabled == true)
            { btnNextPage.PerformClick(); }
            else if (e.KeyCode == Keys.Left && btnPreviousPage.Enabled == true)
            { btnPreviousPage.PerformClick(); }
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
                    pageX = 0;
                    currentPage = 1;
                    labelPageNumber.Text = "Page " + currentPage.ToString();
                    txtSearch.Enabled = false;
                    btnSearch.Enabled = false;
                    checkPreviousPage();
                    txtSearch.Text = "";
                    initialRun = true;
                    gridRefresh();
                    break;
            }
        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {

        }

        public void searchData(string caseValue, int columnValue)
        {
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
                        break;

                    case "Last Name":
                        columnValue = 1;
                        searching = true;
                        searchingForID = false;
                        searchingForLastName = true;
                        searchingForFirstName = false;
                        break;

                    case "First Name":
                        columnValue = 2;
                        searching = true;
                        searchingForID = false;
                        searchingForLastName = false;
                        searchingForFirstName = true;
                        break;

                    case "Show All":
                        searching = false;
                        searchingForID = false;
                        searchingForLastName = false;
                        searchingForFirstName = false;
                        pageX = 0;
                        currentPage = 1;
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
                    gridRefresh();
                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("No Results.");
                    }
                    else
                    {
                        //do nothing
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
            pageX = 0;
            currentPage = 1;
            initialRun = true;
            labelPageNumber.Text = "Page " + currentPage.ToString();
            searchValue = txtSearch.Text;
            string caseValue = comboBox1.Text;
            checkPreviousPage();
            int columnValue = 0;
            searchData(caseValue, columnValue);
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

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            nextButtonClicked = true;
            pageX += 50;
            currentPage++;
            checkPreviousPage();
            labelPageNumber.Text = "Page " + currentPage;
            gridRefresh();
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            previousButtonClicked = true;
            pageX -= 50;
            currentPage--;
            checkPreviousPage();
            labelPageNumber.Text = "Page " + currentPage;
            gridRefresh();
        }
    }
}