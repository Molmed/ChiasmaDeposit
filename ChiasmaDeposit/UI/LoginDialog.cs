using System;
using System.Windows.Forms;
using Molmed.ChiasmaDep;

namespace ChiasmaDeposit.UI
{
    public partial class LoginDialog : Form
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        public String GetPassword()
        {
            return Password.Text.Trim();
        }

        public String GetUserName()
        {
            return UserName.Text.Trim();
        }

        protected static Boolean IsEmpty(String testString)
        {
            return (testString == null) || (testString.Trim().Length == 0);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (IsEmpty(GetUserName()))
            {
                ShowWarning("Please specify a user name.");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        public void SetLoginInformation(String userName, String password)
        {
            // Request for automatic login with bar code.
            UserName.Text = userName;
            Password.Text = password;
            DialogResult = DialogResult.OK;
        }
        protected void ShowWarning(String message)
        {
            MessageBox.Show(message,
                           Config.GetDialogTitleStandard(),
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Exclamation);
        }

    }
}