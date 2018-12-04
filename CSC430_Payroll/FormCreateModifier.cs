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
    public partial class FormCreateModifier : Form
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString); // making connection
        private SqlCommand command;
        private SqlDataReader reader;

        public FormCreateModifier()
        {
            InitializeComponent();
            CreateBenefitsList();
            CreatePlansList();
            radioCredit.Checked = true;
        }

        public FormCreateModifier(string benefitName)
        {
            InitializeComponent();
            CreateBenefitsList();
            comboBox1.SelectedItem = benefitName;
            radioCredit.Checked = true;
        }

        public FormCreateModifier(string benefitName, string planName)
        {
            InitializeComponent();
            CreateBenefitsList();
            comboBox1.SelectedItem = benefitName;
            CreatePlansList();
            comboBox2.SelectedItem = planName;
            radioCredit.Checked = true;
        }

        private int GetBenefitsSize()
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

            return size;
        }

        private void CreateBenefitsList()
        {
            int size = GetBenefitsSize();
            string benefitName = "";

            for (int i = 1; i <= size; i++)
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@benefitNum";
                param.Value = i;

                String sql = "SELECT benefitName = [Benefit Name] FROM Benefits WHERE Number = @benefitNum;";

                command = new SqlCommand(sql, con);
                command.Parameters.Add(param);

                con.Open();
                benefitName = (string)command.ExecuteScalar();
                con.Close();

                comboBox1.Items.Add(benefitName);
            }
        }

        private int GetPlansSize()
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@benefitName";
            param.Value = comboBox1.SelectedItem.ToString();
            int size;
            String sql = "SELECT TOP 1 size = Number FROM BenefitPlans WHERE [Benefit Name] = @benefitName ORDER BY Number DESC; ";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            if (command.ExecuteScalar() != null)        //Error Handling for empty table
                size = (int)command.ExecuteScalar();
            else
                size = 0;
            con.Close();
            return size;
        }

        private void CreatePlansList()
        {
            if (comboBox1.SelectedIndex != -1)
            {
                comboBox2.Enabled = true;
                comboBox2.Items.Clear();
                int size = GetPlansSize();
                string planName = "";

                for (int i = 1; i <= size; i++)
                {
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@num";
                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@benefitName";
                    param2.Value = comboBox1.SelectedItem.ToString();
                    param1.Value = i;

                    String sql = "SELECT planName = [Plan Name] FROM BenefitPlans WHERE Number = @num AND [Benefit Name] = @benefitName;";

                    command = new SqlCommand(sql, con);
                    command.Parameters.Add(param1);
                    command.Parameters.Add(param2);

                    con.Open();
                    planName = (string)command.ExecuteScalar();
                    con.Close();

                    comboBox2.Items.Add(planName);
                }
            }
            else
                comboBox2.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           CreatePlansList();
           comboBox2.SelectedIndex = 0;
        }

        private bool CheckExists()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@newName";
            param1.Value = textBox1.Text;
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = comboBox1.SelectedItem.ToString();
            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@planName";
            param3.Value = comboBox2.SelectedItem.ToString();
            string name = "";

            String sql = "SELECT name = [Name] FROM [Credits/Deductions] WHERE [Benefit Name] = @benefitName AND " +
                         "[Plan Name] = @planName AND [Name] = @newName;";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.Parameters.Add(param3);

            con.Open();
            name = (string)command.ExecuteScalar();
            con.Close();

            if (name == textBox1.Text)
                return true;
            else
                return false;
        }

        private void Create_Click(object sender, EventArgs e)
        {
            bool empty = CheckEmpty();
            bool error = CheckError();

            if (empty || error)             //checks for errors/empty fields after messages have been written
                MessageBox.Show("Some of the information requirements have not been met.");
            else
            {
                CreateModifier();
                this.Close();
            }
        }

        private bool CheckEmpty()
        {
            bool empty = false;
            if (comboBox1.SelectedIndex == -1)
            {
                benefitErrorLabel.Text = "*";
                empty = true;
            }
            else
                benefitErrorLabel.Text = "";

            if (comboBox2.SelectedIndex == -1)
            {
                planErrorLabel.Text = "*";
                empty = true;
            }
            else
                planErrorLabel.Text = "";

            if (!radioCredit.Checked && !radioDeduction.Checked)
            {
                radioErrorLabel.Text = "*";
                empty = true;
            }
            else
                radioErrorLabel.Text = "";

            if (textBox1.Text == "")
            {
                nameErrorLabel.Text = "*";
                empty = true;
            }
            else
                nameErrorLabel.Text = "";

            if (textBox2.Text == "")
            {
                amountErrorLabel.Text = "*";
                empty = true;
            }
            else
                amountErrorLabel.Text = "";

            return empty;
        }

        private bool CheckError()
        {
            bool error = false;
            bool dot = false;
            int count = 0;
            int amountSize = textBox2.Text.Length;

            if (comboBox2.SelectedIndex != -1)
            {
                if (CheckExists())
                {
                    error = true;
                    nameErrorLabel.Text = "Name already Exists";
                }
            }

            for (int i = 0; i < amountSize; i++)
            {
                if (textBox2.Text[i] == '.')    //if decimal is typed
                {
                    if (dot == false)   //first decimal
                        dot = true;
                    else                //second decimal
                    {
                        amountErrorLabel.Text = "Please enter a number";
                        error = true;
                        break;
                    }
                }
                else if (!char.IsDigit(textBox2.Text[i]))    //if non digit is typed
                {
                    amountErrorLabel.Text = "Please enter a number";
                    error = true;
                    break;
                }
                else if (dot == true)       
                {
                    if (count < 2)
                        count++;
                    else                    //if exceeds two decimal places
                    {
                        amountErrorLabel.Text = "Amount exceed 2 decimal places";
                        error = true;
                        break;
                    }
                }
            }

            return error;
        }

        private void CreateModifier()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@planName";
            param1.Value = comboBox2.SelectedItem.ToString();
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = comboBox1.SelectedItem.ToString();
            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@name";
            param3.Value = textBox1.Text;
            SqlParameter param4 = new SqlParameter();
            param4.ParameterName = "@amount";

            if (radioDeduction.Checked)
                param4.Value = "-" + textBox2.Text;
            else
                param4.Value = textBox2.Text;

            String sql = "DECLARE @num int; " +
                         "SET @num = 0; " + 
                         "SELECT TOP 1 @num = Number FROM [Credits/Deductions] " +
                         "WHERE [Benefit Name] = @benefitName AND [Plan Name] = @planName ORDER BY Number DESC; " +
                         "INSERT INTO [Credits/Deductions] (Number, Name, Amount, [Plan Name], [Benefit Name])" +
                         " VALUES (@num + 1, @name, @amount, @planName, @benefitName);";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.Parameters.Add(param3);
            command.Parameters.Add(param4);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
        }

        private void radioCredit_CheckedChanged(object sender, EventArgs e)
        {
            if (radioCredit.Checked)
                label4.Text = "Amount (+)";
            else
                label4.Text = "Amount (-)";
        }

    }
}
