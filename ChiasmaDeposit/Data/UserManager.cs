using System;
using System.Collections.Generic;
using Molmed.ChiasmaDep.Data.Exception;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Data
{
    public class UserManager : ChiasmaDepData
    {
        private static UserList MyUsers = null;
        private static User MyCurrentUser = null;

        private UserManager()
            : base()
        {
        }

        public static UserList GetActiveUsers()
        {
            return GetActiveUsers(true);
        }

        public static void ReleaseAuthorityMapping()
        {
            Database.ReleaseAuthorityMapping();
        }

        public static void SetAuthorityMappingFromBarcode(string userBarcode)
        {
            DataReader reader = null;
            try
            {
                reader = Database.SetAuthorityMappingFromBarcode(userBarcode);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseDataReader(reader);
            }
        }

        public static void SetAuthorityMappingFromSysUser()
        {
            DataReader reader = null;
            try
            {
                reader = Database.SetAuthorityMappingFromSysUser();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseDataReader(reader);
            }
        }

        private static User GetUserFromBarcode(string barcode)
        {
            DataReader reader = null;
            User user = null;
            try
            {
                reader = Database.GetUserFromBarcode(barcode);
                if (reader.Read())
                {
                    user = new User(reader);
                }
            }
            finally
            {
                CloseDataReader(reader);
            }
            return user;
        }

        public static bool IsUserBarcode(string barcode)
        {
            User user = null;

            user = GetUserFromBarcode(barcode);

            return IsNotNull(user);
        }

        public static UserList GetActiveUsers(Boolean includeDevelopers)
        {
            UserList activeUsers;

            LoadUsers();
            activeUsers = new UserList();
            foreach (User user in MyUsers)
            {
                if (user.IsAccountActive() &&
                    (includeDevelopers || !user.IsDeveloper()))
                {
                    activeUsers.Add(user);
                }
            }
            return activeUsers;
        }

        public static User GetCurrentUser()
        {
            LoadUsers();
            return MyCurrentUser;
        }

        public static User GetUser(Int32 userId)
        {
            LoadUsers();
            return MyUsers.GetById(userId);
        }

        public static User GetUser(String identifier)
        {
            LoadUsers();
            return MyUsers[identifier];
        }

        public static User GetUserByBarCode(String barcode)
        {
            LoadUsers();
            return MyUsers.GetByBarCode(barcode);
        }

        private static void LoadUsers()
        {
            DataReader dataReader = null;

            if (IsNull(MyUsers))
            {
                try
                {
                    // Get information about all users from database.
                    dataReader = Database.GetUsers();
                    MyUsers = new UserList();
                    while (dataReader.Read())
                    {
                        MyUsers.Add(new User(dataReader));
                    }
                    dataReader.Close();

                    // Get current user from database.
                    dataReader = Database.GetUserCurrent();
                    if (dataReader.Read())
                    {
                        MyCurrentUser = new User(dataReader);

                        // Return same user object as in MyUsers.
                        MyCurrentUser = MyUsers.GetById(MyCurrentUser.GetId());
                    }
                    else
                    {
                        throw new DataException("Could not retrieve current user from database!");
                    }
                }
                catch
                {
                    MyUsers = null;
                    MyCurrentUser = null;
                    throw;
                }
                finally
                {
                    CloseDataReader(dataReader);
                }
            }
        }

        public static new void Refresh()
        {
            MyUsers = null;
            MyCurrentUser = null;
            LoadUsers();
        }
    }
}
