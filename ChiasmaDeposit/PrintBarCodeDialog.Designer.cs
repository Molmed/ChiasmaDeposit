namespace Molmed.ChiasmaDep.Dialog
{
    partial class PrintBarCodeDialog
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
            this.PrintGroupBox = new System.Windows.Forms.GroupBox();
            this.OffsetY = new System.Windows.Forms.NumericUpDown();
            this.OffsetX = new System.Windows.Forms.NumericUpDown();
            this.AdvancedPanel = new System.Windows.Forms.Panel();
            this.PrintServerNameTextBox = new System.Windows.Forms.TextBox();
            this.PrintServerNameLabel = new System.Windows.Forms.Label();
            this.BarCodeTypeComboBox = new System.Windows.Forms.ComboBox();
            this.BarCodeTypeLabel = new System.Windows.Forms.Label();
            this.PrintModeComboBox = new System.Windows.Forms.ComboBox();
            this.PrintModeLabel = new System.Windows.Forms.Label();
            this.AdvancedButton = new System.Windows.Forms.Button();
            this.TextBesideRadioButton = new System.Windows.Forms.RadioButton();
            this.TextAboveRadioButton = new System.Windows.Forms.RadioButton();
            this.BarCodeWidthComboBox = new System.Windows.Forms.ComboBox();
            this.BarCodeWidthLabel = new System.Windows.Forms.Label();
            this.OffsetYLabel = new System.Windows.Forms.Label();
            this.OffsetXLabel = new System.Windows.Forms.Label();
            this.PrintButton = new System.Windows.Forms.Button();
            this.BarCodeHeightComboBox = new System.Windows.Forms.ComboBox();
            this.BarCodeHeightLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ListGroupBox = new System.Windows.Forms.GroupBox();
            this.BarCodeListView = new System.Windows.Forms.ListView();
            this.PrintGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetX)).BeginInit();
            this.AdvancedPanel.SuspendLayout();
            this.ListGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // PrintGroupBox
            // 
            this.PrintGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PrintGroupBox.Controls.Add(this.OffsetY);
            this.PrintGroupBox.Controls.Add(this.OffsetX);
            this.PrintGroupBox.Controls.Add(this.AdvancedPanel);
            this.PrintGroupBox.Controls.Add(this.AdvancedButton);
            this.PrintGroupBox.Controls.Add(this.TextBesideRadioButton);
            this.PrintGroupBox.Controls.Add(this.TextAboveRadioButton);
            this.PrintGroupBox.Controls.Add(this.BarCodeWidthComboBox);
            this.PrintGroupBox.Controls.Add(this.BarCodeWidthLabel);
            this.PrintGroupBox.Controls.Add(this.OffsetYLabel);
            this.PrintGroupBox.Controls.Add(this.OffsetXLabel);
            this.PrintGroupBox.Controls.Add(this.PrintButton);
            this.PrintGroupBox.Controls.Add(this.BarCodeHeightComboBox);
            this.PrintGroupBox.Controls.Add(this.BarCodeHeightLabel);
            this.PrintGroupBox.Location = new System.Drawing.Point(8, 127);
            this.PrintGroupBox.Name = "PrintGroupBox";
            this.PrintGroupBox.Size = new System.Drawing.Size(592, 240);
            this.PrintGroupBox.TabIndex = 3;
            this.PrintGroupBox.TabStop = false;
            this.PrintGroupBox.Text = "Output";
            // 
            // OffsetY
            // 
            this.OffsetY.Location = new System.Drawing.Point(366, 56);
            this.OffsetY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.OffsetY.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.OffsetY.Name = "OffsetY";
            this.OffsetY.Size = new System.Drawing.Size(65, 20);
            this.OffsetY.TabIndex = 4;
            // 
            // OffsetX
            // 
            this.OffsetX.Location = new System.Drawing.Point(366, 25);
            this.OffsetX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.OffsetX.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.OffsetX.Name = "OffsetX";
            this.OffsetX.Size = new System.Drawing.Size(65, 20);
            this.OffsetX.TabIndex = 1;
            // 
            // AdvancedPanel
            // 
            this.AdvancedPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AdvancedPanel.Controls.Add(this.PrintServerNameTextBox);
            this.AdvancedPanel.Controls.Add(this.PrintServerNameLabel);
            this.AdvancedPanel.Controls.Add(this.BarCodeTypeComboBox);
            this.AdvancedPanel.Controls.Add(this.BarCodeTypeLabel);
            this.AdvancedPanel.Controls.Add(this.PrintModeComboBox);
            this.AdvancedPanel.Controls.Add(this.PrintModeLabel);
            this.AdvancedPanel.Location = new System.Drawing.Point(8, 128);
            this.AdvancedPanel.Name = "AdvancedPanel";
            this.AdvancedPanel.Size = new System.Drawing.Size(576, 72);
            this.AdvancedPanel.TabIndex = 24;
            this.AdvancedPanel.Visible = false;
            // 
            // PrintServerNameTextBox
            // 
            this.PrintServerNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PrintServerNameTextBox.Location = new System.Drawing.Point(358, 37);
            this.PrintServerNameTextBox.Name = "PrintServerNameTextBox";
            this.PrintServerNameTextBox.Size = new System.Drawing.Size(210, 20);
            this.PrintServerNameTextBox.TabIndex = 3;
            // 
            // PrintServerNameLabel
            // 
            this.PrintServerNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PrintServerNameLabel.Location = new System.Drawing.Point(358, 11);
            this.PrintServerNameLabel.Name = "PrintServerNameLabel";
            this.PrintServerNameLabel.Size = new System.Drawing.Size(101, 16);
            this.PrintServerNameLabel.TabIndex = 15;
            this.PrintServerNameLabel.Text = "Print server name:";
            // 
            // BarCodeTypeComboBox
            // 
            this.BarCodeTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BarCodeTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BarCodeTypeComboBox.Items.AddRange(new object[] {
            "Code 128 C"});
            this.BarCodeTypeComboBox.Location = new System.Drawing.Point(96, 40);
            this.BarCodeTypeComboBox.Name = "BarCodeTypeComboBox";
            this.BarCodeTypeComboBox.Size = new System.Drawing.Size(168, 21);
            this.BarCodeTypeComboBox.TabIndex = 2;
            // 
            // BarCodeTypeLabel
            // 
            this.BarCodeTypeLabel.Location = new System.Drawing.Point(8, 40);
            this.BarCodeTypeLabel.Name = "BarCodeTypeLabel";
            this.BarCodeTypeLabel.Size = new System.Drawing.Size(80, 16);
            this.BarCodeTypeLabel.TabIndex = 13;
            this.BarCodeTypeLabel.Text = "Bar code type:";
            // 
            // PrintModeComboBox
            // 
            this.PrintModeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PrintModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PrintModeComboBox.Items.AddRange(new object[] {
            "ZPL script"});
            this.PrintModeComboBox.Location = new System.Drawing.Point(96, 8);
            this.PrintModeComboBox.Name = "PrintModeComboBox";
            this.PrintModeComboBox.Size = new System.Drawing.Size(168, 21);
            this.PrintModeComboBox.TabIndex = 0;
            // 
            // PrintModeLabel
            // 
            this.PrintModeLabel.Location = new System.Drawing.Point(8, 11);
            this.PrintModeLabel.Name = "PrintModeLabel";
            this.PrintModeLabel.Size = new System.Drawing.Size(80, 16);
            this.PrintModeLabel.TabIndex = 9;
            this.PrintModeLabel.Text = "Printing mode:";
            // 
            // AdvancedButton
            // 
            this.AdvancedButton.Location = new System.Drawing.Point(16, 96);
            this.AdvancedButton.Name = "AdvancedButton";
            this.AdvancedButton.Size = new System.Drawing.Size(100, 24);
            this.AdvancedButton.TabIndex = 23;
            this.AdvancedButton.Text = "&Advanced >>";
            this.AdvancedButton.Click += new System.EventHandler(this.AdvancedButton_Click);
            // 
            // TextBesideRadioButton
            // 
            this.TextBesideRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBesideRadioButton.Checked = true;
            this.TextBesideRadioButton.Location = new System.Drawing.Point(448, 58);
            this.TextBesideRadioButton.Name = "TextBesideRadioButton";
            this.TextBesideRadioButton.Size = new System.Drawing.Size(128, 16);
            this.TextBesideRadioButton.TabIndex = 5;
            this.TextBesideRadioButton.TabStop = true;
            this.TextBesideRadioButton.Text = "Text beside bar code";
            // 
            // TextAboveRadioButton
            // 
            this.TextAboveRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextAboveRadioButton.Location = new System.Drawing.Point(448, 27);
            this.TextAboveRadioButton.Name = "TextAboveRadioButton";
            this.TextAboveRadioButton.Size = new System.Drawing.Size(128, 16);
            this.TextAboveRadioButton.TabIndex = 2;
            this.TextAboveRadioButton.Text = "Text above bar code";
            // 
            // BarCodeWidthComboBox
            // 
            this.BarCodeWidthComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BarCodeWidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BarCodeWidthComboBox.Items.AddRange(new object[] {
            "Narrow",
            "Wide"});
            this.BarCodeWidthComboBox.Location = new System.Drawing.Point(104, 56);
            this.BarCodeWidthComboBox.Name = "BarCodeWidthComboBox";
            this.BarCodeWidthComboBox.Size = new System.Drawing.Size(168, 21);
            this.BarCodeWidthComboBox.TabIndex = 3;
            // 
            // BarCodeWidthLabel
            // 
            this.BarCodeWidthLabel.Location = new System.Drawing.Point(16, 58);
            this.BarCodeWidthLabel.Name = "BarCodeWidthLabel";
            this.BarCodeWidthLabel.Size = new System.Drawing.Size(82, 16);
            this.BarCodeWidthLabel.TabIndex = 18;
            this.BarCodeWidthLabel.Text = "Bar code width:";
            // 
            // OffsetYLabel
            // 
            this.OffsetYLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OffsetYLabel.Location = new System.Drawing.Point(280, 59);
            this.OffsetYLabel.Name = "OffsetYLabel";
            this.OffsetYLabel.Size = new System.Drawing.Size(80, 16);
            this.OffsetYLabel.TabIndex = 16;
            this.OffsetYLabel.Text = "Offset Y (dots):";
            // 
            // OffsetXLabel
            // 
            this.OffsetXLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OffsetXLabel.Location = new System.Drawing.Point(280, 27);
            this.OffsetXLabel.Name = "OffsetXLabel";
            this.OffsetXLabel.Size = new System.Drawing.Size(80, 16);
            this.OffsetXLabel.TabIndex = 14;
            this.OffsetXLabel.Text = "Offset X (dots):";
            // 
            // PrintButton
            // 
            this.PrintButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PrintButton.Location = new System.Drawing.Point(493, 206);
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.Size = new System.Drawing.Size(83, 24);
            this.PrintButton.TabIndex = 6;
            this.PrintButton.Text = "&Print";
            this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // BarCodeHeightComboBox
            // 
            this.BarCodeHeightComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BarCodeHeightComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BarCodeHeightComboBox.Items.AddRange(new object[] {
            "Short",
            "Medium",
            "Tall"});
            this.BarCodeHeightComboBox.Location = new System.Drawing.Point(104, 24);
            this.BarCodeHeightComboBox.Name = "BarCodeHeightComboBox";
            this.BarCodeHeightComboBox.Size = new System.Drawing.Size(168, 21);
            this.BarCodeHeightComboBox.TabIndex = 0;
            // 
            // BarCodeHeightLabel
            // 
            this.BarCodeHeightLabel.Location = new System.Drawing.Point(16, 27);
            this.BarCodeHeightLabel.Name = "BarCodeHeightLabel";
            this.BarCodeHeightLabel.Size = new System.Drawing.Size(90, 16);
            this.BarCodeHeightLabel.TabIndex = 0;
            this.BarCodeHeightLabel.Text = "Bar code height:";
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(262, 387);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "&Close";
            // 
            // ListGroupBox
            // 
            this.ListGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ListGroupBox.Controls.Add(this.BarCodeListView);
            this.ListGroupBox.Location = new System.Drawing.Point(8, 7);
            this.ListGroupBox.Name = "ListGroupBox";
            this.ListGroupBox.Size = new System.Drawing.Size(592, 114);
            this.ListGroupBox.TabIndex = 1;
            this.ListGroupBox.TabStop = false;
            this.ListGroupBox.Text = "Bar codes";
            // 
            // BarCodeListView
            // 
            this.BarCodeListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BarCodeListView.FullRowSelect = true;
            this.BarCodeListView.GridLines = true;
            this.BarCodeListView.Location = new System.Drawing.Point(16, 24);
            this.BarCodeListView.Name = "BarCodeListView";
            this.BarCodeListView.Size = new System.Drawing.Size(560, 70);
            this.BarCodeListView.TabIndex = 2;
            this.BarCodeListView.UseCompatibleStateImageBehavior = false;
            this.BarCodeListView.View = System.Windows.Forms.View.Details;
            // 
            // PrintBarCodeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(608, 426);
            this.Controls.Add(this.PrintGroupBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ListGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintBarCodeDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print Bar Codes";
            this.PrintGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetX)).EndInit();
            this.AdvancedPanel.ResumeLayout(false);
            this.AdvancedPanel.PerformLayout();
            this.ListGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox PrintGroupBox;
        internal System.Windows.Forms.Panel AdvancedPanel;
        internal System.Windows.Forms.TextBox PrintServerNameTextBox;
        internal System.Windows.Forms.Label PrintServerNameLabel;
        internal System.Windows.Forms.ComboBox BarCodeTypeComboBox;
        internal System.Windows.Forms.Label BarCodeTypeLabel;
        internal System.Windows.Forms.ComboBox PrintModeComboBox;
        internal System.Windows.Forms.Label PrintModeLabel;
        internal System.Windows.Forms.Button AdvancedButton;
        internal System.Windows.Forms.RadioButton TextBesideRadioButton;
        internal System.Windows.Forms.RadioButton TextAboveRadioButton;
        internal System.Windows.Forms.ComboBox BarCodeWidthComboBox;
        internal System.Windows.Forms.Label BarCodeWidthLabel;
        internal System.Windows.Forms.Label OffsetYLabel;
        internal System.Windows.Forms.Label OffsetXLabel;
        internal System.Windows.Forms.Button PrintButton;
        internal System.Windows.Forms.ComboBox BarCodeHeightComboBox;
        internal System.Windows.Forms.Label BarCodeHeightLabel;
        internal System.Windows.Forms.Button CloseButton;
        internal System.Windows.Forms.GroupBox ListGroupBox;
        private System.Windows.Forms.NumericUpDown OffsetY;
        private System.Windows.Forms.NumericUpDown OffsetX;
        private System.Windows.Forms.ListView BarCodeListView;
    }
}