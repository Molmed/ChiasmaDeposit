using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Molmed.ChiasmaDep.Data
{
    public delegate void BarCodeEventHandler(String barCode);

    class BarCodeController : ChiasmaDepBase
    {
        private String MyBarCodeString;
        private Boolean MyBarCodeFlag;
        private DateTime MyBarCodeReadTime;
        private Form MyForm;

        public event BarCodeEventHandler BarCodeReceived;

        public BarCodeController(Form form)
            : base()
        {
            MyForm = form;
            MyBarCodeFlag = false;
            MyBarCodeString = null;
            MyForm.KeyPreview = true;
            MyForm.KeyDown += new KeyEventHandler(Form_KeyDown);
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            TimeSpan elapsedTime;

            if (MyBarCodeFlag)
            {
                // Check how long it was since the bar code reading began.
                elapsedTime = DateTime.Now.Subtract(MyBarCodeReadTime);
                // If it was more than two secondes ago, it is probably a manual input
                // and should not be regarded as a bar code reading.
                if (elapsedTime.Seconds > BarCodeManager.GetMaxTimeToRead())
                {
                    MyBarCodeFlag = false;
                }
            }

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
                    if (!MyBarCodeFlag)
                    {
                        // Start saving digits.
                        MyBarCodeFlag = true;
                        MyBarCodeReadTime = DateTime.Now;
                        MyBarCodeString = "";
                    }
                    // Add digit.
                    MyBarCodeString += e.KeyCode.ToString().Substring(1);
                    break;

                case Keys.Enter:
                    if (MyBarCodeFlag)
                    {
                        // Fire bar code received event.
                        if (IsNotNull(BarCodeReceived))
                        {
                            BarCodeReceived(MyBarCodeString);
                        }
                    }
                    MyBarCodeFlag = false;
                    break;
                default:
                    MyBarCodeFlag = false;
                    break;
            }
        }
    }
}
