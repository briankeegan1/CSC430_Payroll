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
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection   
            SqlDataAdapter sda = new SqlDataAdapter("SELECT ID, [Last Name], [First Name] FROM Employee", con);
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
            var confirmResult = MessageBox.Show("Are you sure you want to delete this Employee's records?",
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
            FormAddEmployee popUpForm = new FormAddEmployee();
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
                    string sqlquery = "SELECT DOB, Address, ZIP FROM Employee WHERE ID = " + numID;
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
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}
