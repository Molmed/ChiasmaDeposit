using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using ChiasmaDeposit.Data;
using ChiasmaDeposit.Properties;
using ChiasmaDeposit.UI.SampleListDialogs;
using Molmed.ChiasmaDep;
using Molmed.ChiasmaDep.Data;
using Molmed.ChiasmaDep.Data.Exception;
using BarCodeEventHandler = Molmed.ChiasmaDep.Data.BarCodeEventHandler;

namespace ChiasmaDeposit.UI.LoadSampleStorageDialogs 
{
    public partial class LoadSampleStorageDuoDialog : Form
    {
        private Int32 _activityCounter;
        private Point _lastMousePos;

        private LoginWithBarcodeDialog _loginWithBarcodeDialog;

        private BarCodeEventHandler _barcodeEventhandler;
        private BarCodeController _barCodeController;

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

        delegate void UpdateListViewCallback(GenericContainerList selectedContainers,
            IGenericContainer depositContainer);

        private void ActivityTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Update activity information.
            if (MousePosition == _lastMousePos)
            {
                _activityCounter++;
            }
            else
            {
                _lastMousePos = MousePosition;
                _activityCounter = 0;
            }

            // Check if it is time for automatic logout.
            if (_activityCounter > Config.GetAutomaticLogoutTimeLimit())
            {
                Reset();
            }
        }

        private void BarCodeReceived(String barCode)
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
                _activityCounter = 0;
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

        private void UpdateListView(GenericContainerList selectedContainers, 
            IGenericContainer depositContainer)
        {
            if (SampleStorageListView.InvokeRequired)
            {
                var d = new UpdateListViewCallback(UpdateListView);
                Invoke(d, selectedContainers, depositContainer);
            }
            else
            {
                SampleStorageListView.BeginUpdate();
                foreach (IGenericContainer container in selectedContainers)
                {
                    SampleStorageListView.Items.Add(new DuoViewItem(depositContainer, container));
                }
                SampleStorageListView.EndUpdate();
                SampleStorageListView.Columns[(int)ListIndex.SampleContainer].Width = -2;
            }
        }

        private void HandleReceivedBarCode(string barCode)
        {
            var genericContainer = GenericContainerManager.GetGenericContainerByBarCode(barCode);
            if (!CheckContainerType(genericContainer))
            {
                throw new DataException("This bar code neither represent a sample container nor a deposit");
            }
            var sampleListDialog = new SampleListDialog(genericContainer);
            _barCodeController = null;
            if (sampleListDialog.ShowDialog() == DialogResult.OK)
            {
                var depositContainer = sampleListDialog.GetSelectedDeposit();
                var selectedContainers = sampleListDialog.GetSelectedContainers();
                var doublets = GetDoublets(selectedContainers, out var uniqueList);
                if (doublets.Count > 0)
                {
                    var doubletsDialog = new DoubletsDialog(doublets, "These duplicates are sorted out from the list!");
                    doubletsDialog.ShowDialog();
                    selectedContainers = uniqueList;
                }
                UpdateListView(selectedContainers, depositContainer);
                printToolStripMenuItem.Enabled = true;
                exportToolStripMenuItem.Enabled = true;
            }
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

        public static void HandleError(String message, Exception exception)
        {
            var errorDialog = new ShowErrorDialog(message, exception);
            errorDialog.ShowDialog();
        }

        private void Init()
        {
            InitListView();
            // Init barcode controller
            _barCodeController = new BarCodeController(this);
            _barcodeEventhandler = BarCodeReceived;
            _barCodeController.BarCodeReceived += _barcodeEventhandler;
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

            var versionProvider = new VersionProvider();
            Text += $", ChiasmaDeposit {versionProvider.GetApplicationVersion()}";
        }

        private void InitListView()
        {
            SampleStorageListView.Clear();
            SampleStorageListView.Columns.Add("Sample container", -2);
            SampleStorageListView.Columns.Add("Storage", -2);
        }

        private static Boolean IsNotEmpty(ICollection collection)
        {
            return ((collection != null) && (collection.Count > 0));
        }

        private static Boolean IsNotNull(Object testObject)
        {
            return (testObject != null);
        }

        private static Boolean IsNull(Object testObject)
        {
            return (testObject == null);
        }

        public bool Login(bool versionControl)
        {
            // BarCodeController barCodeController;
            Boolean isLoginOk;

            if (Config.GetApplicationMode() == Config.ApplicationMode.Lab)
            {
                // LAB MODE
                // Enable background bar code listening.
                KeyPreview = true;

                if (!LoginDataBase())
                {
                    return false;
                }

                string barcode;
                do
                {
                    // Get login information.
                    _loginWithBarcodeDialog = new LoginWithBarcodeDialog();

                    // Login by bar code is currently not supported.
                    // barCodeController = new BarCodeController(MyLoginForm);
                    // barCodeController.BarCodeReceived += ExecuteBarCode;
                    if (_loginWithBarcodeDialog.ShowDialog() == DialogResult.OK)
                    {
                        barcode = _loginWithBarcodeDialog.Barcode;
                        _loginWithBarcodeDialog = null;
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
                _activityCounter = 0;
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
                    var version = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                                     Assembly.GetExecutingAssembly().GetName().Version.Minor + "." +
                                     Assembly.GetExecutingAssembly().GetName().Version.Build + "." +
                                     Assembly.GetExecutingAssembly().GetName().Version.Revision;

                    // Get the name of the current assembly.
                    var applicationName = Assembly.GetExecutingAssembly().GetName().Name;

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
            catch (Exception ex)
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


        private Boolean LoginDataBase(String userName = null, String password = null)
        {
            // Set newLoginInfo to "null" for integrated security, or to a user name and password for manual login.

            try
            {
                // Try to connect to the database.
                ChiasmaDepData.Database = new Molmed.ChiasmaDep.Database.Dataserver(userName, password);
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
            try
            {
                var manualFileName = "\"" + Config.GetApplicationPath() + "\\ChiasmaDeposit User Manual.docx" + "\"";
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
            try
            {
                var doublets = GetDoublets();
                if (IsNotEmpty(doublets))
                {
                    var doubletsDialog = new DoubletsDialog(doublets);
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

        private void ShowWarning(String message)
        {
            MessageBox.Show(message,
                           Config.GetDialogTitleStandard(),
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Exclamation);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ExportToFile(String filePath)
        {
            StreamWriter sw = null;
            StringBuilder header = new StringBuilder();

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
                    var textLine = viewItem.GetSampleContainer().GetIdentifier() + ", " +
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
                if (sw != null) sw.Close();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySaveFileDialog.ShowDialog();
            if (MySaveFileDialog.FileName == "")
            {
                return;
            }
            var filePath = MySaveFileDialog.FileName;
            ExportToFile(filePath);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var printDoc = new PrintDocument();
            printDoc.PrintPage += printDoc_PrintPage;
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
            header.Append("Move list for ");
            header.Append(UserManager.GetCurrentUser().GetName() + ", ");
            header.Append(DateTime.Today.ToString("D"));
            printText.Append(header + "\n");
            foreach (DuoViewItem viewItem in SampleStorageListView.Items)
            {
                var textLine = viewItem.GetSampleContainer() + ", " +
                                  viewItem.GetContainerPath() + "\n";
                printText.Append(textLine);
            }
            e.Graphics.DrawString(printText.ToString(), myfont, Brushes.Black, 10, 10);
        }

        private void printBarcodeForUncontainedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var printBarCodeDialog = new PrintBarCodeDialog();
                printBarCodeDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                HandleError("Could not print barcode!", ex);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog();
        }
    }
}