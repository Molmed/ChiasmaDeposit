using System.Windows.Forms;
using Molmed.ChiasmaDep.Data;

namespace ChiasmaDeposit.UI.SampleListDialogs
{
    public class ContainerToBePlacedViewItem : ListViewItem
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
}