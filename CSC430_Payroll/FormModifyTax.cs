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
    public partial class FormModifyTax : Form
    {
        private string oldName;

        public FormModifyTax(string name)
        {
            InitializeComponent();
            textBox1.Text = name;
            DisplayRate(name);
            oldName = name;
        }

        private void DisplayRate(string name)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString); // making connection
            SqlDataReader reader;
            SqlCommand command;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@name";
            param.Value = name;

            String sql = "SELECT Rate FROM Taxes WHERE [Tax Name] = @name; ";

            String Output = "";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            con.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Output = Output + reader.GetValue(0).ToString();
                Output = Output[2].ToString() + Output[3];
                textBox2.Text = Output;
            }
            con.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            string newName = textBox1.Text;
            int rateSize = textBox2.Text.Length;

            if (textBox1.Text == "")
                MessageBox.Show("Please enter a tax name.");

            else if (textBox2.Text == "")
                MessageBox.Show("Please enter the tax rate.");

            else if ((rateSize == 1 && !char.IsDigit(textBox2.Text[0])) ||
                (rateSize == 2 && (!char.IsDigit(textBox2.Text[0]) || !char.IsDigit(textBox2.Text[1]))))
            {
                MessageBox.Show("Please enter numbers only for the Rate.");
                textBox2.Text = null;
            }
            else
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString); // making connection
                SqlDataReader reader;
                SqlCommand command;
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@oldName";
                param1.Value = oldName;

                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@newName";
                param2.Value = textBox1.Text;

                SqlParameter param3 = new SqlParameter();
                param3.ParameterName = "@rate";

                if (rateSize == 1)
                    param3.Value = ".0" + textBox2.Text;
                else
                    param3.Value = "." + textBox2.Text;

                String sql = "UPDATE Taxes SET [Tax Name] = @newName WHERE [Tax Name] = @oldName; " +
                             "UPDATE Taxes SET Rate = @rate WHERE [Tax Name] = @newName; ";

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
                this.Close();
            }
        }
    }
}
