using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Molmed.ChiasmaDep.Data;
using ChiasmaDeposit.Properties;

namespace Molmed.ChiasmaDep.Database
{
    public delegate void TransactionCommitedEventHandler();
    public delegate void TransactionRollbackedEventHandler();

    public class Dataserver : ChiasmaDepBase
    {
        private Int32 MyCommandTimeout;
        private SqlConnection MyConnection;
        private String MyConnectionString;
        private SqlTransaction MyTransaction;
        private SqlCommand MyDataReaderCommand;

        public event TransactionCommitedEventHandler TransactionCommited;
        public event TransactionRollbackedEventHandler TransactionRollbacked;

        public Dataserver(String userName, String password)
            : this(userName, password, Settings.Default.DatabaseName)
        {
        }

        public Dataserver(String userName, String password, String database)
        {
            MyCommandTimeout = Settings.Default.DatabaseCommandTimeout;
            SetConnectionString(userName, password, database);
        }

        public Boolean AuthenticateApplication(String applicationName,
                                   String applicationVersion)
        {
            //Returns true if the application with name appName and version
            //appVersion is allowed to connect.
            String cmdText;

            cmdText = "SELECT COUNT(*) FROM application_version " +
                      "WHERE identifier = '" + applicationName + "' AND " +
                      "version = '" + applicationVersion + "'";
            return (this.ExecuteScalar(cmdText) > 0);
        }


        public Int32 CommandTimeout
        {
            get
            {
                return MyCommandTimeout;
            }
            set
            {
                MyCommandTimeout = value;
            }
        }

        private void AssertDatabaseConnection()
        {
            //If the database connection is broken, try to reconnect.
            if (MyConnection.State == ConnectionState.Closed || MyConnection.State == ConnectionState.Broken)
            {
                Connect();
            }
        }

        public void BeginTransaction()
        {
            AssertDatabaseConnection();

            if (IsNull(MyTransaction))
            {
                MyTransaction = MyConnection.BeginTransaction();
            }
            else
            {
                throw new Exception("Transaction already active.");
            }
        }

        public void CommitTransaction()
        {
            if (IsNotNull(MyTransaction))
            {
                MyTransaction.Commit();
                MyTransaction = null;

                if (IsNotNull(TransactionCommited))
                {
                    TransactionCommited();
                }
            }
            else
            {
                throw new Exception("Unable to commit inactive transaction.");
            }
        }

        public Boolean Connect()
        {
            //Opens the database connection.
            MyConnection = new SqlConnection(MyConnectionString);
            MyConnection.Open();

            return (MyConnection.State == ConnectionState.Open);
        }

        public void Disconnect()
        {
            //Closes the current database connection.
            if ((MyConnection.State == ConnectionState.Open) ||
                 (MyConnection.State == ConnectionState.Fetching))
            {
                MyConnection.Close();
            }
        }

        private Int32 ExecuteCommand(SqlCommandBuilder commandBuilder)
        {
            return GetCommand(commandBuilder).ExecuteNonQuery();
        }

        private Int32 ExecuteCommand(String sqlQuery)
        {
            return GetCommand(sqlQuery).ExecuteNonQuery();
        }

        public Int32 ExecuteScalar(SqlCommandBuilder commandBuilder)
        {
            return ExecuteScalar(commandBuilder.GetCommand());
        }

        public Int32 ExecuteScalar(String sqlQuery)
        {
            return Convert.ToInt32(GetCommand(sqlQuery).ExecuteScalar());
        }

        public Int32 GetColumnLength(String tableName, String columnName)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetColumnLength");
            commandBuilder.AddParameter("table", tableName);
            commandBuilder.AddParameter("column", columnName);

            return ExecuteScalar(commandBuilder);
        }

        public SqlCommand GetCommand(SqlCommandBuilder commandBuilder)
        {
            return GetCommand(commandBuilder.GetCommand());
        }

        public SqlCommand GetCommand(String sqlQuery)
        {
            SqlCommand command;

            AssertDatabaseConnection();

            if (IsNull(MyTransaction))
            {
                command = new SqlCommand(sqlQuery, MyConnection);
            }
            else
            {
                command = new SqlCommand(sqlQuery, MyConnection, MyTransaction);
            }
            command.CommandTimeout = CommandTimeout;
            return command;
        }

