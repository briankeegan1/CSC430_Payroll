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

        /*private void btnLogin_Click(object sender, EventArgs e) //when Login button is clicked
         {
             this.Hide();                                //hides the login screen
             FormMain formMain = new FormMain();         //variable for the FormMain to be opened after login
             formMain.Closed += (s, args) => this.Close(); //closes FormLogin
             formMain.Show();                            //displays FormMain
         } //code without SQL login
         */
      
        private void btnLogin_Click(object sender, EventArgs e) //when Login button is clicked
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
                SqlConnection con = new SqlConnection(connectionString); // making connection   

                //SqlCommand sda = new SqlCommand("SELECT COUNT(*) FROM [User] WHERE Username='" + txtUsername.Text + "' AND Password='" + txtPassword.Text + "'", con);

                con.Open();
                SqlCommand sda = new SqlCommand("SELECT COUNT(*) FROM [User] WHERE Username='" + txtUsername.Text + "' AND Password='" + txtPassword.Text + "'", con);

                
               

                int userExists = int.Parse(sda.ExecuteScalar().ToString());
                
                if(userExists > 0)
                {
                    Boolean checkuser = checkUserType();
                    if (checkuser)
                    {
                        this.Hide();                                //hides the login screen
                        formMain formMain = new formMain();         //variable for the FormMain to be opened after login
                        formMain.Closed += (s, args) => this.Close(); //closes FormLogin
                        formMain.Show();                            //displays FormMain
                    }

                    /*else
                    {
                        this.Hide();                                //hides the login screen
                        UserBase userbase = new UserBase();         //variable for the FormMain to be opened after login
                        userbase.Closed += (s, args) => this.Close(); //closes FormLogin
                        userbase.Show();                            //displays FormMain
                    }*/

                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private Boolean checkUserType()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString; //loading connection string from App.config
            SqlConnection con = new SqlConnection(connectionString); // making connection   

            //SqlCommand sda = new SqlCommand("SELECT COUNT(*) FROM [User] WHERE Username='" + txtUsername.Text + "' AND Password='" + txtPassword.Text + "'", con);

            con.Open();
            //SqlCommand sda = new SqlCommand("SELECT COUNT(*) FROM [User] WHERE Username='" + txtUsername.Text + "' AND Password='" + txtPassword.Text + "'", con);
            SqlCommand sda = new SqlCommand("SELECT UserRole FROM [User] WHERE Username='" + txtUsername.Text + "'", con);



            string userType = sda.ExecuteScalar().ToString();

            if (userType == "admin")
            {
                con.Close();
                return true;
            }

            con.Close();
            return false;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            this.Hide();                                //hides the login screen
            FormRegister formRegister = new FormRegister();         //variable for the FormRegister to be opened after register
            formRegister.Closed += (s, args) => this.Close(); //closes FormRegister
            formRegister.Show();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
    }
}