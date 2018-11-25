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
    public partial class FormCreatePlan : Form
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString); // making connection
        private SqlCommand command;
        private SqlDataReader reader;

        public FormCreatePlan()
        {
            InitializeComponent();
            CreateBenefitList();
            textBox2.Text = "Standard";
        }

        public FormCreatePlan(int benefitNum)
        {
            InitializeComponent();
            CreateBenefitList();
            comboBox1.SelectedIndex = benefitNum;
            textBox2.Text = "Standard";
        }

        private void FormCreateBenefit_Load(object sender, EventArgs e)
        {

        }

        private void CreateBenefitList()
        {
            int size;
            String sql = "SELECT TOP 1 size = Number FROM Benefits ORDER BY Number DESC; ";

            command = new SqlCommand(sql, con);

            con.Open();
            if (command.ExecuteScalar() != null)        //Error Handling for empty table
                size = (int)command.ExecuteScalar();
            else
                size = 0;
            con.Close();

            for (int i = 0; i <= size; i++)
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@count";
                param.Value = i;

                sql = "SELECT [Benefit Name] FROM Benefits WHERE Number = @count; ";

                String Output = "";

                command = new SqlCommand(sql, con);
                command.Parameters.Add(param);

                con.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Output = "";
                    Output = Output + reader.GetValue(0);
                    comboBox1.Items.Add(Output);
                }

                con.Close();
            }
        }

        private void Create_Click(object sender, EventArgs e)   //either creates Plan or writes error and/or empty field messages
        {                               
            bool empty = CheckEmpty();
            bool error = CheckError();

            if (empty || error)         //checks for errors/empty fields after messages have been written
                MessageBox.Show("Some of the information requirements have not been met.");
            else
            {
                CreatePlan();
                this.Close();
            }
        }

        private bool CheckPlanExists()
        {
            if (comboBox1.SelectedIndex != -1)
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@benefitname";
                param.Value = comboBox1.SelectedItem.ToString();
                string name = "";

                String sql = "SELECT name = [Plan Name] FROM BenefitPlans WHERE [Benefit Name] = @benefitName; ";

                command = new SqlCommand(sql, con);
                command.Parameters.Add(param);

                con.Open();
                name = (string)command.ExecuteScalar();
                con.Close();

                if (name == textBox2.Text)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBoxRate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxRate.Checked)
                textBox3.Enabled = true;
            else
                textBox3.Enabled = false;
        }

        private void checkBoxFixed_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFixed.Checked)
                textBox4.Enabled = true;
            else
                textBox4.Enabled = false;
        }

        private bool CheckEmpty()
        {
            bool empty = false;

            if (comboBox1.SelectedIndex == -1)
            {
                benefitErrorLabel.Text = "Please select a Benefit.";
                empty = true;
            }
            else
                benefitErrorLabel.Text = "";

            if (textBox2.Text == "")
            {
                planErrorLabel.Text = "Please enter a Benefit Plan name";
                empty = true;
            }
            else
                planErrorLabel.Text = "";


            if (!checkBoxRate.Checked && !checkBoxFixed.Checked)
            {
                payTypeErrorLabel.Text = "Please select at least one Payment Type";
                empty = true;
            }
            else
                payTypeErrorLabel.Text = "";

            if (textBox3.Text == "" && checkBoxRate.Checked)
            {
                rateErrorLabel.Text = "Please enter a Rate";
                empty = true;
            }
            else
                rateErrorLabel.Text = "";

            if (textBox4.Text == "" && checkBoxFixed.Checked)
            {
                fixedAmtErrorLabel.Text = "Please enter a Fixed Payment Amount";
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

            if (CheckPlanExists())
            {
                planErrorLabel.Text = "Plan name is already taken";
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

        private void CreatePlan()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@planName";
            param1.Value = textBox2.Text;
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = comboBox1.SelectedItem.ToString();

            String sql = "DECLARE @num int; " +
                         "SET @num = 0; " +
                         "SELECT TOP 1 @num = Number FROM BenefitPlans WHERE [Benefit Name] = @benefitName ORDER BY Number DESC; " +
                         "INSERT INTO BenefitPlans (Number, [Plan Name], [Benefit Name])" +
                         " VALUES (@num + 1, @planName, @benefitName);";

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

            if (checkBoxRate.Checked)
                AddRate();

            if (checkBoxFixed.Checked)
                AddFixedAmount();

            //AddEmployeeCol(comboBox1.SelectedItem.ToString());
        }

        private void AddRate()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@rate";
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = comboBox1.SelectedItem.ToString();
            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@planName";
            param3.Value = textBox2.Text;

            int rateSize = textBox3.Text.Length;

            if (rateSize == 1)
                param1.Value = ".0" + textBox3.Text;
            else
                param1.Value = "." + textBox3.Text;

            String sql = "UPDATE BenefitPlans SET Rate = @rate Where [Benefit Name] = @benefitName AND [Plan Name] = @planName";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.Parameters.Add(param3);

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
            param2.Value = comboBox1.SelectedItem.ToString();
            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@planName";
            param3.Value = textBox2.Text;

            String sql = "UPDATE BenefitPlans SET [Fixed Payment] = @fixedAmount Where [Benefit Name] = @benefitName AND [Plan Name] = @planName";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.Parameters.Add(param3);

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