        public DataReader GetGenericContainerById(Int32 genericContainerId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetGenericContainerById");
            commandBuilder.AddParameter(GenericContainerData.GENERIC_CONTAINER_ID, genericContainerId);

            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        public DataReader GetContainerPath(Int32 genericContainerId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetContainerPath");
            commandBuilder.AddParameter(GenericContainerData.GENERIC_CONTAINER_ID, genericContainerId);

            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        public DataReader GetContainersTopLevel()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetContainersTopLevel");

            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        public DataReader GetGenericContainerByBarCode(String barCode)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetGenericContainerByBarcode");
            commandBuilder.AddParameter(GenericContainerData.BAR_CODE, barCode);

            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        public DataReader GetGenericContainerByIdentifier(String genericContainerIdentifier)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetGenericContainerByIdentifier");
            commandBuilder.AddParameter(GenericContainerData.IDENTIFIER, genericContainerIdentifier);

            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        public DataReader GetItem(String barCode)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetItem");
            commandBuilder.AddParameter(BarCodeData.BAR_CODE, barCode);

            return GetRow(commandBuilder);
        }

        private DataReader GetRow(SqlCommandBuilder commandBuilder)
        {
            return GetReader(commandBuilder, CommandBehavior.SingleRow |
                            CommandBehavior.SingleResult);
        }


        private DataReader GetReader(SqlCommandBuilder commandBuilder)
        {
            return GetReader(commandBuilder, CommandBehavior.SingleResult);
        }

        private DataReader GetReader(SqlCommandBuilder commandBuilder,
                          CommandBehavior commandBehavior)
        {
            MyDataReaderCommand = GetCommand(commandBuilder);

            return new DataReader(MyDataReaderCommand.ExecuteReader(commandBehavior), MyDataReaderCommand);
        }

        public DataReader GetUserByBarCode(String barCode)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetUserByBarCode");
            commandBuilder.AddParameter(UserData.BAR_CODE, barCode);

            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        public DataReader GetUserCurrent()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetUserCurrent");

            return GetRow(commandBuilder);
        }

        public DataReader GetUserFromBarcode(string barcode)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetUserFromBarcode");
            commandBuilder.AddParameter(LoginData.CHIASMA_BARCODE, barcode);

            return GetReader(commandBuilder);
        }

        public DataReader GetUsers()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_GetUsers");

            return GetReader(commandBuilder);
        }

        public Boolean HasPendingTransaction()
        {
            return (MyTransaction != null);
        }

        public Int32 MoveContainer(Int32 movedContainerId, Int32 toContainerId, int user_id)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_MoveContainer");
            commandBuilder.AddParameter(ContainerData.CONTAINER_ID, movedContainerId);
            if (ChiasmaDepData.IsValidId(toContainerId))
            {
                commandBuilder.AddParameter(ContainerData.TO_CONTAINER_ID, toContainerId);
            }

            commandBuilder.AddParameter(ContainerData.AUTHORITY_ID, user_id);

            return ExecuteCommand(commandBuilder);
        }

        public int ReleaseAuthorityMapping()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_ReleaseAuthorityMapping");

            return ExecuteCommand(commandBuilder);
        }


        public void RollbackTransaction()
        {
            if (IsNotNull(MyTransaction))
            {
                try
                {
                    MyTransaction.Rollback();
                }
                catch
                {
                }
                MyTransaction = null;

                if (IsNotNull(TransactionRollbacked))
                {
                    TransactionRollbacked();
                }
            }
            else
            {
                throw new Exception("Unable to rollback inactive transaction.");
            }
        }

        public DataReader SetAuthorityMappingFromBarcode(string userBarcode)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_SetAuthorityMappingFromBarcode");
            commandBuilder.AddParameter(LoginData.CHIASMA_BARCODE, userBarcode);

            return GetRow(commandBuilder);
        }

        public DataReader SetAuthorityMappingFromSysUser()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("p_SetAuthorityMappingFromSysUser");

            return GetRow(commandBuilder);
        }

        private void SetConnectionString(String userName, String password, String database)
        {
            if (IsEmpty(userName) || IsEmpty(password))
            {
                MyConnectionString = "data source=" + Settings.Default.DataServerAddress +
                                   ";integrated security=true;" +
                                   "initial catalog=" + database + ";";
            }
            else
            {
                MyConnectionString = "data source=" + Settings.Default.DataServerAddress +
                                   ";integrated security=false;" +
                                   "initial catalog=" + database + ";" +
                                   "user id=" + userName + ";" +
                                   "pwd=" + password + ";";
            }
        }
    }
}
