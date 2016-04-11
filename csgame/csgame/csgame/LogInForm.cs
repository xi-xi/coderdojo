using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csgame
{
    public partial class LogInForm : Form
    {
        public LogInForm()
        {
            InitializeComponent();
            this.loginButton.Click += OnLoginButtonClicked;
            usernameTextbox.Click += OnTextBoxKeyDown;
            passwordTextbox.Click += OnTextBoxKeyDown;
        }

        private void OnTextBoxKeyDown(object sender, EventArgs e)
        {
            OnLoggingIn();
        }

        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            OnLoggingIn();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                OnLoggingIn();
            }
            base.OnKeyDown(e);
        }

        public void OnLoggingIn()
        {
            var username = usernameTextbox.Text;
            var password = passwordTextbox.Text;
            if(!CheckUser(username, password))
            {
                errorProvider.SetError(passwordLabel, "User Name or Password is invalid");
                return;
            }
            LogIn(username, password);
        }

        private bool CheckUser(string name, string pass)
        {
            return true;
        }

        public void LogIn(string username, string password)
        {
            var f = new MainForm();
            f.Show();
            this.Hide();
            f.FormClosed += MainFormClosed;
        }

        private void MainFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
