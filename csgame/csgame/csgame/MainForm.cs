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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.comboBox1.Items.Add("hoge");
            this.comboBox1.Items.Add("fuga");
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            base.OnLoad(e);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var str = (string)comboBox1.SelectedItem;
            throw new NotImplementedException();
        }
    }
}
