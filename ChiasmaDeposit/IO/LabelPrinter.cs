using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ChiasmaDeposit.Properties;

namespace Molmed.ChiasmaDep.IO
{
    public class LabelPrinter
    {
        private const Int32 DEFAULT_BAR_CODE_FONT_HEIGHT = 160;
        private const Int32 DEFAULT_BAR_CODE_FONT_WIDTH = 4;
        private const Int32 DEFAULT_BAR_CODE_OFFSET_X = 80;
        private const Int32 DEFAULT_BAR_CODE_OFFSET_Y = 20;
        private const String DEFAULT_FONT = "B";
        private const Int32 DEFAULT_LABEL_OFFSET_X = 80;
        private const Int32 DEFAULT_LABEL_OFFSET_Y = 80;

        private String MyPrintServerName;

        public enum BarCodeTextPositions
        {
            Above,
            Right
        }

        public LabelPrinter()
            : this(LabelPrinter.GetDefaultPrintServerName())
        {
        }

        public LabelPrinter(String printServerName)
        {
            MyPrintServerName = printServerName;
        }

        public static Int32 GetDefaultBarCodeOffsetX()
        {
            return DEFAULT_BAR_CODE_OFFSET_X;
        }

        public static Int32 GetDefaultBarCodeOffsetY()
        {
            return DEFAULT_BAR_CODE_OFFSET_Y;
        }

        public static String GetDefaultFont()
        {
            return DEFAULT_FONT;
        }

        public static Int32 GetDefaultLabelOffsetX()
        {
            return DEFAULT_LABEL_OFFSET_X;
        }

        public static Int32 GetDefaultLabelOffsetY()
        {
            return DEFAULT_LABEL_OFFSET_Y;
        }

        public static String GetDefaultPrintServerName()
        {
            return Settings.Default.LabelPrintServer;
        }

        public void Print(BarCodeLabel[] barCodeLabels)
        {
            Print(barCodeLabels,
                       DEFAULT_BAR_CODE_FONT_HEIGHT,
                       DEFAULT_BAR_CODE_FONT_WIDTH,
                       LabelPrinter.GetDefaultBarCodeOffsetX(),
                       LabelPrinter.GetDefaultBarCodeOffsetY(),
                       BarCodeTextPositions.Right);
        }

        private void Print(String labelScriptFile)
        {
            Process printProcess;
            ProcessStartInfo startInfo;

            // Use ftp to send file to printer.
            startInfo = new ProcessStartInfo("ftp");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;

            printProcess = Process.Start(startInfo);
            printProcess.StandardInput.WriteLine("open");
            printProcess.StandardInput.WriteLine(MyPrintServerName);
            printProcess.StandardInput.WriteLine("");
            printProcess.StandardInput.WriteLine("put");
            printProcess.StandardInput.WriteLine("\"" + labelScriptFile + "\"");
            printProcess.StandardInput.WriteLine("");
            printProcess.StandardInput.WriteLine("bye");
            printProcess.WaitForExit(5000);
        }

        public void Print(String text,
                                            String fontName,
                                            Int32 magnificationFactor,
                                            Int32 offsetX,
                                            Int32 offsetY,
                                            Int32 copies)
        {
            String labelScriptFile;
            ZPLGenerator ZPLGen;

            try
            {
                ZPLGen = new ZPLGenerator();
                labelScriptFile = Environment.GetEnvironmentVariable("TEMP") + "\\Chi_ZPL.ZPL";

                // Write the ZPL script to the temporary location on the local disc.
                ZPLGen.GenerateAlphaNumeric(labelScriptFile, text, fontName, magnificationFactor, offsetX, offsetY, copies);

                // Print file.
                Print(labelScriptFile);

                // Delete script file.
                File.Delete(labelScriptFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when attempting to print label: " + ex.Message, Config.GetDialogTitleStandard(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Print(BarCodeLabel[] barCodeLabels,
                                            Int32 fontHeight,
                                            Int32 fontWidth,
                                            Int32 offsetX,
                                            Int32 offsetY,
                                            BarCodeTextPositions textPosition)
        {
            String labelScriptFile;
            ZPLGenerator ZPLGen;

            try
            {
                ZPLGen = new ZPLGenerator();
                labelScriptFile = Environment.GetEnvironmentVariable("TEMP") + "\\Chi_ZPL.ZPL";

                // Check that all bar codes are valid.
                foreach (BarCodeLabel barCodeLabel in barCodeLabels)
                {
                    if (!ZPLGen.ValidateBarCode128c(barCodeLabel.BarCode))
                    {
                        MessageBox.Show("The bar code " + barCodeLabel.BarCode + " is not a valid Code128c bar code.", Config.GetDialogTitleStandard());
                        return;
                    }
                }

                // Write the ZPL script to the temporary location on the local disc.
                ZPLGen.GenerateCode128c(labelScriptFile, barCodeLabels, fontHeight, fontWidth, offsetX, offsetY, textPosition);

                // Print file.
                Print(labelScriptFile);

                // Delete script file.
                File.Delete(labelScriptFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when attempting to print bar codes: " + ex.Message, Config.GetDialogTitleStandard(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public class BarCodeLabel
        {
            private String MyBarCode;
            private String MyText;

            public BarCodeLabel()
                : this("", "")
            {
            }

            public BarCodeLabel(String barCode, String text)
            {
                MyBarCode = barCode;
                MyText = text;
            }

            public String BarCode
            {
                get
                {
                    return MyBarCode;
                }
                set
                {
                    MyBarCode = value;
                }
            }

            public String Text
            {
                get
                {
                    return MyText;
                }
                set
                {
                    MyText = value;
                }
            }
        }
    }
}
