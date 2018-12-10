using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSC430_Payroll
{
    public partial class UserPayStub : Form
    {
        public string numID = "";

        UserMain form2 = Application.OpenForms.OfType<UserMain>().Single();
        private List<string> Benefits = new List<string>();
        private List<string> Taxes = new List<string>();
        private List<string> TaxRates = new List<string>();
        private List<string> BenefitRates = new List<string>();

        public UserPayStub()
        {
            InitializeComponent();

        }
        public UserPayStub(string employeeID, string text1, string text2, string text3, string text4, string text5,
            decimal taxRate, decimal benefitRate, List<string> taxRates, List<string> benefitRates)
        {
            Taxes = form2.getTaxes();
            Benefits = form2.getBenefits();
            TaxRates = taxRates;
            BenefitRates = benefitRates;
            //Note: weird format when displaying employee try to fix that 
            numID = employeeID;
            InitializeComponent();
            displayFname.Text = text1;
            displayLname.Text = text2;
            displayAddress.Text = text3;
            displayZIP.Text = text4;
            displayDOB.Text = text5;
            displayTaxes.Text = string.Join(Environment.NewLine, Taxes);
            displayRates.Text = string.Join(Environment.NewLine, TaxRates);




            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection  

            con.Open();
            string sqlquery = "SELECT Salary, HoursWorked, Hourly, Tax, OvertimeWorked, Deductions, GrossPay, NetPay FROM Employee WHERE ID = " + numID;


            SqlCommand command = new SqlCommand(sqlquery, con);

            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {

                    this.displayHourlyAmt.Text = reader["Hourly"].ToString();
                    this.displayHrsWorked.Text = reader["HoursWorked"].ToString();
                    this.displayHrAmt.Text = (Convert.ToInt32(displayHourlyAmt.Text) * Convert.ToInt32(displayHrsWorked.Text)).ToString();
                    this.displayOvrHrs.Text = reader["OvertimeWorked"].ToString();
                    this.displayOverAmt.Text = (Convert.ToInt32(displayHourlyAmt.Text) * Convert.ToInt32(displayOvrHrs.Text)).ToString();
                    this.displayGrossPay.Text = "$" + reader["GrossPay"].ToString();
                    this.displayNetPay.Text = "$" + reader["NetPay"].ToString();

                }

                /*using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT [Tax Name] FROM Taxes ", connection))
                    {
                        using (SqlDataReader reader2 = cmd.ExecuteReader())
                        {
                            if (reader2 != null)
                            {
                                while (reader2.Read())
                                {    

                                }
                            }
                        }
                    }
                }*/
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*public string displayMembers(List<String> TaxRates)
        {
            foreach (String s in TaxRates)
            {
                displayRate1.Text += s.ToString() + "\r\n";
            }
            return displayRate1.Text;
        }*/


        /*private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            e.Graphics.DrawImage(bmp, 0, 0);

        }

        Bitmap bmp;
        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            bmp = new Bitmap(this.Size.Width, this.Size.Height, g);
            Graphics mg = Graphics.FromImage(bmp);
            mg.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, this.Size);
            printPreviewDialog1.ShowDialog();


        }*/

        
    }
}
