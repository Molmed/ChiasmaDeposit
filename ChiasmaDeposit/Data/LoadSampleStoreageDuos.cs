using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Molmed.ChiasmaDep.Data.Exception;

namespace Molmed.ChiasmaDep.Data
{
    public interface ISampleStorageDuo
    {
        String GetContainerPath();
        string GetSampleContainerName();
        IGenericContainer GetSampleContainer();
        string GetStorageContainerName();
        IGenericContainer GetStorageContainer();
        void SetStorageContainer(IGenericContainer storageContainer);
        bool IsTerminated();
        bool IsComplete();
        bool IsChecked();
        bool IsFaulty();
        bool IsGood();
        int GetIndex();
        string GetStatusString();
        void SetCompleted();
        void SetChecked(bool isChecked);
        void SetTerminated();
    }

    public class LoadSampleStoreageDuos : ChiasmaDepData
    {
        private enum LoadStatus
        { 
            WaitingForSample,
            WaitingForStorage
        }
        private SampleStorageDuoList MySampleStorageDuos;
        private LoadStatus MyLoadStatus;
        public LoadSampleStoreageDuos()
        {
            MySampleStorageDuos = new SampleStorageDuoList();
            MyLoadStatus = LoadStatus.WaitingForSample;
        }

        private bool CheckContainerType(IGenericContainer genericContainer)
        {
            if (IsNull(genericContainer) || 
                !(IsSampleContainer(genericContainer) || IsStorageContainer(genericContainer)))
            {
                return false;
            }
            return true;
        }

        public int Length()
        {
            return MySampleStorageDuos.Count;
        }

        public ISampleStorageDuo GetSampleStorageDuo(int index)
        {
            if (index > MySampleStorageDuos.Count - 1 || index < 0)
            {
                return null;
            }
            return (ISampleStorageDuo)MySampleStorageDuos[index];
        }

        private bool IsSampleContainer(IGenericContainer genericContainer)
        {
            if (genericContainer.GetContainerType() == ContainerType.FlowCell ||
               genericContainer.GetContainerType() == ContainerType.BeadChip ||
               genericContainer.GetContainerType() == ContainerType.Plate ||
               genericContainer.GetContainerType() == ContainerType.Tube ||
               genericContainer.GetContainerType() == ContainerType.Box)
            {
                return true;
            }
            return false;
        }

        private bool IsStorageContainer(IGenericContainer genericContainer)
        { 
            if(genericContainer.GetContainerType() == ContainerType.Box ||
               genericContainer.GetContainerType() == ContainerType.Building ||
               genericContainer.GetContainerType() == ContainerType.Floor ||
               genericContainer.GetContainerType() == ContainerType.Freezer ||
               genericContainer.GetContainerType() == ContainerType.Refrigerator ||
               genericContainer.GetContainerType() == ContainerType.Room ||
               genericContainer.GetContainerType() == ContainerType.Shelf ||
               genericContainer.GetContainerType() == ContainerType.TopLevel ||
               genericContainer.GetContainerType() == ContainerType.Uncontained)
            {
                return true;
            }
            return false;
        }

        public GenericContainerList CheckForDoublets()
        { 
            // Check if the same sample appears twice among the 
            // IsGood duos
            // Gather sampleContainers that appear twice in a list
            GenericContainerList doublets = new GenericContainerList();
            for (int i = 0; i < MySampleStorageDuos.Count; i++)
            {
                for (int j = i + 1; j < MySampleStorageDuos.Count; j++)
                {
                    if (MySampleStorageDuos[i].IsGood() &&
                        MySampleStorageDuos[i].IsChecked() &&
                        MySampleStorageDuos[j].IsGood() &&
                        MySampleStorageDuos[j].IsChecked() &&
                        MySampleStorageDuos[i].GetSampleContainer().GetId() ==
                        MySampleStorageDuos[j].GetSampleContainer().GetId() &&
                        IsNull(doublets.GetById(MySampleStorageDuos[i].GetSampleContainer().GetId())))
                    {
                        doublets.Add(MySampleStorageDuos[i].GetSampleContainer());
                    }
                }
            }
            return doublets;
        }

        public void HandleRecievedBarCode(string barCode)
        { 
            
        }

