using System;
using System.Text;
using System.Windows.Forms;
using Molmed.ChiasmaDep.Data;

namespace ChiasmaDeposit.UI.LoadSampleStorageDialogs
{
    public class DuoViewItem : ListViewItem
    {
        private readonly IGenericContainer _deposit;
        private readonly IGenericContainer _sampleContainer;
        private string _containerPath;

        public DuoViewItem(IGenericContainer deposit, IGenericContainer container)
            : base(container.GetIdentifier())
        {
            _deposit = deposit;
            _sampleContainer = container;
            _containerPath = "";
            SubItems.Add(deposit.GetIdentifier());
            Checked = true;
        }

        public IGenericContainer GetSampleContainer()
        {
            return _sampleContainer;
        }

        public String GetContainerPath()
        {
            LoadContainerPath();
            return _containerPath;
        }

        private void LoadContainerPath()
        {
            if (_deposit != null && _containerPath == "")
            {
                StringBuilder pathRow = new StringBuilder();
                GenericContainerList pathList = _deposit.GetContainerPath();
                foreach (IGenericContainer singleContainer in pathList)
                {
                    pathRow.Append("//");
                    pathRow.Append(singleContainer.GetIdentifier());
                }
                pathRow.Append("//");
                pathRow.Append(_deposit.GetIdentifier());
                _containerPath = pathRow.ToString();
            }
        }

        public IGenericContainer GetStorageContainer()
        {
            return _deposit;
        }
    }
}