using System;

namespace TraktTVUpdateClient.Forms
{
    partial class SettingsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.vlcPortLbl = new System.Windows.Forms.Label();
            this.vlcPortTxtBox = new System.Windows.Forms.TextBox();
            this.markEpisodeLbl = new System.Windows.Forms.Label();
            this.watchedPercentTrackBar = new System.Windows.Forms.TrackBar();
            this.watchedPercentLbl = new System.Windows.Forms.Label();
            this.saveSettingsBtn = new System.Windows.Forms.Button();
            this.enableVLCCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonRemoveLibrary = new System.Windows.Forms.Button();
            this.buttonAddLibrary = new System.Windows.Forms.Button();
            this.dataGridViewLibraries = new System.Windows.Forms.DataGridView();
            this.ColumnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSourceLibraries = new System.Windows.Forms.BindingSource(this.components);
            this.treeViewFoundSeries = new System.Windows.Forms.TreeView();
            this.buttonReload = new System.Windows.Forms.Button();
            this.richTextBoxMediaInfo = new System.Windows.Forms.RichTextBox();
            this.buttonReportParsingError = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.watchedPercentTrackBar)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLibraries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceLibraries)).BeginInit();
            this.SuspendLayout();
            // 
            // vlcPortLbl
            // 
            this.vlcPortLbl.AutoSize = true;
            this.vlcPortLbl.Location = new System.Drawing.Point(6, 52);
            this.vlcPortLbl.Name = "vlcPortLbl";
            this.vlcPortLbl.Size = new System.Drawing.Size(55, 13);
            this.vlcPortLbl.TabIndex = 0;
            this.vlcPortLbl.Text = "VLC Port: ";
            // 
            // vlcPortTxtBox
            // 
            this.vlcPortTxtBox.Location = new System.Drawing.Point(67, 49);
            this.vlcPortTxtBox.Name = "vlcPortTxtBox";
            this.vlcPortTxtBox.Size = new System.Drawing.Size(65, 20);
            this.vlcPortTxtBox.TabIndex = 1;
            this.vlcPortTxtBox.Text = "2150";
            // 
            // markEpisodeLbl
            // 
            this.markEpisodeLbl.AutoSize = true;
            this.markEpisodeLbl.Location = new System.Drawing.Point(5, 81);
            this.markEpisodeLbl.Name = "markEpisodeLbl";
            this.markEpisodeLbl.Size = new System.Drawing.Size(127, 13);
            this.markEpisodeLbl.TabIndex = 2;
            this.markEpisodeLbl.Text = "Mark episode watched at";
            // 
            // watchedPercentTrackBar
            // 
            this.watchedPercentTrackBar.Location = new System.Drawing.Point(138, 55);
            this.watchedPercentTrackBar.Maximum = 100;
            this.watchedPercentTrackBar.Minimum = 50;
            this.watchedPercentTrackBar.Name = "watchedPercentTrackBar";
            this.watchedPercentTrackBar.Size = new System.Drawing.Size(127, 45);
            this.watchedPercentTrackBar.TabIndex = 3;
            this.watchedPercentTrackBar.Value = 50;
            this.watchedPercentTrackBar.ValueChanged += new System.EventHandler(this.WatchedPercentTrackBar_ValueChanged);
            // 
            // watchedPercentLbl
            // 
            this.watchedPercentLbl.AutoSize = true;
            this.watchedPercentLbl.Location = new System.Drawing.Point(268, 81);
            this.watchedPercentLbl.Name = "watchedPercentLbl";
            this.watchedPercentLbl.Size = new System.Drawing.Size(76, 13);
            this.watchedPercentLbl.TabIndex = 4;
            this.watchedPercentLbl.Text = "90% of the title";
            // 
            // saveSettingsBtn
            // 
            this.saveSettingsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveSettingsBtn.Location = new System.Drawing.Point(12, 346);
            this.saveSettingsBtn.Name = "saveSettingsBtn";
            this.saveSettingsBtn.Size = new System.Drawing.Size(360, 23);
            this.saveSettingsBtn.TabIndex = 5;
            this.saveSettingsBtn.Text = "Save";
            this.saveSettingsBtn.UseVisualStyleBackColor = true;
            this.saveSettingsBtn.Click += new System.EventHandler(this.SaveSettingsBtn_Click);
            // 
            // enableVLCCheckBox
            // 
            this.enableVLCCheckBox.AutoSize = true;
            this.enableVLCCheckBox.Location = new System.Drawing.Point(9, 23);
            this.enableVLCCheckBox.Name = "enableVLCCheckBox";
            this.enableVLCCheckBox.Size = new System.Drawing.Size(82, 17);
            this.enableVLCCheckBox.TabIndex = 6;
            this.enableVLCCheckBox.Text = "Enable VLC";
            this.enableVLCCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.enableVLCCheckBox);
            this.groupBox1.Controls.Add(this.vlcPortLbl);
            this.groupBox1.Controls.Add(this.vlcPortTxtBox);
            this.groupBox1.Controls.Add(this.watchedPercentLbl);
            this.groupBox1.Controls.Add(this.markEpisodeLbl);
            this.groupBox1.Controls.Add(this.watchedPercentTrackBar);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 116);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "VLC Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.buttonRemoveLibrary);
            this.groupBox2.Controls.Add(this.buttonAddLibrary);
            this.groupBox2.Controls.Add(this.dataGridViewLibraries);
            this.groupBox2.Location = new System.Drawing.Point(12, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 206);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Media Libraries";
            // 
            // buttonRemoveLibrary
            // 
            this.buttonRemoveLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveLibrary.Location = new System.Drawing.Point(228, 177);
            this.buttonRemoveLibrary.Name = "buttonRemoveLibrary";
            this.buttonRemoveLibrary.Size = new System.Drawing.Size(126, 23);
            this.buttonRemoveLibrary.TabIndex = 2;
            this.buttonRemoveLibrary.Text = "Remove library";
            this.buttonRemoveLibrary.UseVisualStyleBackColor = true;
            this.buttonRemoveLibrary.Click += new System.EventHandler(this.ButtonRemoveLibrary_Click);
            // 
            // buttonAddLibrary
            // 
            this.buttonAddLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddLibrary.Location = new System.Drawing.Point(6, 177);
            this.buttonAddLibrary.Name = "buttonAddLibrary";
            this.buttonAddLibrary.Size = new System.Drawing.Size(126, 23);
            this.buttonAddLibrary.TabIndex = 1;
            this.buttonAddLibrary.Text = "Add library";
            this.buttonAddLibrary.UseVisualStyleBackColor = true;
            this.buttonAddLibrary.Click += new System.EventHandler(this.ButtonAddLibrary_Click);
            // 
            // dataGridViewLibraries
            // 
            this.dataGridViewLibraries.AllowUserToAddRows = false;
            this.dataGridViewLibraries.AllowUserToDeleteRows = false;
            this.dataGridViewLibraries.AllowUserToResizeColumns = false;
            this.dataGridViewLibraries.AllowUserToResizeRows = false;
            this.dataGridViewLibraries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewLibraries.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLibraries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLibraries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnPath,
            this.ColumnLanguage});
            this.dataGridViewLibraries.Location = new System.Drawing.Point(6, 19);
            this.dataGridViewLibraries.MultiSelect = false;
            this.dataGridViewLibraries.Name = "dataGridViewLibraries";
            this.dataGridViewLibraries.RowHeadersVisible = false;
            this.dataGridViewLibraries.Size = new System.Drawing.Size(348, 152);
            this.dataGridViewLibraries.TabIndex = 0;
            this.dataGridViewLibraries.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewLibraries_CellEndEdit);
            this.dataGridViewLibraries.SelectionChanged += new System.EventHandler(this.DataGridViewLibraries_SelectionChanged);
            // 
            // ColumnPath
            // 
            this.ColumnPath.DataPropertyName = "LibraryPath";
            this.ColumnPath.FillWeight = 65F;
            this.ColumnPath.HeaderText = "Path";
            this.ColumnPath.Name = "ColumnPath";
            // 
            // ColumnLanguage
            // 
            this.ColumnLanguage.DataPropertyName = "Language";
            this.ColumnLanguage.FillWeight = 35F;
            this.ColumnLanguage.HeaderText = "Language";
            this.ColumnLanguage.Name = "ColumnLanguage";
            // 
            // treeViewFoundSeries
            // 
            this.treeViewFoundSeries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewFoundSeries.Location = new System.Drawing.Point(378, 12);
            this.treeViewFoundSeries.Name = "treeViewFoundSeries";
            this.treeViewFoundSeries.Size = new System.Drawing.Size(276, 236);
            this.treeViewFoundSeries.TabIndex = 9;
            this.treeViewFoundSeries.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewFoundSeries_AfterSelect);
            // 
            // buttonReload
            // 
            this.buttonReload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReload.Location = new System.Drawing.Point(378, 254);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(276, 23);
            this.buttonReload.TabIndex = 10;
            this.buttonReload.Text = "Reload files";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.ButtonReload_Click);
            // 
            // richTextBoxMediaInfo
            // 
            this.richTextBoxMediaInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxMediaInfo.DetectUrls = false;
            this.richTextBoxMediaInfo.Location = new System.Drawing.Point(378, 283);
            this.richTextBoxMediaInfo.Name = "richTextBoxMediaInfo";
            this.richTextBoxMediaInfo.ReadOnly = true;
            this.richTextBoxMediaInfo.Size = new System.Drawing.Size(276, 57);
            this.richTextBoxMediaInfo.TabIndex = 11;
            this.richTextBoxMediaInfo.Text = "";
            // 
            // buttonReportParsingError
            // 
            this.buttonReportParsingError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReportParsingError.Location = new System.Drawing.Point(378, 346);
            this.buttonReportParsingError.Name = "buttonReportParsingError";
            this.buttonReportParsingError.Size = new System.Drawing.Size(276, 23);
            this.buttonReportParsingError.TabIndex = 12;
            this.buttonReportParsingError.Text = "Report Error!";
            this.buttonReportParsingError.UseVisualStyleBackColor = true;
            this.buttonReportParsingError.Click += new System.EventHandler(this.ButtonReportParsingError_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 381);
            this.Controls.Add(this.buttonReportParsingError);
            this.Controls.Add(this.richTextBoxMediaInfo);
            this.Controls.Add(this.buttonReload);
            this.Controls.Add(this.treeViewFoundSeries);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.saveSettingsBtn);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(682, 419);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.watchedPercentTrackBar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLibraries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceLibraries)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label vlcPortLbl;
        private System.Windows.Forms.TextBox vlcPortTxtBox;
        private System.Windows.Forms.Label markEpisodeLbl;
        private System.Windows.Forms.TrackBar watchedPercentTrackBar;
        private System.Windows.Forms.Label watchedPercentLbl;
        private System.Windows.Forms.Button saveSettingsBtn;
        private System.Windows.Forms.CheckBox enableVLCCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewLibraries;
        private System.Windows.Forms.Button buttonRemoveLibrary;
        private System.Windows.Forms.Button buttonAddLibrary;
        private System.Windows.Forms.BindingSource bindingSourceLibraries;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLanguage;
        private System.Windows.Forms.TreeView treeViewFoundSeries;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.RichTextBox richTextBoxMediaInfo;
        private System.Windows.Forms.Button buttonReportParsingError;
    }
}