        public void HandleReceivedBarCode_old(string barCode)
        {
            IGenericContainer genericContainer;
            genericContainer = GenericContainerManager.GetGenericContainerByBarCode(barCode);
            ISampleStorageDuo sampleStoreageDuo;
            // Load a SampleContainer or a StorageContainer depending on status
            // Update MySampleStorageDuos
            // Error handling if user try to load another container type than what's expected
            if (!CheckContainerType(genericContainer))
            {
                throw new DataException("This bar code neither represent a sample container nor a deposit");
            }
            switch (MyLoadStatus)
            { 
                case LoadStatus.WaitingForSample:
                    if(IsSampleContainer(genericContainer))
                    {
                        MySampleStorageDuos.Add(new SampleStoreageDuo(genericContainer, MySampleStorageDuos.Count));
                        MyLoadStatus = LoadStatus.WaitingForStorage;
                    }
                    else if (IsStorageContainer(genericContainer))
                    { 
                        // Create a new duo and load it with the storageCOntianer
                        // terminate it, so that the read can be logged
                        sampleStoreageDuo = new SampleStoreageDuo(MySampleStorageDuos.Count);
                        sampleStoreageDuo.SetStorageContainer(genericContainer);
                        sampleStoreageDuo.SetTerminated();
                        MySampleStorageDuos.Add(sampleStoreageDuo);
                        MyLoadStatus = LoadStatus.WaitingForSample;
                    }
                    break;
                case LoadStatus.WaitingForStorage:
                    sampleStoreageDuo = MySampleStorageDuos[MySampleStorageDuos.Count - 1];
                    if (IsStorageContainer(genericContainer))
                    {
                        sampleStoreageDuo.SetStorageContainer(genericContainer);
                        MyLoadStatus = LoadStatus.WaitingForSample;
                    }
                    else if(IsSampleContainer(genericContainer))
                    { 
                        // Make the last duo terminated (failed)
                        // Create a new duo
                        sampleStoreageDuo.SetTerminated();
                        MySampleStorageDuos.Add(new SampleStoreageDuo(genericContainer, MySampleStorageDuos.Count));
                        MyLoadStatus = LoadStatus.WaitingForStorage;
                    }
                    break;
            }
        }
        private class SampleStoreageDuo : ChiasmaDepData, ISampleStorageDuo
        {
            IGenericContainer MySampleContainer;
            IGenericContainer MyStorageContainer;
            String MyContainerPath;
            bool MyIsTerminated;
            bool MyIsComplete;
            bool MyIsChecked;
            string MyStatus;
            int MyIndex;

            public SampleStoreageDuo(int index)
            {
                MySampleContainer = null;
                MyStorageContainer = null;
                MyIsTerminated = false;
                MyIsComplete = false;
                MyStatus = "Waiting for Sample Container";
                MyIndex = index;
                MyIsChecked = true;
                MyContainerPath = "";
            }

            public SampleStoreageDuo(IGenericContainer sampleContainer, int index)
            {
                MySampleContainer = sampleContainer;
                MyStorageContainer = null;
                MyIsTerminated = false;
                MyIsComplete = false;
                MyStatus = "Waiting for Storage id";
                MyIndex = index;
                MyIsChecked = true;
                MyContainerPath = "";
            }

            public SampleStoreageDuo(IGenericContainer deposit, IGenericContainer container)
            {
                MySampleContainer = container;
                MyStorageContainer = deposit;
                MyIsTerminated = true;
                MyIsComplete = true;
                MyIsChecked = true;
                MyContainerPath = "";
            }

            public String GetContainerPath()
            {
                LoadContainerPath();
                return MyContainerPath;
            }

            private void LoadContainerPath()
            {
                if (IsNotNull(MyStorageContainer) && MyContainerPath == "")
                {
                    GenericContainerList pathList;
                    StringBuilder pathRow = new StringBuilder();
                    pathList = MyStorageContainer.GetContainerPath();
                    foreach (IGenericContainer singleContainer in pathList)
                    {
                        pathRow.Append("//");
                        pathRow.Append(singleContainer.GetIdentifier());
                    }
                    pathRow.Append("//");
                    pathRow.Append(GetStorageContainerName());
                    MyContainerPath = pathRow.ToString();
                }
            }

            public bool IsChecked()
            {
                return MyIsChecked;
            }

            public bool IsFaulty()
            {
                return MyIsTerminated && !MyIsComplete;
            }

            public bool IsGood()
            {
                return MyIsTerminated && MyIsComplete;
            }

            public bool IsTerminated()
            {
                return MyIsTerminated;
            }

            public bool IsComplete()
            {
                return MyIsComplete;
            }

            public int GetIndex()
            {
                return MyIndex;
            }

            public IGenericContainer GetSampleContainer()
            {
                return MySampleContainer;
            }

            public string GetSampleContainerName()
            {
                string name;
                name = "";
                if (IsNotNull(MySampleContainer))
                {
                    name = MySampleContainer.GetIdentifier();
                }
                return name;
            }

            public IGenericContainer GetStorageContainer()
            {
                return MyStorageContainer;
            }

            public string GetStorageContainerName()
            {
                string name;
                name = "";
                if (IsNotNull(MyStorageContainer))
                {
                    name = MyStorageContainer.GetIdentifier();
                }
                return name;
            }

            public void SetChecked(bool isChecked)
            {
                MyIsChecked = isChecked;
            }

            public void SetCompleted()
            {
                MyIsComplete = true;
                MyStatus = "Completed";
            }

            public void SetSampleContainer(IGenericContainer sampleContainer)
            {
                MySampleContainer = sampleContainer;
            }

            public void SetStorageContainer(IGenericContainer storageContainer)
            {
                MyStorageContainer = storageContainer;
                if (IsNotNull(MySampleContainer))
                {
                    SetCompleted();
                    SetTerminated();
                }
            }

            public string GetStatusString()
            {
                return MyStatus;
            }

            public void SetTerminated()
            {
                MyIsTerminated = true;
                if (!MyIsComplete)
                {
                    MyStatus = "Ignored";
                }
            }
        }
    }

    public class SampleStorageDuoList : ArrayList
    {
        public new ISampleStorageDuo this[Int32 index]
        {
            get
            {
                return (ISampleStorageDuo)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }

}
