using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Molmed.ChiasmaDep.Dialog;

namespace Molmed.ChiasmaDep
{
    static class Program
    {
        private const Boolean MyVersionControl = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LoadSampleStorageDuoDialog mainForm;
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Init main.
                mainForm = new LoadSampleStorageDuoDialog(false);
                if (mainForm.Login(MyVersionControl))
                {
                    Application.Run(new LoadSampleStorageDuoDialog(true));
                }
            }
            catch (Exception exception)
            {
                LoadSampleStorageDuoDialog.HandleError("General error", exception);
            }
            finally
            {
                LoadSampleStorageDuoDialog.LogoutDatabase();
            }
        }
    }
}