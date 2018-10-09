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
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void btnRegConfirm_Click(object sender, EventArgs e)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection 
            con.Open();
            string sqlquery = "INSERT INTO [User]([Username], [Password]) VALUES(@Username, @Password)";

            try
            {
                SqlCommand command = new SqlCommand(sqlquery, con);
                command.Parameters.AddWithValue("@Username", this.txtUsername.Text);
                command.Parameters.AddWithValue("@Password", this.txtPassword.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Successfully registered. Redirecting to Login Page.");
                this.Hide();
                FormLogin formLogin = new FormLogin();
                formLogin.Closed += (s, args) => this.Close();
                formLogin.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

   
    }
}
