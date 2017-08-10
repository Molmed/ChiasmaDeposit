using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Data
{

    public abstract class TubeRackForTags : TubeRack
    {
        public enum ShowMode
        {
            ShowTags,
            ShowContentsAndTags
        }
        protected ShowMode MyShowMode;

        public TubeRackForTags(DataReader reader, ShowMode showMode)
            : base(reader)
        {
            MyShowMode = showMode;
        }

    }

    public class TubeRackForTagSource : TubeRackForTags
    {
        public TubeRackForTagSource(DataReader reader)
            : base(reader, ShowMode.ShowTags)
        {
        }

        public override TubeRackUsage GetTubeRackUsage()
        {
            return TubeRackUsage.TagSourceTubeRack;
        }

    }

}
