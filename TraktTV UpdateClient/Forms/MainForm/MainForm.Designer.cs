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
            this.seasonOverviewTreeView = new TraktTVUpdateClient.TreeViewEx();
            this.watchedListView = new TraktTVUpdateClient.ListViewEx();
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scoreColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.eventLabel = new System.Windows.Forms.Label();
            this.relogButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.showPosterBox)).BeginInit();
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
            this.showNameLabel.AutoSize = true;
            this.showNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.showNameLabel.Location = new System.Drawing.Point(118, 12);
            this.showNameLabel.Name = "showNameLabel";
            this.showNameLabel.Size = new System.Drawing.Size(180, 24);
            this.showNameLabel.TabIndex = 1;
            this.showNameLabel.Text = "                                  ";
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
            this.removeEpisodeButton.Click += new System.EventHandler(this.removeEpisodeButton_Click);
            // 
            // addEpisodeButton
            // 
            this.addEpisodeButton.Location = new System.Drawing.Point(282, 41);
            this.addEpisodeButton.Name = "addEpisodeButton";
            this.addEpisodeButton.Size = new System.Drawing.Size(20, 20);
            this.addEpisodeButton.TabIndex = 6;
            this.addEpisodeButton.Text = "+";
            this.addEpisodeButton.UseVisualStyleBackColor = true;
            this.addEpisodeButton.Click += new System.EventHandler(this.addEpisodeButton_Click);
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
            this.scoreComboBox.SelectedIndexChanged += new System.EventHandler(this.scoreComboBox_SelectedIndexChanged);
            // 
            // syncButton
            // 
            this.syncButton.BackgroundImage = global::TraktTVUpdateClient.Properties.Resources.Update;
            this.syncButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.syncButton.Location = new System.Drawing.Point(423, 3);
            this.syncButton.Name = "syncButton";
            this.syncButton.Size = new System.Drawing.Size(32, 32);
            this.syncButton.TabIndex = 13;
            this.syncButton.UseVisualStyleBackColor = true;
            this.syncButton.Click += new System.EventHandler(this.syncButton_Click);
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
            this.addShowButton.BackgroundImage = global::TraktTVUpdateClient.Properties.Resources.Search;
            this.addShowButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.addShowButton.Location = new System.Drawing.Point(461, 3);
            this.addShowButton.Name = "addShowButton";
            this.addShowButton.Size = new System.Drawing.Size(32, 32);
            this.addShowButton.TabIndex = 16;
            this.addShowButton.UseVisualStyleBackColor = true;
            this.addShowButton.Click += new System.EventHandler(this.addShowButton_Click);
            // 
            // settingButton
            // 
            this.settingButton.BackgroundImage = global::TraktTVUpdateClient.Properties.Resources.Settings;
            this.settingButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.settingButton.Location = new System.Drawing.Point(499, 3);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(32, 32);
            this.settingButton.TabIndex = 17;
            this.settingButton.UseVisualStyleBackColor = true;
            this.settingButton.Click += new System.EventHandler(this.settingButton_Click);
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
            // seasonOverviewTreeView
            // 
            this.seasonOverviewTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seasonOverviewTreeView.CheckBoxes = true;
            this.seasonOverviewTreeView.Location = new System.Drawing.Point(537, 3);
            this.seasonOverviewTreeView.Name = "seasonOverviewTreeView";
            this.seasonOverviewTreeView.Size = new System.Drawing.Size(145, 373);
            this.seasonOverviewTreeView.TabIndex = 19;
            this.seasonOverviewTreeView.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.seasonOverviewTreeView_BeforeCheck);
            this.seasonOverviewTreeView.DoubleClick += new System.EventHandler(this.seasonOverviewTreeView_DoubleClick);
            // 
            // watchedListView
            // 
            this.watchedListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.watchedListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.progressColumnHeader,
            this.scoreColumnHeader});
            this.watchedListView.FullRowSelect = true;
            this.watchedListView.Location = new System.Drawing.Point(12, 168);
            this.watchedListView.MultiSelect = false;
            this.watchedListView.Name = "watchedListView";
            this.watchedListView.Size = new System.Drawing.Size(519, 208);
            this.watchedListView.TabIndex = 10;
            this.watchedListView.UseCompatibleStateImageBehavior = false;
            this.watchedListView.View = System.Windows.Forms.View.Details;
            this.watchedListView.SelectedIndexChanged += new System.EventHandler(this.watchedListView_SelectedIndexChanged);
            this.watchedListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.watchedListView_MouseDoubleClick);
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
            // eventLabel
            // 
            this.eventLabel.AutoSize = true;
            this.eventLabel.Location = new System.Drawing.Point(308, 149);
            this.eventLabel.Name = "eventLabel";
            this.eventLabel.Size = new System.Drawing.Size(0, 13);
            this.eventLabel.TabIndex = 21;
            // 
            // relogButton
            // 
            this.relogButton.BackgroundImage = global::TraktTVUpdateClient.Properties.Resources.Lock;
            this.relogButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.relogButton.Location = new System.Drawing.Point(385, 3);
            this.relogButton.Name = "relogButton";
            this.relogButton.Size = new System.Drawing.Size(32, 32);
            this.relogButton.TabIndex = 22;
            this.relogButton.UseVisualStyleBackColor = true;
            this.relogButton.Click += new System.EventHandler(this.relogButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 388);
            this.Controls.Add(this.relogButton);
            this.Controls.Add(this.eventLabel);
            this.Controls.Add(this.nextUnwatchedEpisodeLbl);
            this.Controls.Add(this.seasonOverviewTreeView);
            this.Controls.Add(this.vlcConnectStatusLabel);
            this.Controls.Add(this.settingButton);
            this.Controls.Add(this.addShowButton);
            this.Controls.Add(this.traktConnectStatusLabel);
            this.Controls.Add(this.syncButton);
            this.Controls.Add(this.scoreComboBox);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.watchedListView);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListViewEx watchedListView;
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
        private System.Windows.Forms.Label eventLabel;
        private System.Windows.Forms.Button relogButton;
    }
}

