using Microsoft.Win32;
using System;
using System.Collections;
using System.Windows.Forms;
using ChiasmaDeposit.Properties;

namespace Molmed.ChiasmaDep
{
    public class Config
    {
        public enum ApplicationMode
        {
            Office,
            Lab
        }

        public static ApplicationMode GetApplicationMode()
        {
            if (Settings.Default.ApplicationMode.ToUpper() == "LAB")
            {
                return ApplicationMode.Lab;
            }
            else if (Settings.Default.ApplicationMode.ToUpper() == "OFFICE")
            {
                return ApplicationMode.Office;
            }
            else
            {
                throw new Exception("Unknown application mode setting.");
            }
        }

        public static String GetApplicationPath()
        {
            return Application.StartupPath;
        }

        public static Int32 GetAutomaticLogoutTimeLimit()
        {
            //The number of seconds without (mouse) activity
            //before automatic logout.
            return Settings.Default.AutomaticLogoutTimeLimit;
        }

        public static String GetDialogTitleStandard()
        {
            return Settings.Default.DialogTitleStandard;
        }

    }
}
