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
        private SqlCommand command;
        private SqlDataReader reader;

        public FormBenefits()
        {
            InitializeComponent();

            UpdateBenefits();
        }

        private void UpdateBenefits()   //keeps listBox and dropdown boxes up to date
        {
            listBox1.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            int size;
            String sql = "SELECT TOP 1 size = Number FROM Benefits ORDER BY Number DESC;";

            command = new SqlCommand(sql, con);

            con.Open();
            size = (int) command.ExecuteScalar();

            con.Close();

            for (int i = 1; i <= size; i++)
            {
                PrintBenefits(i);
                DropDownBox_Add(i);
                DropDownBox_Delete(i);
            }

                
        }

        private void DropDownBox_Add(int count)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@count";
            param.Value = count;

            String sql = "SELECT [Benefit Name] FROM Benefits WHERE Included = 0 AND Number = @count";
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

        private void DropDownBox_Delete(int count)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@count";
            param.Value = count;

            String sql = "SELECT [Benefit Name] FROM Benefits WHERE Number = @count";
            String Output = "";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Output = "";
                Output = Output + reader.GetValue(0);
                comboBox2.Items.Add(Output);
            }
            con.Close();
        }

        private void PrintBenefits(int count)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@count";
            param.Value = count;

            String sql = "SELECT [Benefit Name] FROM Benefits WHERE Included = 1 AND Number = @count";
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

        private void FormBenefits_Load(object sender, EventArgs e)
        {
            
        }

        private void Remove_Click(object sender, EventArgs e)   //Removes Benefit from listBox
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@input";
            string input = listBox1.GetItemText(listBox1.SelectedItem);
            param.Value = input;

            String sql = "UPDATE Benefits SET Included = 0 WHERE [Benefit Name] = @input";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
            UpdateBenefits();
        }

        private void Add_Click(object sender, EventArgs e)  //Adds Benefit to listBox
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@input";
            string input = comboBox1.GetItemText(comboBox1.SelectedItem);
            param.Value = input;

            String sql = "UPDATE Benefits SET Included = 1 WHERE [Benefit Name] = @input";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            comboBox1.SelectedItem = null;
            con.Close();
            UpdateBenefits();
        }

        private void Create_Click(object sender, EventArgs e)   //Creates NEW Benefit
        {
            SqlParameter param1 = new SqlParameter();
            SqlParameter param2 = new SqlParameter();
            param1.ParameterName = "@input";
            param2.ParameterName = "@count";
            string input = textBox1.Text;
            param1.Value = input;

            if (CheckDuplicate() == true)
            {
                MessageBox.Show("Benefit already exists.", "Error Message");
            }
            else
            {
                String sql = "DECLARE @size INT;" +
                             "SELECT TOP 1 @size = Number FROM Benefits ORDER BY Number DESC;" +
                             "INSERT INTO Benefits (Number, [Benefit Name], Included) VALUES (@size + 1, @input, 1);";

                command = new SqlCommand(sql, con);
                command.Parameters.Add(param1);

                con.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader.GetValue(0));
                }

                con.Close();
                UpdateBenefits();
            }

            textBox1.Text = null;
        }

        private bool CheckDuplicate()
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

            if (name == input)  //return true if there is a duplicate
                return true;
            else
                return false;
        }

        private void Delete_Click(object sender, EventArgs e)   //Permanently Deletes a Benefit
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@input";
            string input = comboBox2.GetItemText(comboBox2.SelectedItem);
            param.Value = input;

            String sql = "DELETE FROM Benefits WHERE [Benefit Name] = @input;";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            comboBox2.SelectedItem = null;
            con.Close();
            ResortTable();
            UpdateBenefits();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
