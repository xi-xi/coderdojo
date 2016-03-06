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
            LogIn(usernameTextbox.Text, passwordTextbox.Text);
        }

        public void LogIn(string username, string password)
        {
            var f = new MainForm();
            f.Show();
        }
    }
}
