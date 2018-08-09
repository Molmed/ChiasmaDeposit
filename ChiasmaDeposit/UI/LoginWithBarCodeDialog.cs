using System;
using System.Windows.Forms;
using Molmed.ChiasmaDep.Data;

namespace ChiasmaDeposit.UI
{
    public partial class LoginWithBarcodeDialog : Form
    {
        private delegate void BarcodeReceivedCallback(string barcode);

        private string _barcode;
        private int _shrinkDistance;

        public LoginWithBarcodeDialog()
        {
            _shrinkDistance = -1;
            InitializeComponent();
            _barcode = "";
            Init();
        }

        private void Init()
        {
            BarcodeTextBox.Select();
            //BarcodeCatcherTextBox.Width = 0;
            var barCodeController = new BarCodeController(this);
            barCodeController.BarCodeReceived += BarCodeReceived;
            var locA = ManualCheckBox.Location;
            var locB = BarcodeTextBox.Location;
            _shrinkDistance = (locB.Y + BarcodeTextBox.Height) - (locA.Y + ManualCheckBox.Height);
            MakeManualLoginInvisible();
        }

        private void MakeManualLoginInvisible()
        {
            BarcodeLabel.Visible = false;
            BarcodeTextBox.Visible = false;
            MyOkButton.Visible = false;
            AcceptButton = null;
            Height -= _shrinkDistance;
            ManualCheckBox.Select();
        }

        private void MakeManualLoginVisible()
        {
            BarcodeLabel.Visible = true;
            BarcodeTextBox.Visible = true;
            MyOkButton.Visible = true;
            AcceptButton = MyOkButton;
            Height += _shrinkDistance;
            BarcodeTextBox.Select();
        }

        private void BarCodeReceived(string barcode)
        {
            if (InvokeRequired)
            {
                BarcodeReceivedCallback c = BarCodeReceived;
                Invoke(c, barcode);
            }
            else
            {
                _barcode = barcode;
                DialogResult = DialogResult.OK;
                Close();
            }

        }

        public string Barcode => _barcode;

        private Boolean IsNotEmpty(String testString)
        {
            return (testString != null) && (testString.Trim().Length > 0);
        }


        private void MyOkButton_Click(object sender, EventArgs e)
        {
            if (IsNotEmpty(BarcodeTextBox.Text.Trim()))
            {
                _barcode = BarcodeTextBox.Text.Trim();
            }
            DialogResult = DialogResult.OK;
        }

        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ManualCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ManualCheckBox.Checked)
            {
                MakeManualLoginVisible();
            }
            else
            {
                MakeManualLoginInvisible();
            }
        }

        private void BarcodeLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
