using System;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using ChiasmaDeposit.UI.LoadSampleStorageDialogs;
using Molmed.ChiasmaDep;
using Molmed.ChiasmaDep.Data;
using Molmed.ChiasmaDep.Data.Exception;
using Timer = System.Timers.Timer;

namespace ChiasmaDeposit.UI.SampleListDialogs
{
    public partial class SampleListDialog : Form
    {
        private IGenericContainer _putInContainer;
        private readonly BarCodeController _barCodeController;
        private Timer _resetTimer = new Timer(200);

        public SampleListDialog()
        {
            InitializeComponent();
            _putInContainer = null;
            _barCodeController = null;
            _resetTimer.Elapsed += ResetTimer_Elapsed;
            InitListView();
        }

        public SampleListDialog(IGenericContainer container)
        {
            InitializeComponent();
            InitListView();
            _resetTimer.Elapsed += ResetTimer_Elapsed;
            _barCodeController = new BarCodeController(this);
            _barCodeController.BarCodeReceived += BarCodeReceived;
            if (LoadSampleStorageDuoDialog.IsSampleContainer(container))
            {
                InitWithSampleContainer(container);
            }
            else if (LoadSampleStorageDuoDialog.IsStorageContainer(container))
            {
                InitWithPutInContainer(container);
            }
            else
            {
                throw new DataException("This container neither represent a sample container nor a deposit");            
            }
        }

        private void ResetTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _resetTimer.Enabled = false;
            _barCodeController.Reset();
        }

        private void AddListviewItem(ContainerToBePlacedViewItem item)
        {
            SampleContainerListView.Items.Add(item);
        }

        private void EnableOkButton()
        {
            OkButton.Enabled = true;
        }

        private void UpdateContainerText(string container)
        {
            PutInContainerTextBox.Text = container;
        }

        private void HandleReceivedBarCode(String barCode)
        {
            var container = GenericContainerManager.GetGenericContainerByBarCode(barCode);
            if (container == null)
            {
                _resetTimer.Enabled = true;
                return;
            }
            if (LoadSampleStorageDuoDialog.IsSampleContainer(container))
            {
                AddListviewItem(new ContainerToBePlacedViewItem(container));
                if (_putInContainer != null)
                {
                    EnableOkButton();
                }
            }
            else if (LoadSampleStorageDuoDialog.IsStorageContainer(container))
            {
                _putInContainer = container;
                UpdateContainerText(container.GetIdentifier());
                DialogResult = DialogResult.OK;
            }
            else
            {
                throw new DataException("This container neither represent a sample container nor a deposit");
            }
        }

        private void BarCodeReceived(String barCode)
        {
            try
            {
                HandleReceivedBarCode(barCode);
            }
            catch (BarCodeException ex)
            {
                ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                LoadSampleStorageDuoDialog.HandleError(ex.Message, ex);
            }
            
        }


        private void ShowWarning(String message)
        {
            MessageBox.Show(message,
                           Config.GetDialogTitleStandard(),
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Exclamation);
        }


        private void InitWithSampleContainer(IGenericContainer container)
        {
            SampleContainerListView.BeginUpdate();
            SampleContainerListView.Items.Add(new ContainerToBePlacedViewItem(container));
            SampleContainerListView.EndUpdate();
            _putInContainer = null;
            StatusLabel.Text = "Waiting for more scanned containers (plates/tubes/etc). \nClose and go to next step by scan a deposit (Box/Shelf/Freezer/etc)";
            OkButton.Enabled = false;
        }

        public IGenericContainer GetSelectedDeposit()
        {
            return _putInContainer;
        }

        public GenericContainerList GetSelectedContainers()
        {
            GenericContainerList containers = new GenericContainerList();
            foreach (ContainerToBePlacedViewItem viewItem in SampleContainerListView.Items)
            {
                containers.Add(viewItem.GetContainerToBePlaced());
            }
            return containers;
        }

        private void InitWithPutInContainer(IGenericContainer container)
        {
            StatusLabel.Text = "Waiting for scanned containers (plates/tubes/etc) to be placed in the deposit below. \nClose and go to next step by pressing OK or cancel.";
            PutInContainerTextBox.Text = container.GetIdentifier();
            _putInContainer = container;
            OkButton.Enabled = false;
        }

        private void InitListView()
        {
            SampleContainerListView.Columns.Add("Container to be placed", -2);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void MyCancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
