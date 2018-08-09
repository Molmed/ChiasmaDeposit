using System;
using System.Windows.Forms;
using ChiasmaDeposit.Properties;

namespace Molmed.ChiasmaDep.Data
{
    public delegate void BarCodeEventHandler(String barCode);

    public class BarCodeController : ChiasmaDepBase
    {
        private String _barCodeString;
        private Boolean _ongoingBarcodeReading;

        public event BarCodeEventHandler BarCodeReceived;

        public BarCodeController(Form form)
        {
            _ongoingBarcodeReading = false;
            _barCodeString = null;
            form.KeyPreview = true;
            form.KeyDown += Form_KeyDown;
            QuitAtInternalBarcodeLength = true;
        }

        private bool QuitAtInternalBarcodeLength { get; }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    if (!_ongoingBarcodeReading)
                    {
                        // Start saving digits.
                        _ongoingBarcodeReading = true;
                        _barCodeString = "";
                    }
                    // Add digit.
                    _barCodeString += e.KeyCode.ToString().Substring(1);

                    if (QuitAtInternalBarcodeLength &&
                        _barCodeString.Length == Settings.Default.BarCodeLengthInternal &&
                        IsNotNull(BarCodeReceived))
                    {
                        BarCodeReceived?.Invoke(_barCodeString);
                    }

                    break;

                default:
                    _ongoingBarcodeReading = false;
                    break;
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
}
