using System;
using System.Collections.Generic;
using System.Text;
using Molmed.ChiasmaDep.Database;
using ChiasmaDeposit.Properties;

namespace Molmed.ChiasmaDep.Data
{
    class BarCodeManager : ChiasmaDepData
    {
        private BarCodeManager()
            : base()
        {
        }
        public static Int32 GetBarCodeMaxLength()
        {
            return Math.Min(GetColumnLength(BarCodeData.TABLE_EXTERNAL, BarCodeData.BAR_CODE_COLUMN),
                           GetColumnLength(BarCodeData.TABLE_INTERNAL, BarCodeData.BAR_CODE_COLUMN));
        }

        public static Int32 GetInternalBarCodeLength()
        {
            return Settings.Default.BarCodeLengthInternal;
        }


        public static IDataIdentity GetItem(String barCode)
        {
            DataReader dataReader = null;
            IDataIdentity dataIdentity = null;
            Int32 id;
            String kind;

            try
            {
                dataReader = Database.GetItem(barCode);
                if (dataReader.Read() &&
                    dataReader.IsNotDBNull(BarCodeData.IDENTIFIABLE_ID) &&
                    dataReader.IsNotDBNull(BarCodeData.KIND))
                {
                    id = dataReader.GetInt32(BarCodeData.IDENTIFIABLE_ID);
                    kind = dataReader.GetString(BarCodeData.KIND);
                    dataReader.Close();

                    switch ((DBKind)(Enum.Parse(typeof(DBKind), kind)))
                    {
                        case DBKind.CONTAINER:
                            //dataIdentity = GenericContainerManager.GetGenericContainer(id);
                            break;
                        case DBKind.AUTHORITY:
                            dataIdentity = UserManager.GetUser(id);
                            break;
                    }
                }
            }
            finally
            {
                CloseDataReader(dataReader);
            }
            return dataIdentity;
        }

        public static Int32 GetMaxTimeToRead()
        {
            // A bar code must be read within this number of seconds
            // to be regarded as an automatic reading.
            return Settings.Default.BarCodeMaxTimeToRead;
        }

        public static Boolean IsBarCodeDefined(String barCode)
        {
            DataReader dataReader = null;

            try
            {
                dataReader = Database.GetItem(barCode);
                return dataReader.Read();
            }
            finally
            {
                CloseDataReader(dataReader);
            }
        }
    }
}
