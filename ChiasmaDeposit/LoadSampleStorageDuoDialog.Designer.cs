namespace Molmed.ChiasmaDep.Dialog
{
    partial class LoadSampleStorageDuoDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadSampleStorageDuoDialog));
            this.SampleStorageListView = new System.Windows.Forms.ListView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.IdentificationTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ActivityTimer = new System.Timers.Timer();
            this.ResetButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.opltionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printBarcodeForUncontainedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MySaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MyPrintDialog = new System.Windows.Forms.PrintDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.ValidationReminderPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ActivityTimer)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SampleStorageListView
            // 
            this.SampleStorageListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SampleStorageListView.CheckBoxes = true;
            this.SampleStorageListView.FullRowSelect = true;
            this.SampleStorageListView.GridLines = true;
            this.SampleStorageListView.Location = new System.Drawing.Point(12, 155);
            this.SampleStorageListView.MultiSelect = false;
            this.SampleStorageListView.Name = "SampleStorageListView";
            this.SampleStorageListView.Size = new System.Drawing.Size(593, 349);
            this.SampleStorageListView.TabIndex = 0;
            this.SampleStorageListView.UseCompatibleStateImageBehavior = false;
            this.SampleStorageListView.View = System.Windows.Forms.View.Details;
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveButton.Location = new System.Drawing.Point(12, 510);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Location = new System.Drawing.Point(530, 510);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 2;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Identification:";
            // 
            // IdentificationTextBox
            // 
            this.IdentificationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IdentificationTextBox.Location = new System.Drawing.Point(85, 102);
            this.IdentificationTextBox.Name = "IdentificationTextBox";
            this.IdentificationTextBox.ReadOnly = true;
            this.IdentificationTextBox.Size = new System.Drawing.Size(520, 20);
            this.IdentificationTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(295, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Sample and Storage Duos (only checked items will be saved)";
            // 
            // ActivityTimer
            // 
            this.ActivityTimer.Enabled = true;
            this.ActivityTimer.Interval = 1000D;
            this.ActivityTimer.SynchronizingObject = this;
            this.ActivityTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.ActivityTimer_Elapsed);
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ResetButton.Location = new System.Drawing.Point(93, 510);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 8;
            this.ResetButton.Text = "Reset Form";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opltionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(617, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // opltionsToolStripMenuItem
            // 
            this.opltionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.printToolStripMenuItem,
            this.printBarcodeForUncontainedToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.opltionsToolStripMenuItem.Name = "opltionsToolStripMenuItem";
            this.opltionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.opltionsToolStripMenuItem.Text = "Options";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.exportToolStripMenuItem.Text = "Export list";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Enabled = false;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.printToolStripMenuItem.Text = "Print list";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // printBarcodeForUncontainedToolStripMenuItem
            // 
            this.printBarcodeForUncontainedToolStripMenuItem.Name = "printBarcodeForUncontainedToolStripMenuItem";
            this.printBarcodeForUncontainedToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.printBarcodeForUncontainedToolStripMenuItem.Text = "Print barcode for Uncontained ...";
            this.printBarcodeForUncontainedToolStripMenuItem.Click += new System.EventHandler(this.printBarcodeForUncontainedToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(243, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.manualToolStripMenuItem.Text = "Manual";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // MySaveFileDialog
            // 
            this.MySaveFileDialog.Filter = "Text files|*.txt";
            // 
            // MyPrintDialog
            // 
            this.MyPrintDialog.UseEXDialog = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(188, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Ready to scan samples or container ...";
            // 
            // ValidationReminderPanel
            // 
            this.ValidationReminderPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ValidationReminderPanel.Location = new System.Drawing.Point(458, 27);
            this.ValidationReminderPanel.Name = "ValidationReminderPanel";
            this.ValidationReminderPanel.Size = new System.Drawing.Size(147, 69);
            this.ValidationReminderPanel.TabIndex = 12;
            // 
            // LoadSampleStorageDuoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ExitButton;
            this.ClientSize = new System.Drawing.Size(617, 544);
            this.Controls.Add(this.ValidationReminderPanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.IdentificationTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.SampleStorageListView);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "LoadSampleStorageDuoDialog";
            this.Text = "Load Sample and Storage Duos";
            ((System.ComponentModel.ISupportInitialize)(this.ActivityTimer)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView SampleStorageListView;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IdentificationTextBox;
        private System.Windows.Forms.Label label3;
        internal System.Timers.Timer ActivityTimer;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem opltionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog MySaveFileDialog;
        private System.Windows.Forms.PrintDialog MyPrintDialog;
        private System.Windows.Forms.ToolStripMenuItem printBarcodeForUncontainedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel ValidationReminderPanel;
    }
}

