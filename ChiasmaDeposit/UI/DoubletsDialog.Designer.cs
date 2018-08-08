namespace ChiasmaDeposit.UI
{
    partial class DoubletsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DoubletsDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PleaseLabel2 = new System.Windows.Forms.Label();
            this.PleaseLabel = new System.Windows.Forms.Label();
            this.DoubletsListBox = new System.Windows.Forms.ListBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.AbortLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "The following sample containers were ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(92, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "entered twice or more!";
            // 
            // PleaseLabel2
            // 
            this.PleaseLabel2.AutoSize = true;
            this.PleaseLabel2.Location = new System.Drawing.Point(92, 45);
            this.PleaseLabel2.Name = "PleaseLabel2";
            this.PleaseLabel2.Size = new System.Drawing.Size(164, 13);
            this.PleaseLabel2.TabIndex = 3;
            this.PleaseLabel2.Text = "Please uncheck rows containing ";
            // 
            // PleaseLabel
            // 
            this.PleaseLabel.AutoSize = true;
            this.PleaseLabel.Location = new System.Drawing.Point(92, 58);
            this.PleaseLabel.Name = "PleaseLabel";
            this.PleaseLabel.Size = new System.Drawing.Size(152, 13);
            this.PleaseLabel.TabIndex = 4;
            this.PleaseLabel.Text = "sample containers from this list.";
            // 
            // DoubletsListBox
            // 
            this.DoubletsListBox.FormattingEnabled = true;
            this.DoubletsListBox.Location = new System.Drawing.Point(95, 112);
            this.DoubletsListBox.Name = "DoubletsListBox";
            this.DoubletsListBox.Size = new System.Drawing.Size(223, 134);
            this.DoubletsListBox.TabIndex = 5;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(115, 252);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // AbortLabel
            // 
            this.AbortLabel.AutoSize = true;
            this.AbortLabel.Location = new System.Drawing.Point(92, 81);
            this.AbortLabel.Name = "AbortLabel";
            this.AbortLabel.Size = new System.Drawing.Size(84, 13);
            this.AbortLabel.TabIndex = 7;
            this.AbortLabel.Text = "Update aborted!";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(46, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // DoubletsDialog
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 287);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.AbortLabel);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.DoubletsListBox);
            this.Controls.Add(this.PleaseLabel);
            this.Controls.Add(this.PleaseLabel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DoubletsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DoubletsDialog";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label PleaseLabel2;
        private System.Windows.Forms.Label PleaseLabel;
        private System.Windows.Forms.ListBox DoubletsListBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Label AbortLabel;
        private System.Windows.Forms.PictureBox pictureBox1;




    }
}