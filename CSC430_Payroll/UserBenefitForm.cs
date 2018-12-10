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
    public partial class UserBenefitForm : Form
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString); // making connection
        private SqlCommand command, command2;
        private SqlDataReader reader;

        public UserBenefitForm()
        {
            InitializeComponent();
            UpdateBenefits();
            comboBox1.Items.Add("Benefit");
            comboBox1.Items.Add("Plan");
            comboBox1.Items.Add("Credit/Deduction");
            comboBox1.SelectedIndex = 0;
        }

        private void UpdateBenefits()   //keeps listBox and dropdown boxes up to date
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            int size;
            String sql = "SELECT TOP 1 size = Number FROM Benefits ORDER BY Number DESC; ";

            command = new SqlCommand(sql, con);

            con.Open();
            if (command.ExecuteScalar() != null)        //Error Handling for empty table
                size = (int)command.ExecuteScalar();
            else
                size = 0;
            con.Close();

            for (int i = 1; i <= size; i++)
            {
                PrintBenefits(i);
            }
        }

        private void UpdatePlans()
        {
            listBox2.Items.Clear();
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@benefitName";
            param.Value = listBox1.SelectedItem.ToString();

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

            if (listBox1.SelectedIndex != -1)
                for (int i = 1; i <= size; i++)
                {
                    PrintPlans(i);
                }

        }

        private void PrintBenefits(int count)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@count";
            param.Value = count;

            String sql = "SELECT [Benefit Name] FROM Benefits WHERE Number = @count; ";

            String Output = "";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Output = "";
                Output = Output + reader.GetValue(0);
                listBox1.Items.Add(Output);
            }

            con.Close();
        }

        private void PrintPlans(int count)
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@count";
            param1.Value = count;
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = listBox1.SelectedItem.ToString();

            String sql = "SELECT [Plan Name] FROM BenefitPlans WHERE " +
                         "[Benefit Name] = @benefitName AND Number = @count; ";

            String Output = "";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Output = "";
                Output = Output + reader.GetValue(0);
                listBox2.Items.Add(Output);
            }

            con.Close();
        }

        /* private void PrintRates(int count)
         {
             SqlParameter param = new SqlParameter();
             param.ParameterName = "@count";
             param.Value = count;

             String sql = "SELECT Rate FROM Benefits WHERE Number = @count; ";

             String Output = "";

             command = new SqlCommand(sql, con);
             command.Parameters.Add(param);

             con.Open();
             reader = command.ExecuteReader();

             while (reader.Read())
             {
                 Output = "";
                 Output = Output + reader.GetValue(0);
                 listBox2.Items.Add(Output);
             }

             con.Close();
         }*/

        private void FormBenefits_Load(object sender, EventArgs e)
        {

        }

        private void CreateBenefit_Click(object sender, EventArgs e)   //Creates NEW Benefit
        {
            if (comboBox1.SelectedIndex == 0)
            {
                FormCreateBenefit popUpForm = new FormCreateBenefit();
                popUpForm.ShowDialog();
                UpdateBenefits();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                if (listBox1.SelectedIndex != -1)
                {
                    FormCreatePlan popUpForm = new FormCreatePlan(listBox1.SelectedIndex);
                    popUpForm.ShowDialog();
                }
                else
                {
                    FormCreatePlan popUpForm = new FormCreatePlan();
                    popUpForm.ShowDialog();
                }
                UpdateBenefits();
            }
        }

        /* private bool CheckDuplicate()       //returns true if there is a duplicate
         {
             SqlParameter param = new SqlParameter();
             param.ParameterName = "@input";
             string input = textBox1.Text;
             param.Value = input;
             string name = "failure";

             String sql = "SELECT name = [Benefit Name] FROM Benefits WHERE [Benefit Name] = @input;";

             command = new SqlCommand(sql, con);
             command.Parameters.Add(param);

             con.Open();
             name = (string)command.ExecuteScalar();
             con.Close();

             if (name == input)
                 return true;
             else
                 return false;
         }*/

        private void DeleteBenefit_Click(object sender, EventArgs e)   //Permanently Deletes a Benefit
        {
            string input = listBox1.SelectedItem.ToString();

            if (input != "")
            {
                var confirmDelete = MessageBox.Show("Are you sure you want to delete this Benefit? It will be removed from each employee that uses it.",
                "Confirm Deletion", MessageBoxButtons.YesNo);

                if (confirmDelete == DialogResult.Yes)
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@benefitName";
                    param.Value = input;

                    String sql = "DELETE FROM Benefits WHERE [Benefit Name] = @benefitName; " +
                                 "DELETE FROM BenefitPlans WHERE [Benefit Name] = @benefitName;";

                    command = new SqlCommand(sql, con);
                    command.Parameters.Add(param);

                    con.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetValue(0));
                    }

                    con.Close();
                    ResortTable();
                    //RemoveEmployeeCol(input);
                    UpdateBenefits();
                }
            }
        }

        private void ResortTable()  //makes sure there isn't a number gap after deletion
        {
            String sql = "DECLARE @rowNum INT;" +
                         "DECLARE @count INT;" +
                         "DECLARE @nextRowNum INT;" +
                         "SET @count = 1;" +

                         "DECLARE @size INT;" +
                         "SELECT TOP 1 @size = Number " +
                         "FROM Benefits " +
                         "ORDER BY Number DESC;" +

                         "WHILE(@count < @size) " +
                         "BEGIN " +
                         "SET @rowNum = -1;" +
                         "SELECT @rowNum = Number FROM Benefits WHERE Number = @count;" +

                         "IF(@rowNum = -1) " +
                         "BEGIN " +
                         "UPDATE Benefits SET Number = @count WHERE Number = (@count + 1);" +
                         "SELECT @rowNum = Number FROM Benefits WHERE Number = @count;" +
                         "PRINT @rowNum;" +
                         "END " +
                         "ELSE " +
                         "PRINT @rowNum;" +
                         "SET @count += 1;" +
                         "END";

            command = new SqlCommand(sql, con);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
        }

        private void AddEmployeeCol(string benefitName)
        {
            benefitName = "BFT: " + benefitName;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@benefitName";
            param.Value = benefitName;

            //Adds new column to Employee table
            String sql = "DECLARE @SQL NVARCHAR(1000); " +
                         "SET @SQL = '" +
                         "ALTER TABLE Employee " +
                         "ADD [' + @benefitName + '] bit; " +
                         "'; " +
                         "EXEC (@SQL);";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }
            con.Close();

            command.Parameters.Remove(param);

            //Sets entire column in Employee to 0
            sql = "DECLARE @SQL VARCHAR(1000);" +
                  "SET @SQL = '" +
                  "UPDATE Employee " +
                  "SET [' + @benefitName + '] = 0 " +
                  "';" +
                  "EXEC (@SQL);";

            command2 = new SqlCommand(sql, con);
            command2.Parameters.Add(param);

            con.Open();
            reader = command2.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePlans();
        }

        private void DeletePlan_Click(object sender, EventArgs e)
        {
            string benefitName = listBox1.SelectedItem.ToString();
            string planName = listBox2.SelectedItem.ToString();

            if (benefitName != "" && planName != "")
            {
                var confirmDelete = MessageBox.Show("Are you sure you want to delete this Benefit Plan? It will be removed from each employee that uses it.",
                "Confirm Deletion", MessageBoxButtons.YesNo);

                if (confirmDelete == DialogResult.Yes)
                {
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@benefitName";
                    param1.Value = benefitName;
                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@planName";
                    param2.Value = planName;

                    String sql = "DELETE FROM BenefitPlans WHERE [Benefit Name] = @benefitName AND [Plan Name] = @planName;";

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
                    ResortTable();
                    //RemoveEmployeeCol(benefitName);
                    UpdatePlans();
                }
            }
        }

        private void RemoveEmployeeCol(string benefitName)
        {
            benefitName = "BFT: " + benefitName;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@benefitName";
            param.Value = benefitName;

            String sql = "IF COL_LENGTH('Employee', '" + benefitName + "') IS NOT NULL " +
                         "BEGIN " +
                         "ALTER TABLE Employee " +
                         "DROP COLUMN [" + benefitName + "];" +
                         "END";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

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
