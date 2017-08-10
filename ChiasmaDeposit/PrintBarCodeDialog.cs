using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Molmed.ChiasmaDep.IO;
using Molmed.ChiasmaDep.Data;

namespace Molmed.ChiasmaDep.Dialog
{
    public partial class PrintBarCodeDialog : Form
    {
        private const String TEXT_COLUMN_NAME = "Text";
        public PrintBarCodeDialog()
        {
            InitializeComponent();
            Init();
        }

        private void AdvancedButton_Click(object sender, EventArgs e)
        {
            try
            {
                AdvancedPanel.Visible = !AdvancedPanel.Visible;
                if (AdvancedPanel.Visible)
                {
                    AdvancedButton.Text = "Advanced <<";
                    PrintGroupBox.Height += AdvancedPanel.Height;
                    CloseButton.Location = new Point(CloseButton.Location.X, CloseButton.Location.Y + AdvancedPanel.Height);
                    this.Height += AdvancedPanel.Height;

                }
                else
                {
                    AdvancedButton.Text = "Advanced >>";
                    PrintGroupBox.Height -= AdvancedPanel.Height;
                    CloseButton.Location = new Point(CloseButton.Location.X, CloseButton.Location.Y - AdvancedPanel.Height);
                    this.Height -= AdvancedPanel.Height;
                }
            }
            catch (Exception exception)
            {
                HandleError("Error during handling of advanced button", exception);
            }
        }

        public static void HandleError(String message, Exception exception)
        {
            ShowErrorDialog errorDialog;

            errorDialog = new ShowErrorDialog(message, exception);
            errorDialog.ShowDialog();
        }

        private Int32 GetBarCodeHeight()
        {
            Int32 barCodeHeight;

            switch (BarCodeHeightComboBox.SelectedItem.ToString())
            {
                case "Short":
                    barCodeHeight = 80;
                    break;
                case "Medium":
                    barCodeHeight = 120;
                    break;
                case "Tall":
                    barCodeHeight = 160;
                    break;
                default:
                    throw new Exception("Unknown bar code height");
            }

            return barCodeHeight;
        }

        private Int32 GetBarCodeWidth()
        {
            Int32 barCodeWidth;

            switch (BarCodeWidthComboBox.SelectedItem.ToString())
            {
                case "Narrow":
                    barCodeWidth = 4;
                    break;
                case "Wide":
                    barCodeWidth = 6;
                    break;
                default:
                    throw new Exception("Unknown bar code width");
            }

            return barCodeWidth;
        }

        private void Init()
        {
            // Hide advanced panel.
            PrintGroupBox.Height -= AdvancedPanel.Height;
            CloseButton.Location = new Point(CloseButton.Location.X, CloseButton.Location.Y - AdvancedPanel.Height);
            this.Height -= AdvancedPanel.Height;

            // Define columns for bar code list view.
            BarCodeListView.Columns.Add("Registered bar codes", (BarCodeListView.Width / 2) - 12);
            BarCodeListView.Columns.Add(TEXT_COLUMN_NAME, (BarCodeListView.Width / 2) - 12);

            // Print bar code properties.
            BarCodeHeightComboBox.SelectedItem = "Tall";
            BarCodeWidthComboBox.SelectedItem = "Narrow";
            OffsetX.Value = LabelPrinter.GetDefaultBarCodeOffsetX();
            OffsetY.Value = LabelPrinter.GetDefaultBarCodeOffsetY();
            BarCodeTypeComboBox.SelectedIndex = 0;
            PrintModeComboBox.SelectedIndex = 0;
            PrintServerNameTextBox.Text = LabelPrinter.GetDefaultPrintServerName();

            IGenericContainer container = null;
            container = ContainerManager.GetUncontainedContainer();
            if (IsNotNull(container))
            {
                BarCodeListView.BeginUpdate();
                BarCodeListView.Items.Add(new BarCodeListViewItem(container));
                BarCodeListView.EndUpdate();
            }
            else
            {
                ShowWarning("Could not find the Uncontained object!");
            }
            PrintButton.Enabled = IsNotEmpty(BarCodeListView.Items);

        }


        protected static Boolean IsNotNull(Object testObject)
        {
            return (testObject != null);
        }

        protected static Boolean IsEmpty(ICollection collection)
        {
            return ((collection == null) || (collection.Count == 0));
        }

        protected static Boolean IsEmpty(String testString)
        {
            return (testString == null) || (testString.Trim().Length == 0);
        }

        protected static Boolean IsNotEmpty(ICollection collection)
        {
            return ((collection != null) && (collection.Count > 0));
        }

        protected static Boolean IsNotEmpty(String testString)
        {
            return (testString != null) && (testString.Trim().Length > 0);
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            Int32 index, offsetX, offsetY;
            LabelPrinter labelPrinter;
            LabelPrinter.BarCodeLabel[] barCodeLabels;
            LabelPrinter.BarCodeTextPositions textPosition;

            try
            {
                barCodeLabels = new LabelPrinter.BarCodeLabel[BarCodeListView.Items.Count];
                for (index = 0; index < BarCodeListView.Items.Count; index++)
                {
                    barCodeLabels[index] = ((BarCodeListViewItem)(BarCodeListView.Items[index])).GetBarCodeLabel();
                }

                offsetX = (Int32)(OffsetX.Value);
                offsetY = (Int32)(OffsetY.Value);

                if (TextAboveRadioButton.Checked)
                {
                    textPosition = LabelPrinter.BarCodeTextPositions.Above;
                }
                else
                {
                    // TextBesideRadioButton is checked.
                    textPosition = LabelPrinter.BarCodeTextPositions.Right;
                }

                labelPrinter = new LabelPrinter(PrintServerNameTextBox.Text);
                labelPrinter.Print(barCodeLabels, GetBarCodeHeight(), GetBarCodeWidth(), offsetX, offsetY, textPosition);
            }
            catch (Exception exception)
            {
                HandleError("Error when attempting to print bar codes", exception);
            }
        }

        protected void ShowWarning(String message)
        {
            MessageBox.Show(message,
                           Config.GetDialogTitleStandard(),
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Exclamation);
        }
        private class BarCodeListViewItem : ListViewItem
        {
            private String MyBarCode;
            private String MyText;

            public BarCodeListViewItem(IGenericContainer container)
                : base(container.GetBarCode())
            {
                MyBarCode = container.GetBarCode();
                MyText = container.GetIdentifier();
                this.SubItems.Add(MyText);
            }
            public LabelPrinter.BarCodeLabel GetBarCodeLabel()
            {
                return new LabelPrinter.BarCodeLabel(MyBarCode, MyText);
            }           
        }
    }
}