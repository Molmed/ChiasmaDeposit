using System;
using System.Windows.Forms;
using Molmed.ChiasmaDep.Data;
using Molmed.ChiasmaDep.Data.Exception;

namespace Molmed.ChiasmaDep.Dialog
{
    public partial class SampleListDialog : Form
    {
        private IGenericContainer _putInContainer;
        private readonly BarCodeController _barCodeController;

        delegate void AddListviewItemCallback(ContainerToBePlacedViewItem item);

        delegate void EnableOkButtonCallback();

        delegate void UpdateContainerTextCallback(string container);

        public SampleListDialog()
        {
            InitializeComponent();
            _putInContainer = null;
            _barCodeController = null;
            InitListView();
        }

        public SampleListDialog(IGenericContainer container)
        {
            InitializeComponent();
            InitListView();
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

        /// <summary>
        /// Fix cross thread operation 
        /// </summary>
        /// <param name="item"></param>
        private void AddListviewItem(ContainerToBePlacedViewItem item)
        {
            if (SampleContainerListView.InvokeRequired)
            {
                var d = new AddListviewItemCallback(AddListviewItem);
                Invoke(d, item);
            }
            else
            {
                SampleContainerListView.Items.Add(item);
            }
        }

        /// <summary>
        /// Fix cross thread operation 
        /// </summary>
        private void EnableOkButton()
        {
            if (OkButton.InvokeRequired)
            {
                var d = new EnableOkButtonCallback(EnableOkButton);
                Invoke(d);
            }
            else
            {
                OkButton.Enabled = true;
            }
        }

        private void UpdateContainerText(string container)
        {
            if (PutInContainerTextBox.InvokeRequired)
            {
                var d = new UpdateContainerTextCallback(UpdateContainerText);
                Invoke(d, container);
            }
            else
            {
                PutInContainerTextBox.Text = container;
            }
        }

        private void HandleReceivedBarCode(String barCode)
        {
            var container = GenericContainerManager.GetGenericContainerByBarCode(barCode);
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

        private class ContainerToBePlacedViewItem : ListViewItem
        {
            private readonly IGenericContainer _container;

            public ContainerToBePlacedViewItem(IGenericContainer container)
                : base(container.GetIdentifier())
            {
                _container = container;
            }

            public IGenericContainer GetContainerToBePlaced()
            {
                return _container;
            }
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
