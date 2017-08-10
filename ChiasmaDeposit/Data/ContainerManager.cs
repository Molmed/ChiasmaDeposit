using System;
using System.Collections.Generic;
using Molmed.ChiasmaDep.Data.Exception;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Data
{
    public class ContainerManager : ChiasmaDepData
    {
        private ContainerManager()
            : base()
        {
        }

        private static void CheckContainerType(ContainerType containerType)
        {
            switch (containerType)
            {
                case ContainerType.Box:
                case ContainerType.Building:
                case ContainerType.Floor:
                case ContainerType.Freezer:
                case ContainerType.Refrigerator:
                case ContainerType.Room:
                case ContainerType.Shelf:
                case ContainerType.TopLevel:
                case ContainerType.Uncontained:
                    // OK to create container.
                    break;

                default:
                    throw new DataArgumentException("Not supported container type " +
                                                 containerType.ToString(),
                                                 "containerType");
            }
        }

        public static IContainer GetContainer(DataReader dataReader)
        {
            // Check arguments.
            CheckNotNull(dataReader, "dataReader");
            ContainerType containerType;

            switch ((ContainerType)(Enum.Parse(typeof(ContainerType), (dataReader.GetString(ContainerData.TYPE)))))
            {
                case ContainerType.Box:
                    containerType = ContainerType.Box;
                    break;
                case ContainerType.Building:
                    containerType = ContainerType.Building;
                    break;
                case ContainerType.Floor:
                    containerType = ContainerType.Floor;
                    break;
                case ContainerType.Freezer:
                    containerType = ContainerType.Freezer;
                    break;
                case ContainerType.Refrigerator:
                    containerType = ContainerType.Refrigerator;
                    break;
                case ContainerType.Room:
                    containerType = ContainerType.Room;
                    break;
                case ContainerType.Shelf:
                    containerType = ContainerType.Shelf;
                    break;
                case ContainerType.TopLevel:
                    containerType = ContainerType.TopLevel;
                    break;
                case ContainerType.Uncontained:
                    containerType = ContainerType.Uncontained;
                    break;
                default:
                    throw new DataException("Unsupported container type " +
                                          dataReader.GetString(ContainerData.TYPE));
            }
            return new Container(dataReader, containerType);
        }

        public static GenericContainerList GetContainersTopLevel()
        {
            DataReader dataReader = null;
            GenericContainerList containers;

            try
            {
                containers = new GenericContainerList();
                dataReader = Database.GetContainersTopLevel();
                while (dataReader.Read())
                {
                    containers.Add(GetContainer(dataReader));
                }
                return containers;
            }
            finally
            {
                CloseDataReader(dataReader);
            }
        }

        public static IContainer GetUncontainedContainer()
        {
            DataReader dataReader = null;
            Container container = null;

            try
            {
                dataReader = Database.GetGenericContainerByIdentifier("Uncontained");
                if (dataReader.Read())
                {
                    container = new Container(dataReader, ContainerType.Uncontained);
                }
                return (IContainer)container;
            }
            finally
            {
                CloseDataReader(dataReader);
            }
        }
    }
}
