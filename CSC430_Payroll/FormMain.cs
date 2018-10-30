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
        private List<string> Taxes = new List<string>();
        private List<string> Benefits = new List<string>();

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
        private decimal totalPages = 1;
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
            updateTaxes();
            updateBenefits();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.AcceptButton = btnSearch;
            this.comboBox1.SelectedIndex = 3;
            this.txtSearch.Enabled = false;
            this.btnSearch.Enabled = false;
            this.btnPreviousPage.Enabled = false;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void resetPlacementValues()
        {
            currentPage = 1;
            pageX = 0;
            totalPages = 1;
            displayCount = 0;
            queryCount = 0;
            previousDisplayCounts.Clear();
            nextButtonClicked = false;
            previousButtonClicked = false;
            checkPreviousPage();
            initialRun = true;
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
                decimal temp = queryCount;
                totalPages = (temp / 50);
                totalPages = (int)Math.Ceiling((decimal)totalPages);
            }
            else if (searchingForID == true)
            {
                int numID = Int32.Parse(searchValue);
                SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE ID = " + numID, con);
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE ID = " + numID + " ORDER BY ID ASC";
                con.Open();
                queryCount = (Int32)countCommand.ExecuteScalar();
                decimal temp = queryCount;
                totalPages = (temp / 50);
                totalPages = (int)Math.Ceiling((decimal)totalPages);
            }
            else if (searchingForLastName == true)
            {
                SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE [Last Name] ='" + searchValue + "'", con);
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE[Last Name] = '" + searchValue + "' ORDER BY ID ASC";
                con.Open();
                queryCount = (Int32)countCommand.ExecuteScalar();
                decimal temp = queryCount;
                totalPages = (temp / 50);
                totalPages = (int)Math.Ceiling((decimal)totalPages);
            }
            else if (searchingForFirstName == true)
            {
                SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE [First Name] ='" + searchValue + "'", con);
                adapterString = "SELECT ID, [Last Name], [First Name] FROM Employee WHERE[First Name] = '" + searchValue + "' ORDER BY ID ASC";
                con.Open();
                queryCount = (Int32)countCommand.ExecuteScalar();
                decimal temp = queryCount;
                totalPages = (temp / 50);
                totalPages = (int)Math.Ceiling((decimal)totalPages);
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

            if (totalPages < 1)
            {
                totalPages = 1;
            }

            if (queryCount < 1)
            {
                btnEditEmployee.Enabled = false;
                btnDeleteEmployee.Enabled = false;
            }
            else
            {
                btnEditEmployee.Enabled = true;
                btnDeleteEmployee.Enabled = true;
            }

            if (ds.Tables["Employee"].Rows.Count < 1)
            {
                btnPreviousPage.PerformClick();
                btnNextPage.Enabled = false;
            }

            labelPageNumber.Text = "Page " + currentPage.ToString() + " of " + totalPages.ToString();
            labelResults.Text = "Results: " + queryCount;
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

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
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

        public List<string> getTaxes()
        {
            return Taxes;
        }

        public List<string> getBenefits()
        {
            return Benefits;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAddEmployee popUpForm = new FormAddEmployee(this);
            popUpForm.ShowDialog();
        }

        private void btnEditEmployee_Click(object sender, EventArgs e)
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
            employeeInfoRefresh(con, dgv);
            taxesListBoxRefresh(con, dgv);
            benefitsListBoxRefresh(con, dgv);
        }

        private void employeeInfoRefresh(SqlConnection con, DataGridView dgv)
        {
            con.Open();
            if (dgv.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow row = dgv.SelectedRows[0];
                    var id = row.Cells["ID"].Value;
                    var lastName = row.Cells["Last Name"].Value;
                    var firstName = row.Cells["First Name"].Value;
                    string sqlquery = "SELECT DOB, Address, ZIP, Hourly, HoursWorked, Tax, OvertimeWorked, Deductions, GrossPay, NetPay FROM Employee WHERE ID = " + id;
                    SqlCommand command = new SqlCommand(sqlquery, con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        this.txtLastName.Text = lastName.ToString();
                        this.txtFirstName.Text = firstName.ToString();
                        this.txtEmployeeID.Text = id.ToString();
                        var dob = reader["DOB"].ToString();
                        dob = dob.Split()[0];
                        this.txtDateOfBirth.Text = dob;
                        this.txtAddress.Text = reader["Address"].ToString();
                        this.txtZipcode.Text = reader["ZIP"].ToString();
                        this.txtHourlyPay.Text = reader["Hourly"].ToString();
                        this.txtHoursWorked.Text = reader["HoursWorked"].ToString();
                        this.txtOvertimeWorked.Text = reader["OvertimeWorked"].ToString();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void taxesListBoxRefresh(SqlConnection con, DataGridView dgv)
        {
            listBox1.Items.Clear();
            if (Taxes.Count != 0)
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    con.Open();
                    for (int i = 0; i < Taxes.Count(); i++)
                    {
                        DataGridViewRow row = dgv.SelectedRows[0];
                        var id = row.Cells["ID"].Value;
                        String sqlquery = "SELECT [" + Taxes[i] + "] FROM Employee WHERE ID = " + id;
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
        }
        private void benefitsListBoxRefresh(SqlConnection con, DataGridView dgv)
        {
            listBox2.Items.Clear();
            if (Benefits.Count != 0)
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    con.Open();
                    for (int i = 0; i < Benefits.Count(); i++)
                    {
                        DataGridViewRow row = dgv.SelectedRows[0];
                        var id = row.Cells["ID"].Value;
                        String sqlquery = "SELECT [" + Benefits[i] + "] FROM Employee WHERE ID = " + id;
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
        }
        private void updateTaxes()
        {
            Taxes.Clear();
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();
            String sqlquery = "SELECT column_name " +
                "FROM information_schema.columns " +
                "WHERE table_name = 'Employee' " +
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

        public string getSelectionID()
        {
            return this.txtEmployeeID.Text;
        }

        private void btnBenefits_Click(object sender, EventArgs e)
        {
            FormBenefits popUpForm = new FormBenefits();
            popUpForm.ShowDialog();
            this.resetPlacementValues();
            this.updateBenefits();
            this.gridRefresh();
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
            searchValue = txtSearch.Text;
            string caseValue = comboBox1.Text;
            previousDisplayCounts.Clear();
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
            this.resetPlacementValues();
            this.updateTaxes();
            this.gridRefresh();
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
            gridRefresh();
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            previousButtonClicked = true;
            pageX -= 50;
            currentPage--;
            checkPreviousPage();
            gridRefresh();
        }

    
        private void btnPrintCheck_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
                SqlConnection con = new SqlConnection(connectionString); // making connection
                con.Open();

                float hoursWorked = Int32.Parse(txtHoursWorked.Text);
                float overtimeWorked = Int32.Parse(txtOvertimeWorked.Text);
                float hourlyPay = Int32.Parse(txtHourlyPay.Text);
                float grossPay = (hoursWorked * hourlyPay) + (overtimeWorked * hourlyPay);

                if (txtOvertimeWorked.Text == "")
                {
                    txtOvertimeWorked.Text = 0.ToString();
                }
                string sqlquery = "UPDATE Employee SET HoursWorked = " + txtHoursWorked.Text + " WHERE ID = " + getSelectionID();
                string sqlquery1 = "UPDATE Employee SET OvertimeWorked = " + txtOvertimeWorked.Text + " WHERE ID = " + getSelectionID();
                string sqlquery2 = "UPDATE Employee SET Hourly = " + txtHourlyPay.Text + " WHERE ID = " + getSelectionID();
                string sqlquery3 = "UPDATE Employee SET GrossPay = " + grossPay.ToString() + " WHERE ID = " + getSelectionID();

                SqlCommand command = new SqlCommand(sqlquery, con);
                SqlCommand command1 = new SqlCommand(sqlquery1, con);
                SqlCommand command2 = new SqlCommand(sqlquery2, con);
                SqlCommand command3 = new SqlCommand(sqlquery3, con);

                command.ExecuteNonQuery();
                command1.ExecuteNonQuery();
                command2.ExecuteNonQuery();
                command3.ExecuteNonQuery();

                con.Close();
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            FormCheck formCheck = new FormCheck(txtEmployeeID.Text, txtFirstName.Text, txtLastName.Text, txtAddress.Text, txtZipcode.Text, 
            txtDateOfBirth.Text);
            formCheck.ShowDialog();
        }

        private void btnDetailedInfo_Click(object sender, EventArgs e)
        {
            DetailedInfo detailedinfo = new DetailedInfo(txtEmployeeID.Text);
            detailedinfo.ShowDialog();
        }
    }
}