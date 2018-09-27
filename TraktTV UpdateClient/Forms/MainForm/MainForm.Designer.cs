using TraktTVUpdateClient.Extension;

namespace TraktTVUpdateClient
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.showPosterBox = new System.Windows.Forms.PictureBox();
            this.showNameLabel = new System.Windows.Forms.Label();
            this.episodeLabel = new System.Windows.Forms.Label();
            this.currentEpisodeTextBox = new System.Windows.Forms.TextBox();
            this.episodeCountLabel = new System.Windows.Forms.Label();
            this.removeEpisodeButton = new System.Windows.Forms.Button();
            this.addEpisodeButton = new System.Windows.Forms.Button();
            this.yearLabel = new System.Windows.Forms.Label();
            this.genreLabel = new System.Windows.Forms.Label();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.scoreComboBox = new System.Windows.Forms.ComboBox();
            this.syncButton = new System.Windows.Forms.Button();
            this.traktConnectStatusLabel = new System.Windows.Forms.Label();
            this.addShowButton = new System.Windows.Forms.Button();
            this.settingButton = new System.Windows.Forms.Button();
            this.vlcConnectStatusLabel = new System.Windows.Forms.Label();
            this.nextUnwatchedEpisodeLbl = new System.Windows.Forms.Label();
            this.relogButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripEventLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.seasonOverviewTreeView = new TraktTVUpdateClient.TreeViewEx();
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scoreColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dataGridViewWatched = new System.Windows.Forms.DataGridView();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressColumn = new TraktTVUpdateClient.Extension.DataGridViewProgressColumn();
            this.ratingColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.showPosterBox)).BeginInit();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWatched)).BeginInit();
            this.SuspendLayout();
            // 
            // showPosterBox
            // 
            this.showPosterBox.Location = new System.Drawing.Point(12, 12);
            this.showPosterBox.Name = "showPosterBox";
            this.showPosterBox.Size = new System.Drawing.Size(100, 150);
            this.showPosterBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.showPosterBox.TabIndex = 0;
            this.showPosterBox.TabStop = false;
            // 
            // showNameLabel
            // 
            this.showNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showNameLabel.AutoSize = true;
            this.showNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.showNameLabel.Location = new System.Drawing.Point(118, 12);
            this.showNameLabel.MaximumSize = new System.Drawing.Size(350, 24);
            this.showNameLabel.Name = "showNameLabel";
            this.showNameLabel.Size = new System.Drawing.Size(260, 24);
            this.showNameLabel.TabIndex = 1;
            this.showNameLabel.Text = "_________________________";
            // 
            // episodeLabel
            // 
            this.episodeLabel.AutoSize = true;
            this.episodeLabel.Location = new System.Drawing.Point(118, 45);
            this.episodeLabel.Name = "episodeLabel";
            this.episodeLabel.Size = new System.Drawing.Size(56, 13);
            this.episodeLabel.TabIndex = 2;
            this.episodeLabel.Text = "Episodes: ";
            // 
            // currentEpisodeTextBox
            // 
            this.currentEpisodeTextBox.Location = new System.Drawing.Point(180, 41);
            this.currentEpisodeTextBox.Name = "currentEpisodeTextBox";
            this.currentEpisodeTextBox.Size = new System.Drawing.Size(31, 20);
            this.currentEpisodeTextBox.TabIndex = 3;
            this.currentEpisodeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CurrentEpisodeTextBox_KeyDown);
            // 
            // episodeCountLabel
            // 
            this.episodeCountLabel.AutoSize = true;
            this.episodeCountLabel.Location = new System.Drawing.Point(217, 44);
            this.episodeCountLabel.Name = "episodeCountLabel";
            this.episodeCountLabel.Size = new System.Drawing.Size(15, 13);
            this.episodeCountLabel.TabIndex = 4;
            this.episodeCountLabel.Text = "/ ";
            // 
            // removeEpisodeButton
            // 
            this.removeEpisodeButton.Location = new System.Drawing.Point(256, 41);
            this.removeEpisodeButton.Name = "removeEpisodeButton";
            this.removeEpisodeButton.Size = new System.Drawing.Size(20, 20);
            this.removeEpisodeButton.TabIndex = 5;
            this.removeEpisodeButton.Text = "-";
            this.removeEpisodeButton.UseVisualStyleBackColor = true;
            this.removeEpisodeButton.Click += new System.EventHandler(this.RemoveEpisodeButton_Click);
            // 
            // addEpisodeButton
            // 
            this.addEpisodeButton.Location = new System.Drawing.Point(282, 41);
            this.addEpisodeButton.Name = "addEpisodeButton";
            this.addEpisodeButton.Size = new System.Drawing.Size(20, 20);
            this.addEpisodeButton.TabIndex = 6;
            this.addEpisodeButton.Text = "+";
            this.addEpisodeButton.UseVisualStyleBackColor = true;
            this.addEpisodeButton.Click += new System.EventHandler(this.AddEpisodeButton_Click);
            // 
            // yearLabel
            // 
            this.yearLabel.AutoSize = true;
            this.yearLabel.Location = new System.Drawing.Point(119, 66);
            this.yearLabel.Name = "yearLabel";
            this.yearLabel.Size = new System.Drawing.Size(35, 13);
            this.yearLabel.TabIndex = 8;
            this.yearLabel.Text = "Year: ";
            // 
            // genreLabel
            // 
            this.genreLabel.AutoSize = true;
            this.genreLabel.Location = new System.Drawing.Point(119, 87);
            this.genreLabel.Name = "genreLabel";
            this.genreLabel.Size = new System.Drawing.Size(42, 13);
            this.genreLabel.TabIndex = 9;
            this.genreLabel.Text = "Genre: ";
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Location = new System.Drawing.Point(118, 108);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(41, 13);
            this.scoreLabel.TabIndex = 11;
            this.scoreLabel.Text = "Score: ";
            // 
            // scoreComboBox
            // 
            this.scoreComboBox.FormattingEnabled = true;
            this.scoreComboBox.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.scoreComboBox.Location = new System.Drawing.Point(165, 105);
            this.scoreComboBox.Name = "scoreComboBox";
            this.scoreComboBox.Size = new System.Drawing.Size(46, 21);
            this.scoreComboBox.TabIndex = 12;
            this.scoreComboBox.SelectedIndexChanged += new System.EventHandler(this.ScoreComboBox_SelectedIndexChanged);
            // 
            // syncButton
            // 
            this.syncButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.syncButton.BackgroundImage = global::TraktTVUpdateClient.Properties.Resources.Update;
            this.syncButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.syncButton.Location = new System.Drawing.Point(499, 98);
            this.syncButton.Name = "syncButton";
            this.syncButton.Size = new System.Drawing.Size(32, 32);
            this.syncButton.TabIndex = 13;
            this.syncButton.UseVisualStyleBackColor = true;
            this.syncButton.Click += new System.EventHandler(this.SyncButton_Click);
            // 
            // traktConnectStatusLabel
            // 
            this.traktConnectStatusLabel.AutoSize = true;
            this.traktConnectStatusLabel.Location = new System.Drawing.Point(119, 129);
            this.traktConnectStatusLabel.Name = "traktConnectStatusLabel";
            this.traktConnectStatusLabel.Size = new System.Drawing.Size(151, 13);
            this.traktConnectStatusLabel.TabIndex = 15;
            this.traktConnectStatusLabel.Text = "trakt.tv Status:  not connected";
            // 
            // addShowButton
            // 
            this.addShowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addShowButton.BackgroundImage = global::TraktTVUpdateClient.Properties.Resources.Search;
            this.addShowButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.addShowButton.Location = new System.Drawing.Point(499, 34);
            this.addShowButton.Name = "addShowButton";
            this.addShowButton.Size = new System.Drawing.Size(32, 32);
            this.addShowButton.TabIndex = 16;
            this.addShowButton.UseVisualStyleBackColor = true;
            this.addShowButton.Click += new System.EventHandler(this.AddShowButton_Click);
            // 
            // settingButton
            // 
            this.settingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingButton.BackgroundImage = global::TraktTVUpdateClient.Properties.Resources.Settings;
            this.settingButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.settingButton.Location = new System.Drawing.Point(499, 3);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(32, 32);
            this.settingButton.TabIndex = 17;
            this.settingButton.UseVisualStyleBackColor = true;
            this.settingButton.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // vlcConnectStatusLabel
            // 
            this.vlcConnectStatusLabel.AutoSize = true;
            this.vlcConnectStatusLabel.Location = new System.Drawing.Point(118, 149);
            this.vlcConnectStatusLabel.Name = "vlcConnectStatusLabel";
            this.vlcConnectStatusLabel.Size = new System.Drawing.Size(138, 13);
            this.vlcConnectStatusLabel.TabIndex = 18;
            this.vlcConnectStatusLabel.Text = "VLC Status:  not connected";
            // 
            // nextUnwatchedEpisodeLbl
            // 
            this.nextUnwatchedEpisodeLbl.AutoSize = true;
            this.nextUnwatchedEpisodeLbl.Location = new System.Drawing.Point(308, 44);
            this.nextUnwatchedEpisodeLbl.Name = "nextUnwatchedEpisodeLbl";
            this.nextUnwatchedEpisodeLbl.Size = new System.Drawing.Size(76, 13);
            this.nextUnwatchedEpisodeLbl.TabIndex = 20;
            this.nextUnwatchedEpisodeLbl.Text = "Next Episode: ";
            // 
            // relogButton
            // 
            this.relogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.relogButton.BackgroundImage = global::TraktTVUpdateClient.Properties.Resources.Lock;
            this.relogButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.relogButton.Location = new System.Drawing.Point(499, 66);
            this.relogButton.Name = "relogButton";
            this.relogButton.Size = new System.Drawing.Size(32, 32);
            this.relogButton.TabIndex = 22;
            this.relogButton.UseVisualStyleBackColor = true;
            this.relogButton.Click += new System.EventHandler(this.RelogButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEventLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 366);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(694, 22);
            this.statusStrip.TabIndex = 23;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripEventLabel
            // 
            this.toolStripEventLabel.Name = "toolStripEventLabel";
            this.toolStripEventLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // seasonOverviewTreeView
            // 
            this.seasonOverviewTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seasonOverviewTreeView.CheckBoxes = true;
            this.seasonOverviewTreeView.Location = new System.Drawing.Point(533, 3);
            this.seasonOverviewTreeView.Name = "seasonOverviewTreeView";
            this.seasonOverviewTreeView.Size = new System.Drawing.Size(145, 362);
            this.seasonOverviewTreeView.TabIndex = 19;
            this.seasonOverviewTreeView.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.SeasonOverviewTreeView_BeforeCheck);
            this.seasonOverviewTreeView.DoubleClick += new System.EventHandler(this.SeasonOverviewTreeView_DoubleClick);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 263;
            // 
            // progressColumnHeader
            // 
            this.progressColumnHeader.Text = "Progress";
            this.progressColumnHeader.Width = 190;
            // 
            // scoreColumnHeader
            // 
            this.scoreColumnHeader.Text = "Score";
            this.scoreColumnHeader.Width = 45;
            // 
            // dataGridViewWatched
            // 
            this.dataGridViewWatched.AllowUserToAddRows = false;
            this.dataGridViewWatched.AllowUserToDeleteRows = false;
            this.dataGridViewWatched.AllowUserToResizeColumns = false;
            this.dataGridViewWatched.AllowUserToResizeRows = false;
            this.dataGridViewWatched.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewWatched.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewWatched.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewWatched.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWatched.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.progressColumn,
            this.ratingColumn});
            this.dataGridViewWatched.Location = new System.Drawing.Point(12, 168);
            this.dataGridViewWatched.MultiSelect = false;
            this.dataGridViewWatched.Name = "dataGridViewWatched";
            this.dataGridViewWatched.ReadOnly = true;
            this.dataGridViewWatched.RowHeadersVisible = false;
            this.dataGridViewWatched.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewWatched.Size = new System.Drawing.Size(515, 195);
            this.dataGridViewWatched.TabIndex = 24;
            this.dataGridViewWatched.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewWatched_ColumnHeaderMouseClick);
            this.dataGridViewWatched.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewWatched_RowEnter);
            this.dataGridViewWatched.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.DataGridViewWatched_SortCompare);
            // 
            // nameColumn
            // 
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // progressColumn
            // 
            this.progressColumn.FillWeight = 50F;
            this.progressColumn.HeaderText = "Progress";
            this.progressColumn.Name = "progressColumn";
            this.progressColumn.ProgressBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(192)))), ((int)(((byte)(222)))));
            this.progressColumn.ReadOnly = true;
            this.progressColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ratingColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ratingColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.ratingColumn.FillWeight = 20F;
            this.ratingColumn.HeaderText = "Rating";
            this.ratingColumn.Name = "ratingColumn";
            this.ratingColumn.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 388);
            this.Controls.Add(this.dataGridViewWatched);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.relogButton);
            this.Controls.Add(this.nextUnwatchedEpisodeLbl);
            this.Controls.Add(this.seasonOverviewTreeView);
            this.Controls.Add(this.vlcConnectStatusLabel);
            this.Controls.Add(this.settingButton);
            this.Controls.Add(this.addShowButton);
            this.Controls.Add(this.traktConnectStatusLabel);
            this.Controls.Add(this.syncButton);
            this.Controls.Add(this.scoreComboBox);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.genreLabel);
            this.Controls.Add(this.yearLabel);
            this.Controls.Add(this.addEpisodeButton);
            this.Controls.Add(this.removeEpisodeButton);
            this.Controls.Add(this.episodeCountLabel);
            this.Controls.Add(this.currentEpisodeTextBox);
            this.Controls.Add(this.episodeLabel);
            this.Controls.Add(this.showNameLabel);
            this.Controls.Add(this.showPosterBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(710, 426);
            this.Name = "MainForm";
            this.Text = "trakt.tv Updater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.showPosterBox)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWatched)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private System.Windows.Forms.PictureBox showPosterBox;
        private System.Windows.Forms.Label showNameLabel;
        private System.Windows.Forms.Label episodeLabel;
        private System.Windows.Forms.TextBox currentEpisodeTextBox;
        private System.Windows.Forms.Label episodeCountLabel;
        private System.Windows.Forms.Button removeEpisodeButton;
        private System.Windows.Forms.Button addEpisodeButton;
        private System.Windows.Forms.Label yearLabel;
        private System.Windows.Forms.Label genreLabel;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.ComboBox scoreComboBox;
        private System.Windows.Forms.Button syncButton;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader progressColumnHeader;
        private System.Windows.Forms.ColumnHeader scoreColumnHeader;
        private System.Windows.Forms.Label traktConnectStatusLabel;
        private System.Windows.Forms.Button addShowButton;
        private System.Windows.Forms.Button settingButton;
        private System.Windows.Forms.Label vlcConnectStatusLabel;
        private TreeViewEx seasonOverviewTreeView;
        private System.Windows.Forms.Label nextUnwatchedEpisodeLbl;
        private System.Windows.Forms.Button relogButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEventLabel;
        private System.Windows.Forms.DataGridView dataGridViewWatched;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private DataGridViewProgressColumn progressColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ratingColumn;
    }
}

