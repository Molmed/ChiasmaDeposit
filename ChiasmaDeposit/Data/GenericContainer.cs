using System;
using System.Collections.Generic;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Data
{
    public enum ContainerType
    {
        BeadChip,
        Box,
        Building,
        Floor,
        Freezer,
        Plate,
        Refrigerator,
        Room,
        Shelf,
        TopLevel,
        Tube,
        FlowCell,
        Uncontained,
        TubeRack
    }

    public enum ContainerStatus
    {
        Active,
        Disposed,
        Returned
    }

    public interface IGenericContainer : IDataBarCode, IDataComment, IDataIdentity
    {
        ContainerList GetContainerPath();
        ContainerType GetContainerType();
        GenericContainerList GetContents();
        GenericContainerList GetContents(String genericContainerIdentifierFilter);
        Boolean IsBeadChip();
        Boolean IsContainer();
        Boolean IsDisposed();
        Boolean IsFlowCell();
        Boolean IsLabContainer();
        Boolean IsPlate();
        Boolean IsReturned();
        Boolean IsTopLevel();
        Boolean IsTube();
    }

    public class GenericContainer : DataBarCode, IGenericContainer
    {
        private ContainerStatus MyStatus;
        private ContainerType MyType;

        public GenericContainer(DataReader dataReader, ContainerType containerType)
            : base(dataReader)
        {
            MyStatus = (ContainerStatus)(Enum.Parse(typeof(ContainerStatus), dataReader.GetString(ContainerData.STATUS)));
            MyType = containerType;
        }

        public GenericContainerList GetContents()
        {
            return GetContents("%");
        }

        public virtual GenericContainerList GetContents(String genericContainerIdentifierFilter)
        {
            // Default which is used by lab containers is to return nothing. 
            return null;
        }

        public ContainerList GetContainerPath()
        {
            ContainerList containers = null;
            DataReader dataReader = null;

            try
            {
                dataReader = Database.GetContainerPath(GetId());
                containers = new ContainerList();
                while (dataReader.Read())
                {
                    containers.Add(ContainerManager.GetContainer(dataReader));
                }
            }
            finally
            {
                CloseDataReader(dataReader);
            }
            return containers;
        }

        public ContainerStatus GetContainerStatus()
        {
            return MyStatus;
        }

        public ContainerType GetContainerType()
        {
            return MyType;
        }

        public override DataType GetDataType()
        {
            return DataType.Container;
        }

        public static Int32 GetIdentifierMaxLength()
        {
            return GetColumnLength(ContainerData.TABLE, ContainerData.IDENTIFIER);
        }

        public Boolean IsActive()
        {
            return MyStatus == ContainerStatus.Active;
        }

        public Boolean IsBeadChip()
        {
            return GetContainerType() == ContainerType.BeadChip;
        }

        public Boolean IsContainer()
        {
            return !IsLabContainer();
        }

        public Boolean IsDisposed()
        {
            return MyStatus == ContainerStatus.Disposed;
        }

        public Boolean IsFlowCell()
        {
            return GetContainerType() == ContainerType.FlowCell;
        }

        public Boolean IsPlate()
        {
            return GetContainerType() == ContainerType.Plate;
        }

        public Boolean IsLabContainer()
        {
            return IsBeadChip() || IsPlate() || IsTube() || IsFlowCell();
        }

        public Boolean IsReturned()
        {
            return MyStatus == ContainerStatus.Returned;
        }

        public Boolean IsTopLevel()
        {
            return GetContainerType() == ContainerType.TopLevel;
        }

        public Boolean IsTube()
        {
            return GetContainerType() == ContainerType.Tube;
        }

    }

    public class GenericContainerList : DataIdentityList
    {

        public IGenericContainer GetByBarCode(String barCode)
        {
            // Check parameter.
            if (barCode == null)
            {
                return null;
            }

            // This search is case insensitive.
            barCode = barCode.ToLower();
            foreach (IGenericContainer dataBarCode in this)
            {
                if (dataBarCode.HasBarCode() &&
                     (dataBarCode.GetBarCode().ToLower() == barCode))
                {
                    return dataBarCode;
                }
            }
            return null;
        }

        public new IGenericContainer GetById(Int32 id)
        {
            return (IGenericContainer)(base.GetById(id));
        }

        public new IGenericContainer this[Int32 Index]
        {
            get
            {
                return (IGenericContainer)(base[Index]);
            }
            set
            {
                base[Index] = value;
            }
        }

        public new IGenericContainer this[String identifier]
        {
            get
            {
                return (IGenericContainer)(base[identifier]);
            }
        }
    }
}
