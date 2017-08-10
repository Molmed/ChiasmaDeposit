using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Data
{

    public enum TubeRackUsage
    { 
        OrdinaryTubeRack,
        TagAssociationTubeRack,
        TagSourceTubeRack
    }

    public abstract class TubeRack : GenericContainer
    {
        protected int MyNumberRows;
        protected int MyNumberColumns;
        protected TubeRackType MyTubeRackType;
        protected int MyTubeRackTypeId;
        protected int MyNumberOfEmptySlots;
        protected int MyControlStartIndex;
        protected int MyTubeRackNumber; // Starts with 1
        protected const string NO_TAG = "NO TAG";
        protected const string POOL = "POOL";
        protected const string ALREADY_TAGGED = "ALREADY TAGGED";


        public TubeRack(DataReader reader)
            : base(reader, ContainerType.TubeRack)
        {
            MyTubeRackTypeId = reader.GetInt32(TubeRackData.TUBE_RACK_TYPE_ID, NO_ID);
            MyTubeRackType = null;
            MyNumberOfEmptySlots = reader.GetInt32(TubeRackData.EMPTY_SLOTS, 0);
            MyTubeRackNumber = reader.GetInt32(TubeRackData.TUBE_RACK_NUMBER, NO_COUNT);
            // Set MyControlStartIndex to the 2nd column, 2nd row, if it exist
            // otherwise 0
            if (MyNumberColumns >= 2 && MyNumberRows >= 2 &&
                (MyNumberRows * MyNumberColumns - MyNumberRows - 1) >= MyNumberOfEmptySlots)
            {
                MyControlStartIndex = MyNumberRows;
            }
            else
            {
                MyControlStartIndex = 0;
            }
        }

        public abstract TubeRackUsage GetTubeRackUsage();

        public int GetNumberOfEmptySlots()
        {
            return MyNumberOfEmptySlots;
        }

        public int GetTubeRackNumber()
        {
            return MyTubeRackNumber;
        }

        public int GetNumberRows()
        {
            return MyNumberRows;
        }

        public int GetNumberColumns()
        {
            return MyNumberColumns;
        }

        public override DataType GetDataType()
        {
            return DataType.TubeRack;
        }

        public int GetNumberOfSlots()
        {
            return MyNumberColumns * MyNumberRows;
        }

        public bool IsControlSlot(int row, int col)
        { 
            int index;
            index = MyNumberRows*col + row;
            return (index >= MyControlStartIndex && index <= MyControlStartIndex + MyNumberOfEmptySlots);
        }

    }

    public class TubeRackList : GenericContainerList
    {
        public new TubeRack GetById(Int32 id)
        {
            return (TubeRack)(base.GetById(id));
        }

        public new TubeRack this[Int32 index]
        {
            get
            {
                return (TubeRack)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        public new TubeRack this[String identifier]
        {
            get
            {
                return (TubeRack)(base[identifier]);
            }
        }

        public override object Clone()
        {
            TubeRackList tubeRacks = new TubeRackList();
            foreach (TubeRack tubeRack in this)
            {
                tubeRacks.Add(tubeRack);
            }
            return tubeRacks;
        }
    }

}
