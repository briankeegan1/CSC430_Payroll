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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            this.AcceptButton = btnLogin;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

       private void btnLogin_Click(object sender, EventArgs e) //when Login button is clicked
        {
            this.Hide();                                //hides the login screen
            FormMain formMain = new FormMain();         //variable for the FormMain to be opened after login
            formMain.Closed += (s, args) => this.Close(); //closes FormLogin
            formMain.Show();                            //displays FormMain
        } //code without SQL login

        /*private void btnLogin_Click(object sender, EventArgs e) //when Login button is clicked
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection   
            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM [User] WHERE username='" + txtUsername.Text + "' AND password='" + txtPassword.Text + "'", con);
            con.Open();
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if(dt.Rows[0][0].ToString() == "1")
            {
                this.Hide();                                //hides the login screen
                FormMain formMain = new FormMain();         //variable for the FormMain to be opened after login
                formMain.Closed += (s, args) => this.Close(); //closes FormLogin
                formMain.Show();                            //displays FormMain
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
