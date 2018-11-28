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
    public partial class FormBenefits : Form
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString); // making connection
        private SqlCommand command, command2;
        private SqlDataReader reader;
        private string curBenefitName, curPlanName, curModName;

        public FormBenefits()
        {
            InitializeComponent();
            UpdateBenefits();
            benefitErrorLabel.Text = "";
            planErrorLabel.Text = "";
            rateErrorLabel.Text = "";
            fixedErrorLabel.Text = "";
            modifierErrorLabel.Text = "";
            modAmtErrorLabel.Text = "";
        }

        private void UpdateBenefits()   //keeps Benefits up to date
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

        private void UpdatePlans()      //keeps Plans up to date
        {
            listBox2.Items.Clear();
            if (listBox1.SelectedIndex != -1)
            {
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
        }

        private void UpdateModifiers()
        {
                comboBox3.Items.Clear();
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@planName";
                param1.Value = listBox2.SelectedItem.ToString();
                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@benefitName";
                param2.Value = listBox1.SelectedItem.ToString();

                int size;
                String sql = "SELECT TOP 1 size = Number FROM [Credits/Deductions] " +
                             "WHERE [Benefit Name] = @benefitName AND [Plan Name] = @planName ORDER BY Number DESC; ";

                command = new SqlCommand(sql, con);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);

                con.Open();
                if (command.ExecuteScalar() != null)        //Error Handling for empty table
                    size = (int)command.ExecuteScalar();
                else
                    size = 0;
                con.Close();

                for (int i = 1; i <= size; i++)
                {
                    PrintModifiers(i);
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

        private void PrintModifiers(int count)
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@planName";
            param1.Value = listBox2.SelectedItem.ToString();
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = listBox1.SelectedItem.ToString();
            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@count";
            param3.Value = count;

            String sql = "SELECT [Name] FROM [Credits/Deductions] WHERE " +
                         "[Benefit Name] = @benefitName AND [Plan Name] = @planName AND Number = @count; ";

            String Output = "";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.Parameters.Add(param3);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Output = "";
                Output = Output + reader.GetValue(0);
                comboBox3.Items.Add(Output);
            }

            con.Close();
        }

        private void FormBenefits_Load(object sender, EventArgs e)
        {
            
        }
        
        private void CreateBenefit_Click(object sender, EventArgs e)   //Creates NEW Benefit
        {
                FormCreateBenefit popUpForm = new FormCreateBenefit();
                popUpForm.ShowDialog();
                UpdateBenefits();
        }

        private void CreatePlan_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)        //if there are no benefits, don't allow plan creation
            {
                MessageBox.Show("Please create a Benefit first", "Error");
            }
            else     //create plans
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
                UpdatePlans();
            }
        }

        private void CreateModifier_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)        //if there are no benefits, don't allow plan creation
            {
                MessageBox.Show("Please create a Benefit first", "Error");
            }
            else        //create modifiers
            {
                if (listBox1.SelectedIndex != -1)
                {
                    if (listBox2.SelectedIndex != -1)   //if both benefit and plan are selected
                    {
                        FormCreateModifier popupForm = new FormCreateModifier(listBox1.SelectedItem.ToString(), listBox2.SelectedItem.ToString());
                        popupForm.ShowDialog();
                    }
                    else                                //if only benefit is selected
                    {
                        FormCreateModifier popupForm = new FormCreateModifier(listBox1.SelectedItem.ToString());
                        popupForm.ShowDialog();
                    }
                }
                else                                    //if nothing is selected
                {
                    FormCreateModifier popupForm = new FormCreateModifier();
                    popupForm.ShowDialog();
                }
                printInfo();
            }
        }    

        private void DeleteBenefit_Click(object sender, EventArgs e)   //Permanently Deletes a Benefit
        {   
            if (listBox1.SelectedIndex != -1)
            {
                string input = listBox1.SelectedItem.ToString();
                var confirmDelete = MessageBox.Show("Are you sure you want to delete this Benefit? It will be removed from each employee that uses it.",
                "Confirm Deletion", MessageBoxButtons.YesNo);

                if (confirmDelete == DialogResult.Yes)
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@benefitName";
                    param.Value = input;

                    String sql = "DELETE FROM Benefits WHERE [Benefit Name] = @benefitName; " +
                                 "DELETE FROM BenefitPlans WHERE [Benefit Name] = @benefitName;" +
                                 "DELETE FROM [Credits/Deductions] WHERE [Benefit Name] = @benefitName;";

                    command = new SqlCommand(sql, con);
                    command.Parameters.Add(param);

                    con.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetValue(0));
                    }

                    con.Close();
                    ResortBenefitsTable();
                    //RemoveEmployeeCol(input);
                    UpdateBenefits();
                    printInfo();
                }
            }
        }

        private void ResortBenefitsTable()  //makes sure there isn't a number gap after deletion
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
                         "END " +
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

        private void ResortPlansTable()  //makes sure there isn't a number gap after deletion
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@benefitName";
            param.Value = listBox1.SelectedItem.ToString();

            String sql = "DECLARE @rowNum INT;" +
                         "DECLARE @count INT;" +
                         "DECLARE @nextRowNum INT;" +
                         "SET @count = 1;" +

                         "DECLARE @size INT;" +
                         "SELECT TOP 1 @size = Number " +
                         "FROM BenefitPlans WHERE [Benefit Name] = @benefitName " +
                         "ORDER BY Number DESC;" +

                         "WHILE(@count < @size) " +
                         "BEGIN " +
                         "SET @rowNum = -1;" +
                         "SELECT @rowNum = Number FROM BenefitPlans WHERE Number = @count AND [Benefit Name] = @benefitName;" +

                         "IF(@rowNum = -1) " +
                         "BEGIN " +
                         "UPDATE BenefitPlans SET Number = @count WHERE Number = (@count + 1) AND [Benefit Name] = @benefitName;" +
                         "END " +
                         "SET @count += 1;" +
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

        private void ResortModifiersTable()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@planName";
            param1.Value = listBox2.SelectedItem.ToString();
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = listBox1.SelectedItem.ToString();

            String sql = "DECLARE @rowNum INT;" +
                         "DECLARE @count INT;" +
                         "DECLARE @nextRowNum INT;" +
                         "SET @count = 1;" +

                         "DECLARE @size INT;" +
                         "SELECT TOP 1 @size = Number " +
                         "FROM [Credits/Deductions] WHERE [Plan Name] = @planName AND [Benefit Name] = @benefitName " +
                         "ORDER BY Number DESC;" +

                         "WHILE(@count < @size) " +
                         "BEGIN " +
                         "SET @rowNum = -1;" +
                         "SELECT @rowNum = Number FROM [Credits/Deductions] WHERE Number = @count AND [Plan Name] = @planName AND [Benefit Name] = @benefitName;" +

                         "IF(@rowNum = -1) " +
                         "BEGIN " +
                         "UPDATE [Credits/Deductions] SET Number = @count WHERE Number = (@count + 1) AND [Plan Name] = @planName AND [Benefit Name] = @benefitName;" +
                         "END " +
                         "SET @count += 1;" +
                         "END";

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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePlans();
            printInfo();
            DeletePlan.Enabled = false;
            if (listBox1.SelectedIndex != -1 && listBox1.Items.Count != 0)
            {
                DeleteBenefit.Enabled = true;
                ModifyInfo.Enabled = true;
            }
            else
            {
                DeleteBenefit.Enabled = false;
                ModifyInfo.Enabled = false;
            }

            benefitErrorLabel.Text = "";
            planErrorLabel.Text = "";
            rateErrorLabel.Text = "";
            fixedErrorLabel.Text = "";
            modifierErrorLabel.Text = "";
            modAmtErrorLabel.Text = "";
        }

        private void DeletePlan_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)       //if plan is selected
            {
                if (listBox2.Items.Count == 1)      //if only 1 plan
                {
                    MessageBox.Show("Each Benefit must have at least one plan. Please create another plan before deleting this one or delete the Benefit itself.",
                        "Error");
                }
                else
                {
                    string benefitName = listBox1.SelectedItem.ToString();
                    string planName = listBox2.SelectedItem.ToString();
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
                        ResortPlansTable();
                        //RemoveEmployeeCol(benefitName);
                        UpdatePlans();
                        printInfo();
                    }
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

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            printInfo();

            if (listBox2.SelectedIndex != -1 && listBox2.Items.Count != 0)
                DeletePlan.Enabled = true;
            else
                DeletePlan.Enabled = false;

            benefitErrorLabel.Text = "";
            planErrorLabel.Text = "";
            rateErrorLabel.Text = "";
            fixedErrorLabel.Text = "";
            modifierErrorLabel.Text = "";
            modAmtErrorLabel.Text = "";
        }

        private void printInfo()
        {
            if (listBox1.SelectedIndex != -1)           //if benefit is selected
            {
                benefitTextBox.Enabled = true;                           //print benefit name
                benefitTextBox.Text = listBox1.SelectedItem.ToString();
                curBenefitName = listBox1.SelectedItem.ToString();

                if (listBox2.SelectedIndex != -1)           //if plan is selected 
                {
                    planTextBox.Enabled = true;                             //print plan info
                    planTextBox.Text = listBox2.SelectedItem.ToString();
                    curPlanName = listBox2.SelectedItem.ToString();
                    rateTextBox.Enabled = true;
                    fixedTextBox.Enabled = true;
                    printRateAndFixed();

                    UpdateModifiers();
                    if (comboBox3.Items.Count != 0)     //if plan has modifiers
                    {
                        comboBox3.Enabled = true;       //print modifier info
                        comboBox3.SelectedIndex = 0;
                        modifierTextBox.Enabled = true;
                        modifierTextBox.Text = comboBox3.SelectedItem.ToString();
                        curModName = comboBox3.SelectedItem.ToString();
                        modAmtTextBox.Enabled = true;
                        DeleteModifier.Enabled = true;
                        printmodifierAmt();
                    }
                }
            }

            if (comboBox3.Items.Count == 0 || listBox2.SelectedIndex == -1)//if plan has no modifiers
            {
                comboBox3.Enabled = false;          //disable and clear info
                modifierTextBox.Enabled = false;
                modAmtTextBox.Enabled = false;
                DeleteModifier.Enabled = false;
                comboBox3.Text = null;
                modifierTextBox.Text = null;
                modAmtTextBox.Text = null;
            }
            if (listBox2.SelectedIndex == -1)   //if no plan selected
            {
                planTextBox.Enabled = false;    //disable and clear info
                rateTextBox.Enabled = false;
                fixedTextBox.Enabled = false;
                planTextBox.Text = null;
                rateTextBox.Text = null;
                fixedTextBox.Text = null;
            }
            if (listBox1.SelectedIndex == -1)   //if no benefit selected
            {
                benefitTextBox.Enabled = false; //disable and clear info
                benefitTextBox.Text = null;
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != -1)
            {
                modifierTextBox.Text = comboBox3.SelectedItem.ToString();
                printmodifierAmt();
            }
        }

        private void printmodifierAmt()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@planName";
            param1.Value = listBox2.SelectedItem.ToString();
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = listBox1.SelectedItem.ToString();
            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@modName";
            param3.Value = comboBox3.SelectedItem.ToString();

            String sql = "SELECT amt = Amount FROM [Credits/Deductions] WHERE " +
                         "[Benefit Name] = @benefitName AND [Plan Name] = @planName AND Name = @modName; ";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.Parameters.Add(param3);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                modAmtTextBox.Text = reader["amt"].ToString();
            }

            con.Close();
        }

        private void printRateAndFixed()
        {
            string rate = "", fixedAmt = "";
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@benefitName";
            param1.Value = listBox1.SelectedItem.ToString();
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@planName";
            param2.Value = listBox2.SelectedItem.ToString();

            //Adds new column to Employee table
            String sql = "SELECT rate = Rate, fixedAmt = [Fixed Payment] FROM BenefitPlans " +
                         "WHERE [Benefit Name] = @benefitName AND [Plan Name] = @planName;";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                rate = reader["rate"].ToString();
                if (rate != "")
                    rate = rate.Remove(0, 2);
                fixedAmt = reader["fixedAmt"].ToString();
            }
            con.Close();

            rateTextBox.Text = null;
            fixedTextBox.Text = null;
            rateTextBox.Text = rate;
            fixedTextBox.Text = fixedAmt;       
        }

        private void ModifyInfo_Click(object sender, EventArgs e)
        {
            bool planSelected = false, modSelected = false, empty = false, error = false ;

            benefitErrorLabel.Text = "";
            planErrorLabel.Text = "";
            rateErrorLabel.Text = "";
            fixedErrorLabel.Text = "";
            modifierErrorLabel.Text = "";
            modAmtErrorLabel.Text = "";

            if (listBox2.SelectedIndex != -1)
                planSelected = true;

            if (comboBox3.Enabled == true)
                modSelected = true;

            empty = CheckInfoEmpty();
            error = CheckInfoErrors();

            if (!empty && !error)
            {
                ModifyBenefitName();             //modify benefit
                if (planSelected)
                    ModifyPlanInfo();               //modify plan
                if (modSelected)
                    ModifyMods();     //modify credits/deductions
                UpdateBenefits();
                printInfo();
            }
        }

        private void ModifyBenefitName()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@oldName";
            param1.Value = curBenefitName;
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@newName";
            param2.Value = benefitTextBox.Text;

            String sql = "UPDATE Benefits SET [Benefit Name] = @newName WHERE [Benefit Name] = @oldName; " +
                         "UPDATE BenefitPlans SET [Benefit Name] = @newName WHERE [Benefit Name] = @oldName; " +
                         "UPDATE [Credits/Deductions] SET [Benefit Name] = @newName WHERE [Benefit Name] = @oldName; ";

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
            //UpdateBenefits();
        }

        private void DeleteModifier_Click(object sender, EventArgs e)
        {
            var confirmDelete = MessageBox.Show("Are you sure you want to delete this Credit/Deduction? It will be removed from each employee that uses it.",
            "Confirm Deletion", MessageBoxButtons.YesNo);

            if (confirmDelete == DialogResult.Yes)
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@name";
                param.Value = comboBox3.SelectedItem.ToString();

                String sql = "DELETE FROM [Credits/Deductions] WHERE [Name] = @name;";

                command = new SqlCommand(sql, con);
                command.Parameters.Add(param);

                con.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader.GetValue(0));
                }

                con.Close();
                ResortModifiersTable();
                printInfo();
            }
        }

        private void ModifyPlanInfo()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@oldName";
            param1.Value = curPlanName;
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@newName";
            param2.Value = planTextBox.Text;
            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@rate";
            SqlParameter param4 = new SqlParameter();
            param4.ParameterName = "@fixedAmt";
            param4.Value = fixedTextBox.Text;
            SqlParameter param5 = new SqlParameter();
            param5.ParameterName = "@benefitName";
            param5.Value = benefitTextBox.Text;

            int rateSize = rateTextBox.Text.Length;
            string rateSQL = "Rate = @rate, ", fixedSQL = "[Fixed Payment] = @fixedAmt ";

            if (rateSize == 1)
                param3.Value = ".0" + rateTextBox.Text;
            else if (rateSize == 2)
                param3.Value = "." + rateTextBox.Text;
            else if (rateSize == 0)
                rateSQL = "Rate = NULL, ";

            if (fixedTextBox.Text == "")
                fixedSQL = "[Fixed Payment] = NULL ";

            String sql = "UPDATE BenefitPlans " +
                         "SET [Plan Name] = @newName, " + rateSQL + fixedSQL +
                         "WHERE [Plan Name] = @oldName AND [Benefit Name] = @benefitName; " +
                         "UPDATE [Credits/Deductions] SET [Plan Name] = @newName WHERE [Plan Name] = @oldName " +
                         "AND [Benefit Name] = @benefitName; ";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            if (rateSize != 0)
                command.Parameters.Add(param3);
            if (fixedTextBox.Text != "")
                command.Parameters.Add(param4);
            command.Parameters.Add(param5);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
            //UpdatePlans();
            //printInfo();
        }

        private void ModifyMods()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@oldName";
            param1.Value = curModName;
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@newName";
            param2.Value = modifierTextBox.Text;
            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@AMT";
            param3.Value = modAmtTextBox.Text;
            SqlParameter param4 = new SqlParameter();
            param4.ParameterName = "@planName";
            param4.Value = planTextBox.Text;
            SqlParameter param5 = new SqlParameter();
            param5.ParameterName = "@benefitName";
            param5.Value = benefitTextBox.Text;

            String sql = "UPDATE [Credits/Deductions] " +
                         "SET [Name] = @newName, [Amount] = @AMT " +
                         "WHERE [Name] = @oldName AND [Plan Name] = @planName AND [Benefit Name] = @benefitName; ";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.Parameters.Add(param3);
            command.Parameters.Add(param4);
            command.Parameters.Add(param5);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
            printInfo();
        }

        private bool CheckInfoEmpty()
        {
            bool empty = false;

            int rateSize = rateTextBox.Text.Length;
            int amtSize = fixedTextBox.Text.Length;
            bool dot = false;
            int count = 0;

            if (benefitTextBox.Text != curBenefitName)  //if benefit name was edited
            {
                if (benefitTextBox.Text == "")          //if benefit name is empty
                {
                    benefitErrorLabel.Text = "*";
                    empty = true;
                }
            }

            if (listBox2.SelectedIndex != -1)       //if plan is selected
            {
                if (planTextBox.Text != curPlanName)        //if plan name was edited
                {
                    if (planTextBox.Text == "")             //if plan name is empty
                    {
                        planErrorLabel.Text = "*";
                        empty = true;
                    }
                }
                else
                    planErrorLabel.Text = "";

                if (rateTextBox.Text == "" && fixedTextBox.Text == "")   //if rate and fixed are empty
                {
                    rateErrorLabel.Text = "Please enter a Rate and/or Fixed Amount";
                    empty = true;
                }                  

                if (comboBox3.Enabled == true)      //if plan has credits/deductions
                {
                    if (modifierTextBox.Text != curModName)     //if mod name was edited
                    {
                        if (modifierTextBox.Text == "")         //if mod name is empty
                        {
                            modifierErrorLabel.Text = "*";
                            empty = true;
                        }
                    }

                    if (modAmtTextBox.Text == "")               //if mod amount is empty
                    {
                        modAmtErrorLabel.Text = "*";
                        empty = true;
                    }
                }
            }

            return empty;
        }

        private bool CheckInfoErrors() {
            bool error = false;
            int rateSize = rateTextBox.Text.Length;
            bool dot = false;
            int count = 0;

            if (benefitTextBox.Text != curBenefitName)  //if benefit name was edited
            {
                if (CheckBenefitExists())          //if benefit name exists
                {
                    benefitErrorLabel.Text = "Benefit name is already taken";
                    error = true;
                }
            }

            if (listBox2.SelectedIndex != -1)       //if plan is selected
            {
                if (planTextBox.Text != curPlanName)        //if plan name was edited
                {
                    if (CheckPlanExists())             //if plan name exists
                    {
                        planErrorLabel.Text = "Plan Name already Exists";
                        error = true;
                    }
                }

                if ((rateSize == 1 && !char.IsDigit(rateTextBox.Text[0])) ||
                    (rateSize == 2 && (!char.IsDigit(rateTextBox.Text[0]) || !char.IsDigit(rateTextBox.Text[1]))))
                    rateErrorLabel.Text = "Rate must be a number";      //if rate isn't a number

                if (fixedTextBox.Text == ".")                         //fixed amount check
                {
                    fixedErrorLabel.Text = "Amount must be a number";
                    error = true;
                }
                else
                {
                    for (int i = 0; i < fixedTextBox.Text.Length; i++)           //loop through fixed amount
                    {
                        if (!char.IsDigit(fixedTextBox.Text[i]))    //check for non digits
                        {
                            if (fixedTextBox.Text[i] == '.' && dot == false)    //exclude first decimal, if used
                                dot = true;
                            else
                            {
                                fixedErrorLabel.Text = "Amount must be a number";
                                error = true;
                                break;
                            }
                        }
                        else if (dot == true)                 //checks how many numbers after decimal, if used
                        {
                            if (count == 2)
                            {
                                fixedErrorLabel.Text = "Amount exceeded two decimal places";
                                error = true;
                                break;
                            }
                            else
                                count++;
                        }
                    }
                }

                if (comboBox3.Enabled == true)      //if plan has credits/deductions
                {
                    if (modifierTextBox.Text != curModName)     //if mod name was edited
                    {
                        if (CheckModExists())              //if mod name exists
                        {
                            modifierErrorLabel.Text = "C/D Name already Exists";
                            error = true;
                        }
                    }

                    if (modAmtTextBox.Text == ".")              //mod amount check
                    {
                        modAmtErrorLabel.Text = "Amount must be a number";
                        error = true;
                    }
                    else
                    {
                        dot = false;
                        for (int i = 0; i < modAmtTextBox.Text.Length; i++)           //loop through mod amount
                        {
                            if (!char.IsDigit(modAmtTextBox.Text[i]))    //check for non digits
                            {
                                if (modAmtTextBox.Text[i] == '.' && dot == false)    //exclude first decimal, if used
                                    dot = true;
                                else
                                {
                                    modAmtErrorLabel.Text = "Amount must be a number";
                                    error = true;
                                    break;
                                }
                            }
                            else if (dot == true)                 //checks how many numbers after decimal, if used
                            {
                                if (count == 2)
                                {
                                    modAmtErrorLabel.Text = "Amount exceeded two decimal places";
                                    error = true;
                                    break;
                                }
                                else
                                    count++;
                            }
                        }
                    }
                }
            }
            return error;
        }

        private bool CheckBenefitExists()
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@newBenefit";
            string newBenefit = benefitTextBox.Text;
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

        private bool CheckPlanExists()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@benefitname";
            param1.Value = listBox1.SelectedItem.ToString();
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@planName";
            param2.Value = planTextBox.Text;
            string name = "";

            String sql = "SELECT name = [Plan Name] FROM BenefitPlans WHERE [Benefit Name] = @benefitName AND [Plan Name] = @planName; ";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);

            con.Open();
            name = (string)command.ExecuteScalar();
            con.Close();

            if (name == planTextBox.Text)
                return true;
            else
                return false;
        }

        private bool CheckModExists()
        {
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@newName";
            param1.Value = modifierTextBox.Text;
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = listBox1.SelectedItem.ToString();
            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@planName";
            param3.Value = listBox2.SelectedItem.ToString();
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

            if (name == modifierTextBox.Text)
                return true;
            else
                return false;
        }

    }
}
