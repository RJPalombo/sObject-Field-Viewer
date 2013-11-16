using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sObjectFieldViewer
{
    public partial class login : Form
    {
        private string username;
        private string password;
        private bool sandbox;

        public string getUsername()
        {
            return username;
        }
        public void setUsername(string _username)
        {
            username = _username;
        }

        public string getPassword()
        {
            return password;
        }
        public void setPassword(string _password)
        {
            password = _password;
        }

        public bool getSandbox()
        {
            return sandbox;
        }
        public void setSandbox(bool _sandbox)
        {
            sandbox = _sandbox;
        }

        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            setUsername(username_txt.Text);
            setPassword(password_txt.Text);
            setSandbox(sandbox_chk.Checked);
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {

        }

    }
}
