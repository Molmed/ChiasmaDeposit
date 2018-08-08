using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ChiasmaDeposit.UI
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
            var version = $"{Assembly.GetExecutingAssembly().GetName().Version.Major}." +
                          $"{Assembly.GetExecutingAssembly().GetName().Version.Minor}." +
                          $"{Assembly.GetExecutingAssembly().GetName().Version.Build}";
            VersionLabel.Text = version;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
