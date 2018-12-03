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
    public partial class FormCreateBenefit : Form
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString); // making connection
        private SqlCommand command;
        private SqlDataReader reader;

        public FormCreateBenefit()
        {
            InitializeComponent();
            textBox2.Text = "Standard";
            radioRate.Checked = true;
        }

        private void FormCreateBenefit_Load(object sender, EventArgs e)
        {

        }

        private void Create_Click(object sender, EventArgs e)       //either creates Plan or writes error and/or empty field messages
        {
            bool empty = CheckEmpty();
            bool error = CheckError();

            if (empty || error)          //checks for errors/empty fields after messages have been written
                MessageBox.Show("Some of the information requirements have not been met.");
            else
            {
                CreateBenefit();
                CreatePlan();
                this.Close();
            }
        }

        private bool CheckBenefitExists()
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@newBenefit";
            string newBenefit = textBox1.Text;
            param.Value = newBenefit;
            string name = "";

            String sql = "SELECT name = [Benefit Name] FROM Benefits WHERE [Benefit Name] = @newBenefit;";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            name = (string)command.ExecuteScalar();
            con.Close();

            if (name == newBenefit)
                return true;
            else
                return false;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioRate_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRate.Checked)
                textBox3.Enabled = true;
            else
                textBox3.Enabled = false;
        }

        private void radioFixed_CheckedChanged(object sender, EventArgs e)
        {
            if (radioFixed.Checked)
                textBox4.Enabled = true;
            else
                textBox4.Enabled = false;
        }

        private bool CheckEmpty()
        {
            bool empty = false;

            if (textBox1.Text == "")
            {
                benefitErrorLabel.Text = "*";
                empty = true;
            }
            else
                benefitErrorLabel.Text = "";

            if (textBox2.Text == "")
            {
                planErrorLabel.Text = "*";
                empty = true;
            }
            else
                planErrorLabel.Text = "";

            if (textBox3.Text == "" && radioRate.Checked)
            {
                rateErrorLabel.Text = "*";
                empty = true;
            }
            else
                rateErrorLabel.Text = "";

            if (textBox4.Text == "" && radioFixed.Checked)
            {
                fixedAmtErrorLabel.Text = "*";
                empty = true;
            }
            else
                fixedAmtErrorLabel.Text = "";

            return empty;
        }

        private bool CheckError()
        {
            bool error = false;
            string rate = textBox3.Text;
            string fixedAmount = textBox4.Text;
            int amtSize = fixedAmount.Length;
            int rateSize = rate.Length;
            int count = 0;
            bool dot = false;

            if (CheckBenefitExists())
            {
                benefitErrorLabel.Text = "Benefit name is already taken";
                error = true;
            }

            if ((rateSize == 1 && !char.IsDigit(textBox3.Text[0])) ||
                 (rateSize == 2 && (!char.IsDigit(textBox3.Text[0]) || !char.IsDigit(textBox3.Text[1]))))
                rateErrorLabel.Text = "Rate must be a number";          //rate check

            if (fixedAmount == ".")                         //fixed amount check
            {
                fixedAmtErrorLabel.Text = "Amount must be a number";
                error = true;
            }
            else
            {
                for (int i = 0; i < amtSize; i++)           //loop through fixed amount
                {
                    if (!char.IsDigit(textBox4.Text[i]))    //check for non digits
                    {
                        if (textBox4.Text[i] == '.' && dot == false)    //exclude first decimal, if used
                            dot = true;
                        else
                        {
                            fixedAmtErrorLabel.Text = "Amount must be a number";
                            error = true;
                            break;
                        }
                    }
                    else if (dot == true)                 //checks how many numbers after decimal, if used
                    {
                        if (count == 2)
                        {
                            fixedAmtErrorLabel.Text = "Amount exceeded two decimal places";
                            error = true;
                            break;
                        }
                        else
                            count++;
                    }
                }
            }

            return error;
        }

        private void CreateBenefit() {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@benefitName";
            param1.Value = textBox1.Text;

            String sql = "DECLARE @size INT;" +
                         "SET @size = 0;" +
                         "SELECT TOP 1 @size = Number FROM Benefits ORDER BY Number DESC;" +
                         "INSERT INTO Benefits (Number, [Benefit Name]) VALUES (@size + 1, @benefitName);";


            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
        }

        private void CreatePlan()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@planName";
            param1.Value = textBox2.Text;
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = textBox1.Text;

            String sql = "INSERT INTO BenefitPlans (Number, [Plan Name], [Benefit Name])" +
                         " VALUES (1, @planName, @benefitName);";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();

            if (radioRate.Checked)
                AddRate();

            if (radioFixed.Checked)
                AddFixedAmount();

        }

        private void AddRate()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@rate";
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = textBox1.Text;

            int rateSize = textBox3.Text.Length;

            if (rateSize == 1)
                param1.Value = ".0" + textBox3.Text;
            else
                param1.Value = "." + textBox3.Text;

            String sql = "UPDATE BenefitPlans SET Rate = @rate Where [Benefit Name] = @benefitName"; 

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
        }

        private void AddFixedAmount()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@fixedAmount";
            param1.Value = textBox4.Text;
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = textBox1.Text;

            String sql = "UPDATE BenefitPlans SET [Fixed Payment] = @fixedAmount Where [Benefit Name] = @benefitName";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
        }


    }
}
