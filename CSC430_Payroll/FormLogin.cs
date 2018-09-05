using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSC430_Payroll
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e) //when Login button is clicked
        {
            this.Hide();                                //hides the login screen
            FormMain formMain = new FormMain();         //variable for the FormMain to be opened after login
            formMain.Closed += (s, args) => this.Close(); //closes FormLogin
            formMain.Show();                            //displays FormMain
        }
    }
}
