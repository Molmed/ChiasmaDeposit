using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Timers;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;
using ChiasmaDeposit.Properties;
using Molmed.ChiasmaDep;
using Molmed.ChiasmaDep.Data;
using Molmed.ChiasmaDep.Data.Exception;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Dialog 
{
    public partial class LoadSampleStorageDuoDialog : Form
    {
        private enum ShowMode : int
        { 
            OnlyValid = 0,
            All = 1
        }

        private Int32 MyActivityCounter;
        private Point MyLastMousePos;

        private LoginWithBarcodeDialog MyLoginWithBarcodeDialog;

        private BarCodeEventHandler MyBarcodeEventhandler;
        private BarCodeController MyBarCodeController;

        public LoadSampleStorageDuoDialog()
        {
            InitializeComponent();
        }

        public LoadSampleStorageDuoDialog(bool loggedIn)
        {
            InitializeComponent();
            if (loggedIn)
            {
                Init();
            }
        }

        private void ActivityTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Update activity information.
            if (MousePosition == MyLastMousePos)
            {
                MyActivityCounter++;
            }
            else
            {
                MyLastMousePos = MousePosition;
                MyActivityCounter = 0;
            }
            ShowTimeLeft();

            // Check if it is time for automatic logout.
            if (MyActivityCounter > Config.GetAutomaticLogoutTimeLimit())
            {
                Reset();
            }
        }

        public void BarCodeReceived(String barCode)
        {
            try
            {
                HandleReceivedBarCode(barCode);
            }
            catch (BarCodeException exception)
            {
                ShowWarning(exception.Message);
            }
            catch (Exception exception)
            {
                HandleError("Could not receive bar code information", exception);
            }
            finally
            {
                MyActivityCounter = 0;
            }
        }

        private GenericContainerList GetDoublets(GenericContainerList newContainers, 
            out GenericContainerList uniqueList)
        {
            Dictionary<int, string> idDict = new Dictionary<int, string>();
            GenericContainerList doublets = new GenericContainerList();
            uniqueList = new GenericContainerList();
            foreach (DuoViewItem viewItem in SampleStorageListView.Items)
            {
                if (viewItem.Checked && !idDict.ContainsKey(viewItem.GetSampleContainer().GetId()))
                {
                    idDict.Add(viewItem.GetSampleContainer().GetId(), "");
                }
            }
            foreach (IGenericContainer container in newContainers)
            {
                if (!idDict.ContainsKey(container.GetId()))
                {
                    idDict.Add(container.GetId(), "");
                    uniqueList.Add(container);
                }
                else
                {
                    doublets.Add(container);
                }
            }
            return doublets;
        }

        private void HandleReceivedBarCode(string barCode)
        {
            SampleListDialog sampleListDialog;
            IGenericContainer genericContainer;
            IGenericContainer depositContainer;
            GenericContainerList selectedContainers;
            GenericContainerList doublets, uniqueList;
            DoubletsDialog doubletsDialog;
            genericContainer = GenericContainerManager.GetGenericContainerByBarCode(barCode);
            if (!CheckContainerType(genericContainer))
            {
                throw new Data.Exception.DataException("This bar code neither represent a sample container nor a deposit");
            }
            sampleListDialog = new SampleListDialog(genericContainer);
            MyBarCodeController = null;
            if (sampleListDialog.ShowDialog() == DialogResult.OK)
            {
                depositContainer = sampleListDialog.GetSelectedDeposit();
                selectedContainers = sampleListDialog.GetSelectedContainers();
                doublets = GetDoublets(selectedContainers, out uniqueList);
                if (doublets.Count > 0)
                {
                    doubletsDialog = new DoubletsDialog(doublets, "These duplicates are sorted out from the list!");
                    doubletsDialog.ShowDialog();
                    selectedContainers = uniqueList;
                }
                SampleStorageListView.BeginUpdate();
                foreach (IGenericContainer container in selectedContainers)
                {
                    SampleStorageListView.Items.Add(new DuoViewItem(depositContainer, container));
                }
                SampleStorageListView.EndUpdate();
                SampleStorageListView.Columns[(int)DuoViewItem.ListIndex.SampleContainer].Width = -2;
                this.printToolStripMenuItem.Enabled = true;
                this.exportToolStripMenuItem.Enabled = true;
            }
            MyBarCodeController = new BarCodeController(this);
            MyBarCodeController.BarCodeReceived += MyBarcodeEventhandler;
        }

        private bool CheckContainerType(IGenericContainer genericContainer)
        {
            if (IsNull(genericContainer) ||
                !(IsSampleContainer(genericContainer) || IsStorageContainer(genericContainer)))
            {
                return false;
            }
            return true;
        }

        public static bool IsSampleContainer(IGenericContainer genericContainer)
        {
            if (genericContainer.GetContainerType() == ContainerType.FlowCell ||
               genericContainer.GetContainerType() == ContainerType.BeadChip ||
               genericContainer.GetContainerType() == ContainerType.Plate ||
               genericContainer.GetContainerType() == ContainerType.Tube ||
                genericContainer.GetContainerType() == ContainerType.TubeRack)
            {
                return true;
            }
            return false;
        }

        public static bool IsStorageContainer(IGenericContainer genericContainer)
        {
            if (genericContainer.GetContainerType() == ContainerType.Box ||
               genericContainer.GetContainerType() == ContainerType.Building ||
               genericContainer.GetContainerType() == ContainerType.Floor ||
               genericContainer.GetContainerType() == ContainerType.Freezer ||
               genericContainer.GetContainerType() == ContainerType.Refrigerator ||
               genericContainer.GetContainerType() == ContainerType.Room ||
               genericContainer.GetContainerType() == ContainerType.Shelf ||
               genericContainer.GetContainerType() == ContainerType.TopLevel ||
               genericContainer.GetContainerType() == ContainerType.Uncontained)
            {
                return true;
            }
            return false;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static String GetApplicationPath()
        {
            return Application.StartupPath;
        }

        public static void HandleError(String message, Exception exception)
        {
            ShowErrorDialog errorDialog;

            errorDialog = new ShowErrorDialog(message, exception);
            errorDialog.ShowDialog();
        }

        private void Init()
        {
            InitListView();
            // Init barcode controller
            MyBarCodeController = new BarCodeController(this);
            MyBarcodeEventhandler = new BarCodeEventHandler(BarCodeReceived);
            MyBarCodeController.BarCodeReceived += MyBarcodeEventhandler;
            IdentificationTextBox.Text = UserManager.GetCurrentUser().GetName();
            if (Settings.Default.DatabaseName.ToLower().Contains("practice"))
            {
                Text += $" ({Settings.Default.DatabaseName})";
                ValidationReminderPanel.BackgroundImage = Resources.ValidationBackground;
            }
            else if (Settings.Default.DatabaseName.ToLower().Contains("devel"))
            {
                Text += $" ({Settings.Default.DatabaseName})";
                ValidationReminderPanel.BackgroundImage = Resources.DevelBackground;
            }

        }

        private void InitListView()
        {
            SampleStorageListView.Clear();
            SampleStorageListView.Columns.Add("Sample container", -2);
            SampleStorageListView.Columns.Add("Storage", -2);
        }

        protected static Boolean IsEmpty(ICollection collection)
        {
            return ((collection == null) || (collection.Count == 0));
        }

        protected static Boolean IsEmpty(String testString)
        {
            return (testString == null) || (testString.Trim().Length == 0);
        }

        protected static Boolean IsNotEmpty(ICollection collection)
        {
            return ((collection != null) && (collection.Count > 0));
        }

        protected static Boolean IsNotEmpty(String testString)
        {
            return (testString != null) && (testString.Trim().Length > 0);
        }

        protected static Boolean IsNotNull(Object testObject)
        {
            return (testObject != null);
        }

        protected static Boolean IsNull(Object testObject)
        {
            return (testObject == null);
        }

        public bool Login(bool versionControl)
        {
            // BarCodeController barCodeController;
            Boolean isLoginOk = false;
            String version;
            String applicationName;
            string barcode;

            if (Config.GetApplicationMode() == Config.ApplicationMode.Lab)
            {
                // LAB MODE
                // Enable background bar code listening.
                KeyPreview = true;

                if (!LoginDataBase())
                {
                    return false;
                }

                do
                {
                    // Get login information.
                    MyLoginWithBarcodeDialog = new LoginWithBarcodeDialog();

                    // Login by bar code is currently not supported.
                    // barCodeController = new BarCodeController(MyLoginForm);
                    // barCodeController.BarCodeReceived += ExecuteBarCode;
                    if (MyLoginWithBarcodeDialog.ShowDialog() == DialogResult.OK)
                    {
                        barcode = MyLoginWithBarcodeDialog.Barcode;
                        MyLoginWithBarcodeDialog = null;
                    }
                    else
                    {
                        // The user cancelled.
                        LogoutDatabase();
                        return false;
                    }
                }   // Try to login to the database.
                while (!SetAuthorityMappingForBarcode(barcode));


                UserManager.Refresh();
                Text = Config.GetDialogTitleStandard() + " - " + UserManager.GetCurrentUser().GetName();

                // Start the activity timer. It is used for automatic logout in lab mode.
                ActivityTimer.Enabled = false;
                MyActivityCounter = 0;
                isLoginOk = true;
            }
            else
            {
                // OFFICE MODE
                isLoginOk = LoginDataBase();
                SetAuthorityMappingFromSysUser();
            }

            if (!UserManager.GetCurrentUser().IsAccountActive())
            {
                throw new Exception("User " + UserManager.GetCurrentUser().GetName() + " has been inactivated");
            }

            if (isLoginOk)
            {
                if (versionControl)
                {
                    // Get version number of the current assembly.
                    version = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                              Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                              Assembly.GetExecutingAssembly().GetName().Version.Build + "." +
                              Assembly.GetExecutingAssembly().GetName().Version.Revision;

                    // Get the name of the current assembly.
                    applicationName = Assembly.GetExecutingAssembly().GetName().Name;

                    // Make sure the current application is allowed to connect.
                    if (!ChiasmaDepData.Database.AuthenticateApplication(applicationName, version))
                    {
                        HandleError("The current version of this program is not allowed to connect to the database.", null);
                        return false;
                    }
                }

                // Cache data.
                // This is done in order to avoid DateReader already open exception.
                ChiasmaDepData.Refresh();
            }

            return isLoginOk;
        }

        private bool SetAuthorityMappingForBarcode(string userBarcode)
        {
            if (!UserManager.IsUserBarcode(userBarcode))
            {
                MessageBox.Show("This barcode could not be linked to a Chiasma user!", "Login failure", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            try
            {
                ChiasmaDepData.BeginTransaction();
                UserManager.SetAuthorityMappingFromBarcode(userBarcode);
                ChiasmaDepData.CommitTransaction();
            }
            catch (System.Exception ex)
            {
                ChiasmaDepData.RollbackTransaction();
                HandleError("Could not logon to database", ex);
            }

            return true;
        }

        private void SetAuthorityMappingFromSysUser()
        {
            UserManager.SetAuthorityMappingFromSysUser();
        }



        public Boolean Login_old(Boolean versionControl)
        {
            String version;
            String applicationName;

            if (versionControl)
            {
                // Get version number of the current assembly.
                version = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                          Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                          Assembly.GetExecutingAssembly().GetName().Version.Build + "." +
                          Assembly.GetExecutingAssembly().GetName().Version.Revision;

                // Get the name of the current assembly.
                applicationName = Assembly.GetExecutingAssembly().GetName().Name;

                // Make sure the current application is allowed to connect.
                try
                {
                    if (!(LoginDataBase() &&
                          (ChiasmaDepData.Database.AuthenticateApplication(applicationName, version))))
                    {
                        HandleError("The current version of this program is not allowed to connect to the database.", null);
                        return false;
                    }
                }
                finally
                {
                    LogoutDatabase();
                }
            }

            return Login();
        }

        private Boolean Login()
        {
            // BarCodeController barCodeController;
            Boolean isLoginOk = false;
            string userBarcode = "";


            if (Config.GetApplicationMode() == Config.ApplicationMode.Lab)
            {
                // LAB MODE
                // Enable background bar code listening.
                KeyPreview = true;

                if (!LoginDataBase())
                {
                    return false;
                }

                do
                {
                    // Get login information.
                    MyLoginWithBarcodeDialog = new LoginWithBarcodeDialog();

                    // Login by bar code is currently not supported.
                    // barCodeController = new BarCodeController(MyLoginForm);
                    // barCodeController.BarCodeReceived += ExecuteBarCode;
                    if (MyLoginWithBarcodeDialog.ShowDialog() == DialogResult.OK)
                    {
                        userBarcode = MyLoginWithBarcodeDialog.Barcode;
                        MyLoginWithBarcodeDialog = null;
                    }
                    else
                    {
                        // The user cancelled.
                        return false;
                    }
                }   // Try to login to the database.
                while (!SetAuthorityMappingForBarcode(userBarcode));

                UserManager.Refresh();
                Text = Config.GetDialogTitleStandard() + " - " + UserManager.GetCurrentUser().GetName();

                // Start the activity timer. It is used for automatic logout in lab mode.
                this.ActivityTimer.Enabled = true;
                MyActivityCounter = 0;
                isLoginOk = true;
            }
            else
            {
                // OFFICE MODE
                isLoginOk = LoginDataBase();
                SetAuthorityMappingFromSysUser();
            }

            if (!UserManager.GetCurrentUser().IsAccountActive())
            {
                throw new Exception("User " + UserManager.GetCurrentUser().GetName() + " has been inactivated");
            }

            if (isLoginOk)
            {
                // Cache data.
                // This is done in order to avoid DateReader already open exception.
                ChiasmaDepData.Refresh();
                //ActivityTimer.Enabled = true;
            }

            return isLoginOk;
        }

        private Boolean LoginDataBase()
        {
            return LoginDataBase(null, null);
        }

        private Boolean LoginDataBase(String userName, String password)
        {
            // Set newLoginInfo to "null" for integrated security, or to a user name and password for manual login.

            try
            {
                // Try to connect to the database.
                ChiasmaDepData.Database = new Database.Dataserver(userName, password);
                if (!ChiasmaDepData.Database.Connect())
                {
                    throw new Exception("Could not connect user " + userName + " to database");
                }

            }
            catch (Exception exception)
            {
                HandleError("Unable to connect to the database.", exception);
                return false;
            }
            return true;
        }

        private void Logout()
        {
            // Close the child forms.
            foreach (Form child in MdiChildren)
            {
                child.Close();
            }

            LogoutDatabase();
            Text = Config.GetDialogTitleStandard();

            // Reset the login information
            ActivityTimer.Enabled = false;
        }

        public static void LogoutDatabase()
        {
            // Disconnect the data server.
            if (IsNotNull(ChiasmaDepData.Database))
            {
                UserManager.ReleaseAuthorityMapping();
                ChiasmaDepData.Database.Disconnect();
                ChiasmaDepData.Database = null;
            }
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String manualFileName;

            try
            {
                manualFileName = "\"" + Config.GetApplicationPath() + "\\ChiasmaDeposit User Manual.docx" + "\"";
                Process.Start("WinWord.exe", manualFileName);
            }
            catch (Exception exception)
            {
                HandleError("Error when attempting to show manual", exception);
            }
        }

        private void MoveContainer(IGenericContainer moveContainer,
                         IGenericContainer toContainer, int userId)
        {
            // Moves containers into target container. If tocontainer is null
            // the moveContainers is moved out from currect location
            try
            {
                if (toContainer.GetContainerType() == ContainerType.Uncontained)
                {
                    toContainer = null;
                }
                // Move the containers.
                ChiasmaDepData.BeginTransaction();
                GenericContainerManager.MoveGenericContainer(moveContainer, toContainer, userId);
                ChiasmaDepData.CommitTransaction();
            }
            catch
            {
                ChiasmaDepData.RollbackTransaction();
                throw;
            }
        }

        private void Reset()
        {
            InitListView();
            SampleStorageListView.Select();
            //ActivityTimer.Stop();
        }

        private void RestartTimer()
        {
            MyActivityCounter = 0;
            ActivityTimer.Start();
            ShowTimeLeft();
        }


        private void ResetButton_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private GenericContainerList GetDoublets()
        {
            // Check if the same sample appears twice among 
            GenericContainerList doublets = new GenericContainerList();
            Dictionary<int, string> idDict = new Dictionary<int, string>();
            foreach (DuoViewItem viewItem in SampleStorageListView.Items)
            {
                if (viewItem.Checked)
                {
                    if (!idDict.ContainsKey(viewItem.GetSampleContainer().GetId()))
                    {
                        idDict.Add(viewItem.GetSampleContainer().GetId(), "");
                    }
                    else
                    {
                        doublets.Add(viewItem.GetSampleContainer());
                    }
                }
            }
            return doublets;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Get every checked row (SampleStorageDuos) that are okey, 
            // update database for each of them
            GenericContainerList doublets;
            DoubletsDialog doubletsDialog;
            try
            {
                doublets = GetDoublets();
                if (IsNotEmpty(doublets))
                {
                    doubletsDialog = new DoubletsDialog(doublets);
                    doubletsDialog.ShowDialog();
                    SampleStorageListView.Select();
                    return;
                }

                foreach (DuoViewItem duoView in SampleStorageListView.Items)
                {
                    if (duoView.Checked)
                    {
                        MoveContainer(duoView.GetSampleContainer(), duoView.GetStorageContainer(), UserManager.GetCurrentUser().GetId());
                    }
                }
                MessageBox.Show(this,
                                "Update is completed!",
                               Config.GetDialogTitleStandard(),
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
                Reset();

            }
            catch (Exception exception)
            {
                HandleError("Could not update locations of sample containers", exception);
            }
            SampleStorageListView.Select();
        }

        protected void ShowWarning(String message)
        {
            MessageBox.Show(message,
                           Config.GetDialogTitleStandard(),
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Exclamation);
        }

        private void ShowTimeLeft()
        {
            int timeleft, mins, secs;
            String timeString;
            timeleft = Config.GetAutomaticLogoutTimeLimit() - MyActivityCounter;
            mins = timeleft / 60;
            secs = timeleft % 60;
            timeString = "Logout in " + mins + " minutes and " + secs + " seconds";

        }

        private class DuoViewItem : ListViewItem
        {
            private LoadSampleStorageDuoDialog MyDuoDialog;
            private IGenericContainer MyDeposit;
            private IGenericContainer MySampleContainer;
            private string MyContainerPath;

            public enum ListIndex : int
            {
                SampleContainer = 0,
                Storage = 1,
                Status = 2
            }

            public DuoViewItem(IGenericContainer deposit, IGenericContainer container)
                : base(container.GetIdentifier())
            {
                MyDeposit = deposit;
                MySampleContainer = container;
                MyContainerPath = "";
                this.SubItems.Add(deposit.GetIdentifier());
                this.Checked = true;
            }

            public DuoViewItem(ISampleStorageDuo sampleStorageDuo, LoadSampleStorageDuoDialog duoDialog)
                : base(sampleStorageDuo.GetSampleContainerName())
            {
                MyDuoDialog = duoDialog;
                MyContainerPath = "";
                this.Checked = sampleStorageDuo.IsChecked();
                this.Selected = true;
                this.UseItemStyleForSubItems = false;
                this.SubItems.Add(sampleStorageDuo.GetContainerPath());
            }

            public IGenericContainer GetSampleContainer()
            {
                return MySampleContainer;
            }

            public String GetContainerPath()
            {
                LoadContainerPath();
                return MyContainerPath;
            }

            private void LoadContainerPath()
            {
                if (IsNotNull(MyDeposit) && MyContainerPath == "")
                {
                    GenericContainerList pathList;
                    StringBuilder pathRow = new StringBuilder();
                    pathList = MyDeposit.GetContainerPath();
                    foreach (IGenericContainer singleContainer in pathList)
                    {
                        pathRow.Append("//");
                        pathRow.Append(singleContainer.GetIdentifier());
                    }
                    pathRow.Append("//");
                    pathRow.Append(MyDeposit.GetIdentifier());
                    MyContainerPath = pathRow.ToString();
                }
            }

            public IGenericContainer GetStorageContainer()
            {
                return MyDeposit;
            }

            public void Update()
            {
                this.SubItems[(int)ListIndex.SampleContainer].Text = MySampleContainer.GetIdentifier();
                this.SubItems[(int)ListIndex.Storage].Text = MyDeposit.GetIdentifier();
            }
       }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ExportToFile(String filePath)
        {
            StreamWriter sw = null;
            StringBuilder header = new StringBuilder();
            string textLine;

            try
            {
                sw = new StreamWriter(filePath, false, Encoding.GetEncoding(1252));
                // Get all checked and completed items in list
                header.Append("Move list for ");
                header.Append(UserManager.GetCurrentUser().GetName() + ", ");
                header.Append(DateTime.Today.ToString("D"));
                sw.WriteLine(header.ToString());
                foreach (DuoViewItem viewItem in SampleStorageListView.Items)
                {
                    textLine = viewItem.GetSampleContainer().GetIdentifier() + ", " +
                        viewItem.GetStorageContainer().GetIdentifier();
                    sw.WriteLine(textLine);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to export list!", ex);
            }
            finally
            {
                sw.Close();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filePath;
            MySaveFileDialog.ShowDialog();
            if (MySaveFileDialog.FileName == "")
            {
                return;
            }
            filePath = MySaveFileDialog.FileName;
            ExportToFile(filePath);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument printDoc;
            printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            if (MyPrintDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            StringBuilder printText = new StringBuilder();
            StringBuilder header = new StringBuilder();
            Font myfont = new Font("new times roman", 12, FontStyle.Regular);
            string textLine;
            header.Append("Move list for ");
            header.Append(UserManager.GetCurrentUser().GetName() + ", ");
            header.Append(DateTime.Today.ToString("D"));
            printText.Append(header + "\n");
            foreach (DuoViewItem viewItem in SampleStorageListView.Items)
            {
                textLine = viewItem.GetSampleContainer() + ", " +
                    viewItem.GetContainerPath() + "\n";
                printText.Append(textLine);
            }
            e.Graphics.DrawString(printText.ToString(), myfont, Brushes.Black, 10, 10);
        }

        private void printBarcodeForUncontainedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintBarCodeDialog printBarCodeDialog;
            try
            {
                printBarCodeDialog = new PrintBarCodeDialog();
                printBarCodeDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                HandleError("Could not print barcode!", ex);
            }
        }
    }
}