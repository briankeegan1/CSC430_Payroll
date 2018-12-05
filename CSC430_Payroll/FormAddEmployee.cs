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
    public partial class FormAddEmployee : Form
    {
        //use for refresh grid
        private readonly formMain form1;
        private List<string> Benefits = new List<string>();
        private List<string> Taxes = new List<string>();
        private List<string> BenefitPlans = new List<string>();
        private List<string> CreditsDeductions = new List<string>();

        private List<object> AppliedPlans = new List<object>();
        private List<object> AppliedCreditsDeductions = new List<object>();

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
            refreshTaxesListBox();
            refreshBenefitsListBox();
            refreshBenefitPlans();
            refreshCreditsDeductions();
            this.AcceptButton = btnCreateEmployee;
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
            for (int i = 0; i < Benefits.Count(); i++)
            {
                if (checkedListBox2.GetItemChecked(i) == true)
                {
                    sqlquery = "UPDATE Employee SET [" + Benefits[i] + "] = " + 1 + "WHERE ID = " + id;
                    SqlCommand command = new SqlCommand(sqlquery, con);
                    command.ExecuteNonQuery();
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
            Benefits = form1.getBenefits();
            for (int i = 0; i < Benefits.Count(); i++)
            {
                checkedListBox2.Items.Add(Benefits[i]);
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

                int checkedIndex = 0;
                if (AppliedPlans.Count() > 0)
                {
                    for(int i = 0; i < AppliedPlans.Count(); i++)
                    {
                        string temp = AppliedPlans[i].ToString();
                        string temp2 = AppliedPlans[i].ToString().Substring(0, AppliedPlans[i].ToString().Length - 2);
                        char last = temp[temp.Length - 1];
                        int benefitNum = (last - '0') - 1;
                        if (checkedListBox2.SelectedIndex == benefitNum)
                        {
                            checkedIndex = checkedListBox3.Items.IndexOf(temp2);
                            if(checkedIndex >= 0)
                            {
                                checkedListBox3.SetItemCheckState(checkedIndex, CheckState.Checked);
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
                        AppliedPlans.Remove(checkedListBox3.Items[i]);
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

        private void ifPlanCheckedOrNot()
        {
            int[] checkedIndex = new int[999];
            foreach (int index in checkedListBox3.CheckedIndices)
            {
                checkedIndex[index] = index;
            }

            if (checkedListBox3.CheckedItems.Count > 0)
            {
                if (checkedListBox3.GetItemCheckState(checkedIndex[checkedListBox3.SelectedIndex]) == CheckState.Checked
                    && checkedListBox3.SelectedIndex == checkedIndex[checkedListBox3.SelectedIndex])
                {
                    checkedListBox4.Enabled = true;
                }
                else
                    checkedListBox4.Enabled = false;
            }
            else
            {
                checkedListBox4.Enabled = false;
                checkedListBox4.Items.Clear();
            }
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
                for(int i = 0; i < AppliedCreditsDeductions.Count(); i++)
                {
                    int checkedIndex = 0;
                    string temp = AppliedCreditsDeductions[i].ToString();
                    string temp2 = AppliedCreditsDeductions[i].ToString().Substring(0, AppliedCreditsDeductions[i].ToString().Length - 4);
                    char last = temp[temp.Length - 1];
                    int planNum = (last - '0') - 1;
                    if (checkedListBox3.SelectedIndex == planNum)
                    {
                        checkedIndex = checkedListBox4.Items.IndexOf(temp2);
                        if(checkedIndex >= 0)
                        {
                            checkedListBox4.SetItemCheckState(checkedIndex, CheckState.Checked);
                        }
                    }
                }
            }
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox4.Items.Clear();
            updatePlans();
            
        }
        public void checkedListBox2_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (checkedListBox2.CheckedItems.Count == 0)
            {
                checkedListBox4.Items.Clear();
            }
            bool removed = false;
            if (checkedListBox2.CheckedItems.Count > 0 && checkedListBox2.CheckedItems.Contains(checkedListBox2.SelectedItem))
            {
                checkedListBox3.Enabled = false;
                for (int i = 0; i < checkedListBox3.Items.Count; i++)
                {
                    checkedListBox3.SetItemCheckState(i, CheckState.Unchecked);
                    AppliedPlans.Remove(checkedListBox3.Items[i]);
                    removed = true;
                }
                for (int i = 0; i < checkedListBox4.Items.Count; i++)
                {
                    checkedListBox4.SetItemCheckState(i, CheckState.Unchecked);
                    AppliedCreditsDeductions.Remove(checkedListBox4.Items[i]);
                }
                checkedListBox4.Items.Clear();
            }
            else
            {
                checkedListBox3.Enabled = true;
            }
            if(removed)
            {
                removeCreditsDeductions();
                checkedListBox4.Items.Clear();
            }
        }

        private void removeCreditsDeductions()
        {
            checkedListBox3.SelectedIndex = 0;
            for(int i = 0; i < checkedListBox3.Items.Count; i++)
            {
                checkedListBox3.SelectedIndex = i;
                foreach (object Item in checkedListBox4.Items)
                {
                    AppliedCreditsDeductions.Remove(Item + "," + (checkedListBox2.SelectedIndex + 1).ToString() + "," + (checkedListBox3.SelectedIndex + 1).ToString());
                }
            }
        }

        private void checkedListBox3_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                bool added = false;
                bool removed = false;
                foreach (object Item in checkedListBox3.CheckedItems)
                {
                    if(!AppliedPlans.Contains(Item + "," + (checkedListBox2.SelectedIndex + 1).ToString()))
                    {
                        AppliedPlans.Add(Item + "," + (checkedListBox2.SelectedIndex + 1).ToString());
                        added = true;
                    }
                }
                foreach (object Item in checkedListBox3.Items)
                {
                    if (!checkedListBox3.CheckedItems.Contains(Item))
                    {
                        if (AppliedPlans.Contains(Item + "," + (checkedListBox2.SelectedIndex+1).ToString()))
                        {
                            AppliedPlans.Remove(Item + "," + (checkedListBox2.SelectedIndex+1).ToString());
                            removed = true;
                        }
                    }
                }
                if(added)
                {
                    ifPlanCheckedOrNot();
                }
                else if(removed)
                {
                    foreach (object Item in checkedListBox4.Items)
                    {
                        AppliedCreditsDeductions.Remove(Item + "," + (checkedListBox2.SelectedIndex + 1).ToString() + "," + (checkedListBox3.SelectedIndex + 1).ToString());
                    }
                    updatePlans();
                    checkedListBox4.Items.Clear();
                }
            }));
            if (checkedListBox3.CheckedItems.Count > 0 && checkedListBox3.CheckedItems.Contains(checkedListBox3.SelectedItem))
            {
                updateModifiers();
                for (int i = 0; i < checkedListBox4.Items.Count; i++)
                {
                    checkedListBox4.SetItemCheckState(i, CheckState.Unchecked);
                    AppliedCreditsDeductions.Remove(checkedListBox4.Items[i]);
                }
            }
        }

        private void checkedListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ifPlanCheckedOrNot();
            if(checkedListBox3.SelectedItem != null)
            {
                updateModifiers();
            }
            
        }

        private void checkedListBox4_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                foreach (object Item in checkedListBox4.CheckedItems)
                {
                    if (!AppliedCreditsDeductions.Contains(Item + "," + (checkedListBox2.SelectedIndex + 1).ToString() + "," + (checkedListBox3.SelectedIndex + 1).ToString()))
                    {
                        AppliedCreditsDeductions.Add(Item + "," + (checkedListBox2.SelectedIndex + 1).ToString() + "," + (checkedListBox3.SelectedIndex + 1).ToString());
                    }
                }
                foreach (object Item in checkedListBox4.Items)
                {
                    if (!checkedListBox4.CheckedItems.Contains(Item))
                    {
                        if (AppliedCreditsDeductions.Contains(Item + "," + (checkedListBox2.SelectedIndex+1).ToString() + "," + (checkedListBox3.SelectedIndex+1).ToString()))
                        {
                            AppliedCreditsDeductions.Remove(Item + "," + (checkedListBox2.SelectedIndex+1).ToString() + "," + (checkedListBox3.SelectedIndex+1).ToString());
                        }
                    }
                }
            }));
        }
    }
}