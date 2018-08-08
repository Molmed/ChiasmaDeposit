using System;
using System.Windows.Forms;
using Molmed.ChiasmaDep.Data;

namespace ChiasmaDeposit.UI
{
    public partial class DoubletsDialog : Form
    {
        public DoubletsDialog(GenericContainerList doublets)
        {
            InitializeComponent();
            foreach (IGenericContainer gcont in doublets)
            {
                DoubletsListBox.Items.Add(gcont.GetIdentifier());
            }
        }

        public DoubletsDialog(GenericContainerList doublets, string message)
        {
            InitializeComponent();
            foreach (IGenericContainer gcont in doublets)
            {
                DoubletsListBox.Items.Add(gcont.GetIdentifier());
            }
            PleaseLabel2.Text = message;
            PleaseLabel.Visible = false;
            AbortLabel.Visible = false;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}