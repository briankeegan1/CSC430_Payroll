﻿using System;
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
    public partial class FormAddEmployee : Form
    {
        //use for refresh grid
        private readonly formMain form1;
        private List<string> Benefits = new List<string>();
        private List<string> Taxes = new List<string>();
        private List<string> BenefitPlans = new List<string>();
        private List<string> CreditsDeductions = new List<string>();

        private List<object> AppliedBenefits = new List<object>();
        private List<object> AppliedPlans = new List<object>();
        private List<object> AppliedCreditsDeductions = new List<object>();

        private int noInvoke = 0;

        public FormAddEmployee()
        {
            InitializeComponent();
        }
        //use for refresh grid
        public FormAddEmployee(formMain form)
        {
            InitializeComponent();
            form1 = form;
            txtZipcode.MaxLength = 10;
            refreshBenefts();
            refreshBenefitPlans();
            refreshCreditsDeductions();
            refreshTaxesListBox();
            refreshBenefitsListBox();
            this.AcceptButton = btnCreateEmployee;
        }

        private void refreshBenefts()
        {
            Benefits = form1.getBenefits();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
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

        

        private void btnCreateEmployee_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection 
            con.Open();
            string sqlquery = "INSERT INTO Employee(ID, [Last Name], [First Name], DOB, Address, ZIP) VALUES(@ID, @LastName,@FirstName,@DOB,@Address,@ZIP)";
            string sqlquery1 = "SELECT COUNT(*) FROM [Employee] WHERE ([ID] = @ID)";

            SqlCommand command = new SqlCommand(sqlquery, con);
            SqlCommand command1 = new SqlCommand(sqlquery1, con);

            try
            {
                int numID = Int32.Parse(this.txtEmployeeID.Text);
                command1.Parameters.AddWithValue("@ID", txtEmployeeID.Text);
                int checkID = (int)command1.ExecuteScalar();

                if (checkID > 0)
                {
                    MessageBox.Show("ID is already taken.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (numID < 0)
                {
                    MessageBox.Show("ID cannot be an integer less than 0. (ex: 0, 1, 2, 3)");
                }
                else
                {
                    command.Parameters.AddWithValue("@ID", numID);
                    command.Parameters.AddWithValue("@LastName", this.txtLastName.Text);
                    command.Parameters.AddWithValue("@FirstName", this.txtFirstName.Text);
                    command.Parameters.AddWithValue("@DOB", this.dateTimePicker1.Value);
                    command.Parameters.AddWithValue("@Address", this.txtAddress.Text);
                    command.Parameters.AddWithValue("@ZIP", this.txtZipcode.Text);
                    command.ExecuteNonQuery();
                    addTaxes(con, numID);
                    addBenefits(con, numID);
                    addPlans(con, numID);
                    addCreditsDeductions(con, numID);
                    MessageBox.Show("Employee Created.");
                    form1.resetPlacementValues();
                    //calling grid refresh function from FormMain
                    form1.gridRefresh();
                    con.Close();
                    this.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addTaxes(SqlConnection con, int id)
        {
            string sqlquery = "";
            for (int i = 0; i < Taxes.Count(); i++)
            {
                if(checkedListBox1.GetItemChecked(i) == true)
                {
                    sqlquery = "UPDATE Employee SET [" + Taxes[i] + "] = " + 1 + "WHERE ID = " + id;
                    SqlCommand command = new SqlCommand(sqlquery, con);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void addBenefits(SqlConnection con, int id)
        {
            string sqlquery = "";
            string insertion = "";
            for (int i = 0; i < Benefits.Count(); i++)
            {
                string temp = Benefits[i].ToString();
                char benefitNum = temp[temp.Length - 1];
                for(int j = 0; j < AppliedBenefits.Count(); j++)
                {
                    char[] appliedBenefitNum = AppliedBenefits[j].ToString().ToCharArray();
                    if (benefitNum == appliedBenefitNum[0])
                    {
                        string temp2 = Benefits[i].ToString();
                        insertion += temp2;
                        sqlquery = "UPDATE Employee SET Benefits = " + "'" + insertion + "'" + " WHERE ID = " + id;
                        SqlCommand command = new SqlCommand(sqlquery, con);
                        command.ExecuteNonQuery();
                        insertion += "|";
                    }
                }
            }
        }

        private void addPlans(SqlConnection con, int id)
        {
            string sqlquery = "";
            string insertion = "";
            for (int i = 0; i < BenefitPlans.Count(); i++)
            {
                string temp = BenefitPlans[i].ToString();
                char planNum = temp[temp.Length - 1];
                char benefitNum = temp[temp.Length - 3];
                for (int j = 0; j < AppliedPlans.Count(); j++)
                {
                    string temp2 = AppliedPlans[j].ToString();
                    string appliedPlanNumString = temp2.Substring(temp2.Length - 1);
                    string appliedBenefitNumString = temp2.Substring(temp2.Length - 3);

                    char[] appliedPlanNum = appliedPlanNumString.ToCharArray();
                    char[] appliedBenefitNum = appliedBenefitNumString.ToCharArray();

                    if (benefitNum == appliedBenefitNum[0] && planNum == appliedPlanNum[0])
                    {
                        string temp3 = BenefitPlans[i].ToString();
                        insertion += temp3;
                        sqlquery = "UPDATE Employee SET Plans = " + "'" + insertion + "'" + " WHERE ID = " + id;
                        SqlCommand command = new SqlCommand(sqlquery, con);
                        command.ExecuteNonQuery();
                        insertion += "|";
                    }
                }
            }
        }

        private void addCreditsDeductions(SqlConnection con, int id)
        {
            string sqlquery = "";
            string insertion = "";
            for (int i = 0; i < CreditsDeductions.Count(); i++)
            {
                string temp = CreditsDeductions[i].ToString();
                char creditNum = temp[temp.Length - 1];
                char planNum = temp[temp.Length - 3];
                char benefitNum = temp[temp.Length - 5];
                
                for (int j = 0; j < AppliedCreditsDeductions.Count(); j++)
                {
                    string temp2 = AppliedCreditsDeductions[j].ToString();
                    string appliedCreditNumString = temp2.Substring(temp2.Length - 1);
                    string appliedPlanNumString = temp2.Substring(temp2.Length - 3);
                    string appliedBenefitNumString = temp2.Substring(temp2.Length - 5);

                    char[] appliedCreditNum = appliedCreditNumString.ToCharArray();
                    char[] appliedPlanNum = appliedPlanNumString.ToCharArray();
                    char[] appliedBenefitNum = appliedBenefitNumString.ToCharArray();

                    if (benefitNum == appliedBenefitNum[0] && planNum == appliedPlanNum[0] && creditNum == appliedCreditNum[0])
                    {
                        string temp3 = CreditsDeductions[i].ToString();
                        insertion += temp3;
                        sqlquery = "UPDATE Employee SET [Credits/Deductions] = " + "'" + insertion + "'" + " WHERE ID = " + id;
                        SqlCommand command = new SqlCommand(sqlquery, con);
                        command.ExecuteNonQuery();
                        insertion += "|";
                    }
                }
            }
        }

        private void refreshTaxesListBox()
        {
            Taxes = form1.getTaxes();
            for (int i = 0; i < Taxes.Count(); i++)
            {
                checkedListBox1.Items.Add(Taxes[i]);
            }
        }

        private void refreshBenefitsListBox()
        {
            
            for (int i = 0; i < Benefits.Count(); i++)
            {
                checkedListBox2.Items.Add(Benefits[i].Substring(0, Benefits[i].Length - 2));
            }
        }

        private void refreshBenefitPlans()
        {
            BenefitPlans = form1.getBenefitPlans();
        }
        
        private void refreshCreditsDeductions()
        {
            CreditsDeductions = form1.getCreditsDeductions();
        }

        private void FormAddEmployee_Load(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void printPlans(int count)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlDataReader reader;
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@count";
            param1.Value = count;
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = checkedListBox2.SelectedItem.ToString();

            String sql = "SELECT [Plan Name] FROM BenefitPlans WHERE " +
                         "[Benefit Name] = @benefitName AND Number = @count; ";

            String Output = "";

            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Output = "";
                Output = Output + reader.GetValue(0);
                checkedListBox3.Items.Add(Output);
            }

            
            
            
            con.Close();
        }

        private void updatePlans()
        {
            checkedListBox3.Items.Clear();
            if (checkedListBox2.SelectedIndex != -1)
            {
                SqlParameter param = new SqlParameter();
                string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                param.ParameterName = "@benefitName";
                param.Value = checkedListBox2.SelectedItem.ToString();

                int size;
                String sql = "SELECT TOP 1 size = Number FROM BenefitPlans WHERE [Benefit Name] = @benefitName ORDER BY Number DESC; ";
                
                SqlCommand command = new SqlCommand(sql, con);
                command.Parameters.Add(param);

                con.Open();
                if (command.ExecuteScalar() != null)        //Error Handling for empty table
                    size = (int)command.ExecuteScalar();
                else
                    size = 0;
                con.Close();

                if (checkedListBox2.SelectedIndex != -1)
                {
                    for (int i = 1; i <= size; i++)
                    {
                        printPlans(i);
                    }
                }

                if (AppliedPlans.Count() > 0)
                { 
                    for(int i = 0; i < checkedListBox3.Items.Count; i++)
                    {
                        if(checkedListBox2.GetItemCheckState(checkedListBox2.SelectedIndex) == CheckState.Checked)
                        {
                            string temp = (checkedListBox2.SelectedIndex+1).ToString() + "," + (i+1).ToString(); //bnum,plannum
                            if (AppliedPlans.Contains(temp))
                            {
                                char planNum = temp[temp.Length - 1];
                                char benefitNum = temp[temp.Length - 3];
                                int planNumInt = Convert.ToInt32(new string(planNum, 1));
                                int benefitNumInt = Convert.ToInt32(new string(benefitNum, 1));
                                if ((checkedListBox2.SelectedIndex+1) == benefitNumInt)
                                {
                                    checkedListBox3.SetItemCheckState((planNumInt - 1), CheckState.Checked);
                                    noInvoke++;
                                }
                            }
                        }
                    }
                }

                if (checkedListBox2.CheckedItems.Count > 0 && checkedListBox2.CheckedItems.Contains(checkedListBox2.SelectedItem))
                {
                    checkedListBox3.Enabled = true;
                }
                else
                {
                    checkedListBox3.Enabled = false;
                    for (int i = 0; i < checkedListBox3.Items.Count; i++)
                    {
                        checkedListBox3.SetItemCheckState(i, CheckState.Unchecked);
                        //AppliedPlans.Remove(checkedListBox3.Items[i]);
                    }
                    checkedListBox4.Items.Clear();
                }
            }
        }

        private void PrintModifiers(int count)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@planName";
            param1.Value = checkedListBox3.SelectedItem.ToString();
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = checkedListBox2.SelectedItem.ToString();
            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@count";
            param3.Value = count;

            String sql = "SELECT [Name] FROM [Credits/Deductions] WHERE " +
                         "[Benefit Name] = @benefitName AND [Plan Name] = @planName AND Number = @count; ";

            String Output = "";

            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.Parameters.Add(param3);

            con.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Output = "";
                Output = Output + reader.GetValue(0);
                checkedListBox4.Items.Add(Output);
            }
            
            
            con.Close();
        }

        private void updateModifiers()
        {
            checkedListBox4.Items.Clear();
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@planName";
            param1.Value = checkedListBox3.SelectedItem.ToString();
            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@benefitName";
            param2.Value = checkedListBox2.SelectedItem.ToString();

            int size;
            String sql = "SELECT TOP 1 size = Number FROM [Credits/Deductions] " +
                         "WHERE [Benefit Name] = @benefitName AND [Plan Name] = @planName ORDER BY Number DESC; ";

            SqlCommand command = new SqlCommand(sql, con);
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

            if (AppliedCreditsDeductions.Count() > 0)
            {
                for (int i = 0; i < checkedListBox4.Items.Count; i++)
                {
                    if (checkedListBox3.GetItemCheckState(checkedListBox3.SelectedIndex) == CheckState.Checked)
                    {
                        string temp = (checkedListBox2.SelectedIndex + 1).ToString() + 
                            "," + (checkedListBox3.SelectedIndex + 1).ToString() + "," + (i+1).ToString(); //bnum,plannum,creditnum
                        if (AppliedCreditsDeductions.Contains(temp))
                        {
                            char creditNum = temp[temp.Length - 1];
                            char planNum = temp[temp.Length - 3];
                            char benefitNum = temp[temp.Length - 5];

                            int creditNumInt = Convert.ToInt32(new string(creditNum, 1));
                            int planNumInt = Convert.ToInt32(new string(planNum, 1));
                            int benefitNumInt = Convert.ToInt32(new string(benefitNum, 1));
 
                            if ((checkedListBox2.SelectedIndex + 1) == benefitNumInt && 
                                (checkedListBox3.SelectedIndex + 1) == planNumInt)
                            {
                                checkedListBox4.SetItemCheckState((creditNumInt - 1), CheckState.Checked);
                                noInvoke++;
                            }
                        }
                    }
                }
            }
        }        

        private void removeAllPlans()
        {
            for (int i = 0; i < checkedListBox3.Items.Count; i++)
            {
                checkedListBox3.SelectedIndex = i;
                string temp = (checkedListBox2.SelectedIndex + 1).ToString() + ","
                    + (checkedListBox3.SelectedIndex + 1).ToString();
                    AppliedPlans.Remove(temp);
                checkedListBox3.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void removeAllCreditsDeductions()
        {
            for(int i = 0; i < checkedListBox3.Items.Count; i++)
            {
                checkedListBox3.SelectedIndex = i;
                for(int j = 0; j < checkedListBox4.Items.Count; j++)
                {
                    checkedListBox4.SelectedIndex = j;
                    string temp = (checkedListBox2.SelectedIndex + 1).ToString() + ","
                        + (checkedListBox3.SelectedIndex + 1).ToString() + ","
                        + (checkedListBox4.SelectedIndex + 1).ToString();
                    checkedListBox4.SetItemCheckState(i, CheckState.Unchecked);
                    AppliedCreditsDeductions.Remove(temp);
                }
            }
        }

        private void removeCreditsDeductionsOnIndex()
        {
            for(int i = 0; i < checkedListBox4.Items.Count; i++)
            {
                checkedListBox4.SelectedIndex = i;
                string temp = (checkedListBox2.SelectedIndex + 1).ToString() + ","
                    + (checkedListBox3.SelectedIndex + 1).ToString() + ","
                    + (checkedListBox4.SelectedIndex + 1).ToString();
                checkedListBox4.SetItemCheckState(i, CheckState.Unchecked);
                AppliedCreditsDeductions.Remove(temp);
            }
        }

        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (!AppliedBenefits.Contains((checkedListBox2.SelectedIndex + 1).ToString()))
                {
                    AppliedBenefits.Add((checkedListBox2.SelectedIndex + 1).ToString());
                }

                if (!checkedListBox2.CheckedItems.Contains(checkedListBox2.SelectedItem))
                {
                    if (AppliedBenefits.Contains((checkedListBox2.SelectedIndex + 1).ToString()))
                    {
                        AppliedBenefits.Remove((checkedListBox2.SelectedIndex + 1).ToString());
                    }
                }
                if (checkedListBox2.GetItemCheckState(checkedListBox2.SelectedIndex) == CheckState.Checked)
                {
                    checkedListBox3.Enabled = true;
                }
                else
                {
                    removeAllPlans();
                    removeAllCreditsDeductions();
                    checkedListBox3.Enabled = false;
                    checkedListBox4.Enabled = false;
                    checkedListBox4.Items.Clear();
                }
            }));
            

        }

        private void checkedListBox3_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                if(noInvoke == 0)
                {
                    string temp = (checkedListBox2.SelectedIndex + 1).ToString() + "," + (checkedListBox3.SelectedIndex + 1).ToString();
                    if (!AppliedPlans.Contains(temp))
                    {
                        AppliedPlans.Add(temp);
                    }

                    if (!checkedListBox3.CheckedItems.Contains(checkedListBox3.SelectedItem))
                    {
                        if (AppliedPlans.Contains(temp))
                        {
                            AppliedPlans.Remove(temp);
                        }
                    }

                }
                if (checkedListBox3.SelectedIndex != -1)
                {
                    if (checkedListBox3.GetItemCheckState(checkedListBox3.SelectedIndex) == CheckState.Checked)
                    {
                        checkedListBox4.Enabled = true;
                    }
                    else
                    {
                        checkedListBox4.Enabled = false;
                        removeCreditsDeductionsOnIndex();
                    }
                        
                }
                if (noInvoke != 0)
                {
                    noInvoke--;
                }
            }));
            
        }

        private void checkedListBox4_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (noInvoke == 0)
                {
                    string temp = (checkedListBox2.SelectedIndex + 1).ToString() + "," + (checkedListBox3.SelectedIndex + 1).ToString() + "," + (checkedListBox4.SelectedIndex + 1).ToString();
                    if (!AppliedCreditsDeductions.Contains(temp))
                    {

                        AppliedCreditsDeductions.Add(temp);
                    }

                    if (!checkedListBox4.CheckedItems.Contains(checkedListBox4.SelectedItem))
                    {
                        if (AppliedCreditsDeductions.Contains(temp))
                        {
                            AppliedCreditsDeductions.Remove(temp);
                        }
                    }
                }
                if (noInvoke != 0)
                {
                    noInvoke--;
                }

            }));
        }

        private void checkedListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox3.SelectedItem != null)
            {
                updateModifiers();
            }

            if(checkedListBox3.CheckedItems.Count > 0)
            {
                if (checkedListBox3.GetItemCheckState(checkedListBox3.SelectedIndex) == CheckState.Checked)
                {
                    checkedListBox4.Enabled = true;
                }
                else
                    checkedListBox4.Enabled = false;
            }
        }
        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(checkedListBox2.CheckedItems.Count > 0)
            {
                if (checkedListBox2.GetItemCheckState(checkedListBox2.SelectedIndex) == CheckState.Checked)
                {
                    checkedListBox3.Enabled = true;
                }
                else
                    checkedListBox3.Enabled = false;
            }

            checkedListBox4.Items.Clear();
            updatePlans();
        }
    }
}