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
    public partial class CreateCreditOrDeduc : Form
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString); // making connection
        private SqlCommand command;
        private SqlDataReader reader;


        public CreateCreditOrDeduc()
        {
            InitializeComponent();
            CreateBenefitsList();
            CreatePlansList();
        }

        public CreateCreditOrDeduc(string benefitName)
        {
            InitializeComponent();
            CreateBenefitsList();
            comboBox1.SelectedItem = benefitName;
;        }

        public CreateCreditOrDeduc(string benefitName, string planName)
        {
            InitializeComponent();
            CreateBenefitsList();
            comboBox1.SelectedItem = benefitName;
            CreatePlansList();
            comboBox2.SelectedItem = planName;
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
           comboBox2.SelectedIndex = -1;
        }
    }
}
