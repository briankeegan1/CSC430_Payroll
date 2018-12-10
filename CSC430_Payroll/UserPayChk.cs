using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSC430_Payroll
{
    public partial class UserPayChk : Form
    {
        UserMain form2 = Application.OpenForms.OfType<UserMain>().FirstOrDefault();

        public UserPayChk(string Fname, string Lname)
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config


            //Create your private font collection object.
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(@"C:\Users\Tonyyy\Desktop\BankFont.ttf");
            displayName.Text = Fname + Lname;
            displayRouting.Font = new Font(pfc.Families[0], displayAcc.Font.Size);
            displayRouting.Text = "123456789";
            
            displayDate.Text = DateTime.Now.ToString("M/d/yyyy");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var id = form2.getSelectionID();
                using (SqlCommand cmd = new SqlCommand("SELECT AccountNo, CheckNo, NetPay FROM Employee WHERE ID =" + id, connection))
                {
                    using (SqlDataReader reader2 = cmd.ExecuteReader())
                    {
                        if (reader2 != null)
                        {
                            while (reader2.Read())
                            {

                                displayAcc.Font = new Font(pfc.Families[0], displayAcc.Font.Size);
                                this.displayAcc.Text = reader2["AccountNo"].ToString();
                                displayCheckN.Font = new Font(pfc.Families[0], displayCheckN.Font.Size);
                                this.displayCheckN.Text = "0" + reader2["CheckNo"].ToString();
                                this.checkNum.Text = reader2["CheckNo"].ToString();
                                this.displayAmt.Text = reader2["NetPay"].ToString();
                                var words = Convert.ToDouble(this.displayAmt.Text);
                                this.displayAmt.Text = NumberToWords(words);

                            }
                        }

                    }
                }
            }
        }

        //algorithm to conver amount payed to words (found it online)
        public static string NumberToWords(double doubleNumber)
        {
            var beforeFloatingPoint = (int)Math.Floor(doubleNumber);
            var beforeFloatingPointWord = $"{NumberToWords(beforeFloatingPoint)} dollars";
            var afterFloatingPointWord =
                $"{SmallNumberToWord((int)((doubleNumber - beforeFloatingPoint) * 100), "")} cents";
            return $"{beforeFloatingPointWord} and {afterFloatingPointWord}";
        }

        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            var words = "";

            if (number / 1000000000 > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if (number / 1000000 > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if (number / 1000 > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            words = SmallNumberToWord(number, words);

            return words;
        }

        private static string SmallNumberToWord(int number, string words)
        {
            if (number <= 0) return words;
            if (words != "")
                words += " ";

            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
            return words;
        }

        
    }

}
