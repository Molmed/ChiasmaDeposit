using System;
using System.Collections.Generic;
using Molmed.ChiasmaDep.Data.Exception;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Data
{
    public delegate void GenericContainerCreatedEventHandler(GenericContainerList containers);
    public delegate void GenericContainerMovedEventHandler(GenericContainerList movedContainers,
                                                     IGenericContainer toContainer);
    public delegate void GenericContainerRestoredEventHandler(IGenericContainer container);

    public class GenericContainerManager : ChiasmaDepData
    {
        private GenericContainerManager()
            : base()
        {
        }

        private static void CheckCirularReferens(IGenericContainer moveContainer,
                                               IGenericContainer toContainer)
        {
            GenericContainerList containerPath;

            // Check that containers not are moved into it's own content.
            // Avoid circular reference.
            if (IsNotNull(toContainer))
            {
            containerPath = toContainer.GetContainerPath();
            //Add the destination container itself to the path.
            containerPath.Add(toContainer);
                if (containerPath.IsMember(moveContainer))
                {
                    throw new DataException("Can't move " +
                                          moveContainer.GetIdentifier() +
                                          " into " +
                                          toContainer.GetIdentifier() +
                                          ". Circular referenses are not allowed!");
                }
            }
        }

        private static void CheckMoveContainer(IGenericContainer moveContainer)
        {
            // Check that no top level container is moved into another container.
            if (moveContainer.IsTopLevel())
            {
                throw new DataException("Can't move a TopLevel container into another container");
            }
        }

        private static void CheckToContainer(IGenericContainer toContainer)
        {
            // Check that target container isn't a BeadChip, plate or tube.
            if (IsNotNull(toContainer) && toContainer.IsBeadChip())
            {
                throw new DataException("Can't move containers into a BeadChip");
            }
            if (IsNotNull(toContainer) && toContainer.IsPlate())
            {
                throw new DataException("Can't move containers into a plate");
            }
            if (IsNotNull(toContainer) && toContainer.IsTube())
            {
                throw new DataException("Can't move containers into a tube");
            }
        }

        public static IGenericContainer GetGenericContainerByBarCode(String barCode)
        {
            DataReader dataReader = null;

            try
            {
                // Check arguments.
                CheckNotEmpty(barCode, "barCode");
                CheckLength(barCode, "barCode", BarCodeManager.GetBarCodeMaxLength());

                // Get data from database.
                dataReader = Database.GetGenericContainerByBarCode(barCode);
                return GetFirstGenericContainer(dataReader);
            }
            finally
            {
                CloseDataReader(dataReader);
            }
        }

        public static GenericContainerList GetGenericContainers(DataReader dataReader)
        {
            // This function differ from the original Chiasma-function
            // No need to implement classes for plates, tubes, flowcells and beadchips here.
            GenericContainerList containers = null;
            GenericContainerList containersSubGroup = null;
            TubeRackList tubeRacks;

            containers = new GenericContainerList();
            while (dataReader.Read())
            {
                // Add containers, not BeadChip, plate or tube.
                containers.Add(ContainerManager.GetContainer(dataReader));
            }
            containers.Sort();
            if (dataReader.NextResult())
            {
                tubeRacks = new TubeRackList();
                while (dataReader.Read())
                {
                    tubeRacks.Add(new TubeRackForTagSource(dataReader));
                }
                tubeRacks.Sort();
                containers.AddRange(tubeRacks);
            }

            if (dataReader.NextResult())
            {
                containersSubGroup = new GenericContainerList();
                while (dataReader.Read())
                {
                    // Add FlowCells.
                    containersSubGroup.Add(new GenericContainer(dataReader, ContainerType.FlowCell));
                }
                containersSubGroup.Sort();
                containers.AddRange(containersSubGroup);
            }
            if (dataReader.NextResult())
            {
                containersSubGroup = new GenericContainerList();
                while (dataReader.Read())
                {
                    // Add BeadChips.
                    containersSubGroup.Add(new GenericContainer(dataReader, ContainerType.BeadChip));
                }
                containersSubGroup.Sort();
                containers.AddRange(containersSubGroup);
            }
            if (dataReader.NextResult())
            {
                containersSubGroup = new GenericContainerList();
                while (dataReader.Read())
                {
                    // Add plates.
                    containersSubGroup.Add(new GenericContainer(dataReader, ContainerType.Plate));
                }
                containersSubGroup.Sort();
                containers.AddRange(containersSubGroup);
            }
            if (dataReader.NextResult())
            {
                containersSubGroup = new GenericContainerList();
                while (dataReader.Read())
                {
                    // Add tubes.
                    containersSubGroup.Add(new GenericContainer(dataReader, ContainerType.Tube));
                }
                containersSubGroup.Sort();
                containers.AddRange(containersSubGroup);
            }
            return containers;
        }

        private static IGenericContainer GetFirstGenericContainer(DataReader dataReader)
        {
            GenericContainerList genericContainers;

            genericContainers = GetGenericContainers(dataReader);
            if (IsEmpty(genericContainers))
            {
                return null;
            }
            else
            {
                return genericContainers[0];
            }
        }

        public static void MoveGenericContainer(IGenericContainer moveContainer,
                                       IGenericContainer toContainer, int user_id)
        {
            // Check parameters.
            CheckNotNull(moveContainer, "moveContainer");
            CheckMoveContainer(moveContainer);
            CheckToContainer(toContainer);
            CheckCirularReferens(moveContainer, toContainer);
            Database.MoveContainer(moveContainer.GetId(), GetId(toContainer), user_id);
        }
    }
}
