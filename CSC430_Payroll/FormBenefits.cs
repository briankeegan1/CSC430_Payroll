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
        public FormBenefits()
        {
            InitializeComponent();

            UpdateBenefits();
        }

        private void UpdateBenefits()
        {
            listBox1.Items.Clear();
            comboBox1.Items.Clear();

            for (int i = 0; i < 10; i++)
                PrintBenefits(i);

            for (int i = 0; i < 10; i++)
                DropDownBox(i);
        }

        private void DropDownBox(int count)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();

            SqlCommand command;
            SqlDataReader reader;

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@count";
            param.Value = count;


            String sql, Output = "";


            sql = "Select [Benefit Name] from Benefits where Included = 0 and Number = @count";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Output = "";
                Output = Output + reader.GetValue(0);
                comboBox1.Items.Add(Output);
            }

            con.Close();
        }

        private void PrintBenefits(int count)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();

            SqlCommand command;
            SqlDataReader reader;

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@count";
            param.Value = count;

            String sql, Output = "";

            sql = "Select [Benefit Name] from Benefits where Included = 1 and Number = @count";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();

            SqlCommand command;
            SqlDataReader reader;

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@name";
            string name = listBox1.GetItemText(listBox1.SelectedItem);
            param.Value = name;

            String sql;

            sql = "Update Benefits Set Included = 0 where [Benefit Name] = @name";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            con.Close();
            UpdateBenefits();

        }

        private void Add_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection
            con.Open();

            SqlCommand command;
            SqlDataReader reader;

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@name";
            string name = comboBox1.GetItemText(comboBox1.SelectedItem);
            param.Value = name;

            String sql;

            sql = "Update Benefits Set Included = 1 where [Benefit Name] = @name";

            command = new SqlCommand(sql, con);
            command.Parameters.Add(param);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            comboBox1.SelectedItem = null;
            con.Close();
            UpdateBenefits();
        }
    }
}
