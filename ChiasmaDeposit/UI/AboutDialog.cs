using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ChiasmaDeposit.Data;

namespace ChiasmaDeposit.UI
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
            var versionProvider = new VersionProvider();
            VersionLabel.Text = versionProvider.GetApplicationVersion();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
