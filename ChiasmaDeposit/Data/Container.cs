using System;
using System.Collections.Generic;
using Molmed.ChiasmaDep.Data.Exception;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Data
{
    public interface IContainer : IGenericContainer
    {
    }

    public class Container : GenericContainer, IContainer
    {
        public Container(DataReader dataReader, ContainerType containerType)
            : base(dataReader, containerType)
        {
        }


        public static Int32 GetCommentMaxLength()
        {
            return GetColumnLength(ContainerData.TABLE, ContainerData.COMMENT);
        }

        //public override GenericContainerList GetContents(String containerIdentifierFilter)
        //{
        //    DataReader dataReader = null;

        //    try
        //    {
        //        dataReader = Database.GetGenericContainerContents(GetId(), containerIdentifierFilter);
        //        return GenericContainerManager.GetGenericContainers(dataReader);
        //    }
        //    finally
        //    {
        //        CloseDataReader(dataReader);
        //    }
        //}
    }

    public class ContainerList : GenericContainerList
    {
        public new IContainer GetById(Int32 id)
        {
            return (IContainer)(base.GetById(id));
        }

        public new IContainer this[Int32 Index]
        {
            get
            {
                return (IContainer)(base[Index]);
            }
            set
            {
                base[Index] = value;
            }
        }

        public new IContainer this[String identifier]
        {
            get
            {
                return (IContainer)(base[identifier]);
            }
        }
    }
}
