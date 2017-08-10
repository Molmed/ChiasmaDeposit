using System;
using System.Collections;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Data
{
    public class TubeRackType : DataIdentity
    {
        private Int32 MySizeX;
        private Int32 MySizeY;

        public TubeRackType(DataReader dataReader)
            : base(dataReader)
        {
            MySizeX = dataReader.GetInt32(TubeRackTypeData.SIZE_X);
            MySizeY = dataReader.GetInt32(TubeRackTypeData.SIZE_Y);
        }

        public override DataType GetDataType()
        {
            return DataType.TubeRackType;
        }

        public static Int32 GetIdentifierMaxLength()
        {
            return GetColumnLength(TubeRackTypeData.TABLE, TubeRackTypeData.IDENTIFIER);
        }

        public static Int32 GetLabelMaxLength()
        {
            return GetColumnLength(TubeRackLabelData.TABLE, TubeRackLabelData.IDENTIFIER);
        }

        public Int32 GetSlotCount()
        {
            return MySizeX * MySizeY;
        }

        public Int32 GetSizeX()
        {
            return MySizeX;
        }

        public Int32 GetSizeY()
        {
            return MySizeY;
        }

        public bool CheckSlotPosition(string position)
        {
            int row, col;
            if (!TubeRackManager.ParseSlotPosition(position, out row, out col))
            {
                return false;
            }
            if (row < 0 || row + 1 > GetSizeY())
            {
                return false;
            }
            if (col < 0 || col + 1 > GetSizeX())
            {
                return false;
            }
            return true;
        }
    }

    public class TubeRackTypeList : DataIdentityList
    {
        public new TubeRackType GetById(Int32 id)
        {
            return (TubeRackType)(base.GetById(id));
        }

        public new TubeRackType this[Int32 index]
        {
            get
            {
                return (TubeRackType)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        public new TubeRackType this[String identifier]
        {
            get
            {
                return (TubeRackType)(base[identifier]);
            }
        }
    }
}
