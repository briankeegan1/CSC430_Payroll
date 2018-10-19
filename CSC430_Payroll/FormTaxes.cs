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
    public partial class FormTaxes : Form
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString); // making connection
        private SqlCommand command;
        private SqlDataReader reader;

        public FormTaxes()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            UpdateTaxes();
        }

        private void UpdateTaxes()   //keeps listBox and dropdown boxes up to date
        {
            listBox1.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            int size;
            String sql = "SELECT TOP 1 size = Number FROM Taxes ORDER BY Number DESC;";

            command = new SqlCommand(sql, con);

            con.Open();
            if (command.ExecuteScalar() != null)        //Error Handling for empty table
                size = (int)command.ExecuteScalar();
            else
                size = 0;

            con.Close();

            for (int i = 1; i <= size; i++)
            {
                PrintTaxes(i);
                DropDownBox_Add(i);
                DropDownBox_Delete(i);
            }


        }

        private void DropDownBox_Add(int count)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@count";
            param.Value = count;

            String sql = "SELECT [Tax Name] FROM Taxes WHERE Included = 0 AND Number = @count";
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

            String sql = "SELECT [Tax Name] FROM Taxes WHERE Number = @count";
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

        private void PrintTaxes(int count)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@count";
            param.Value = count;

            String sql = "SELECT [Tax Name] FROM Taxes WHERE Included = 1 AND Number = @count";
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

        private void FormTaxes_Load(object sender, EventArgs e)
        {

        }

        private void Remove_Click(object sender, EventArgs e)   //Removes Tax from listBox
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@input";
            string input = listBox1.GetItemText(listBox1.SelectedItem);
            param.Value = input;

            String sql = "UPDATE Taxes SET Included = 0 WHERE [Tax Name] = @input";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
            UpdateTaxes();
        }

        private void Add_Click(object sender, EventArgs e)  //Adds Tax to listBox
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@input";
            string input = comboBox1.GetItemText(comboBox1.SelectedItem);
            param.Value = input;

            String sql = "UPDATE Taxes SET Included = 1 WHERE [Tax Name] = @input";

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
            UpdateTaxes();
        }

        private void Create_Click(object sender, EventArgs e)   //Creates NEW Tax
        {
            int rateSize = textBox2.Text.Length;

            if (rateSize == 1 && !char.IsDigit(textBox2.Text[0]) || 
                rateSize == 2 && !char.IsDigit(textBox2.Text[1]))
            {   
                MessageBox.Show("Please enter numbers only for the Rate.");
                textBox2.Text = null;
            }
            else
            {
                SqlParameter param1 = new SqlParameter();
                SqlParameter param2 = new SqlParameter();
                param1.ParameterName = "@taxName";
                param2.ParameterName = "@rate";
                param1.Value = textBox1.Text;

                if (rateSize == 1)
                    param2.Value = ".0" + textBox2.Text;
                else
                    param2.Value = "." + textBox2.Text;

                if (CheckDuplicate() == true)
                {
                    MessageBox.Show("Tax already exists.", "Error Message");
                }
                else
                {
                    String sql = "DECLARE @size INT;" +
                                  "SET @size = 0;" +
                                 "SELECT TOP 1 @size = Number FROM Taxes ORDER BY Number DESC;" +
                                 "INSERT INTO Taxes (Number, [Tax Name], Included, Rate) VALUES (@size + 1, @taxName, 1, @rate);";

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
                    UpdateTaxes();
                }

                textBox1.Text = null;
                textBox2.Text = null;
            }
        }

        private bool CheckDuplicate()
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@input";
            string input = textBox1.Text;
            param.Value = input;
            string name = "failure";

            String sql = "SELECT name = [Tax Name] FROM Taxes WHERE [Tax Name] = @input;";

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

        private void Delete_Click(object sender, EventArgs e)   //Permanently Deletes a Tax
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@input";
            string input = comboBox2.GetItemText(comboBox2.SelectedItem);
            param.Value = input;

            String sql = "DELETE FROM Taxes WHERE [Tax Name] = @input;";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            comboBox1.SelectedItem = null; //clears ADD combo box incase deleted item is selected
            comboBox2.SelectedItem = null;
            con.Close();
            ResortTable();
            UpdateTaxes();
        }

        private void ResortTable()  //makes sure there isn't a number gap after deletion
        {
            String sql = "DECLARE @rowNum INT;" +
                         "DECLARE @count INT;" +
                         "DECLARE @nextRowNum INT;" +
                         "SET @count = 1;" +

                         "DECLARE @size INT;" +
                         "SELECT TOP 1 @size = Number " +
                         "FROM Taxes " +
                         "ORDER BY Number DESC;" +

                         "WHILE(@count < @size) " +
                         "BEGIN " +
                         "SET @rowNum = -1;" +
                         "SELECT @rowNum = Number FROM Taxes WHERE Number = @count;" +

                         "IF(@rowNum = -1) " +
                         "BEGIN " +
                         "UPDATE Taxes SET Number = @count WHERE Number = (@count + 1);" +
                         "SELECT @rowNum = Number FROM Taxes WHERE Number = @count;" +
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

    }
}
