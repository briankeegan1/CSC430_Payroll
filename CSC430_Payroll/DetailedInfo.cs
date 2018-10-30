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
    public partial class DetailedInfo : Form
    {
        public string numID = "";
        public DetailedInfo(string employeeID)
        {
            InitializeComponent();

            numID = employeeID;
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection  
            con.Open();
            string sqlquery = "SELECT Salary, Hourly, Tax, Overtime, Deductions, GrossPay, NetPay FROM Employee WHERE ID = " + numID;
            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    this.displaySal.Text = reader["Salary"].ToString();
                    this.displayHrly.Text = reader["Hourly"].ToString();
                    this.displayOvr.Text = reader["Overtime"].ToString();
                    this.displayHoursWorked.Text = reader["Overtime"].ToString();
                    this.displayGross.Text = reader["GrossPay"].ToString();
                    this.displayTax.Text = reader["Tax"].ToString();
                    this.displayDeductions.Text = reader["Deductions"].ToString();
                    this.displayNetpay.Text = reader["NetPay"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

         

    }
}
