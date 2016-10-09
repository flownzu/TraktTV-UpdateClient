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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.showNameLabel = new System.Windows.Forms.Label();
            this.episodeLabel = new System.Windows.Forms.Label();
            this.currentEpisodeTextBox = new System.Windows.Forms.TextBox();
            this.episodeCountLabel = new System.Windows.Forms.Label();
            this.removeEpisodeButton = new System.Windows.Forms.Button();
            this.addEpisodeButton = new System.Windows.Forms.Button();
            this.episodeProgressBar = new System.Windows.Forms.ProgressBar();
            this.yearLabel = new System.Windows.Forms.Label();
            this.genreLabel = new System.Windows.Forms.Label();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.scoreComboBox = new System.Windows.Forms.ComboBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.connectStatusLabel = new System.Windows.Forms.Label();
            this.addShowButton = new System.Windows.Forms.Button();
            this.watchedListView = new TraktTVUpdateClient.ListViewEx();
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scoreColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 150);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
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
            // episodeProgressBar
            // 
            this.episodeProgressBar.Location = new System.Drawing.Point(308, 41);
            this.episodeProgressBar.Name = "episodeProgressBar";
            this.episodeProgressBar.Size = new System.Drawing.Size(223, 23);
            this.episodeProgressBar.TabIndex = 7;
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
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(463, 140);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(68, 22);
            this.updateButton.TabIndex = 13;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // connectStatusLabel
            // 
            this.connectStatusLabel.AutoSize = true;
            this.connectStatusLabel.Location = new System.Drawing.Point(119, 149);
            this.connectStatusLabel.Name = "connectStatusLabel";
            this.connectStatusLabel.Size = new System.Drawing.Size(151, 13);
            this.connectStatusLabel.TabIndex = 15;
            this.connectStatusLabel.Text = "trakt.tv Status:  not connected";
            // 
            // addShowButton
            // 
            this.addShowButton.Location = new System.Drawing.Point(389, 140);
            this.addShowButton.Name = "addShowButton";
            this.addShowButton.Size = new System.Drawing.Size(68, 22);
            this.addShowButton.TabIndex = 16;
            this.addShowButton.Text = "Add Show";
            this.addShowButton.UseVisualStyleBackColor = true;
            this.addShowButton.Click += new System.EventHandler(this.addShowButton_Click);
            // 
            // watchedListView
            // 
            this.watchedListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.progressColumnHeader,
            this.scoreColumnHeader});
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 388);
            this.Controls.Add(this.addShowButton);
            this.Controls.Add(this.connectStatusLabel);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.scoreComboBox);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.watchedListView);
            this.Controls.Add(this.genreLabel);
            this.Controls.Add(this.yearLabel);
            this.Controls.Add(this.episodeProgressBar);
            this.Controls.Add(this.addEpisodeButton);
            this.Controls.Add(this.removeEpisodeButton);
            this.Controls.Add(this.episodeCountLabel);
            this.Controls.Add(this.currentEpisodeTextBox);
            this.Controls.Add(this.episodeLabel);
            this.Controls.Add(this.showNameLabel);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainForm";
            this.Text = "trakt.tv Updater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListViewEx watchedListView;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label showNameLabel;
        private System.Windows.Forms.Label episodeLabel;
        private System.Windows.Forms.TextBox currentEpisodeTextBox;
        private System.Windows.Forms.Label episodeCountLabel;
        private System.Windows.Forms.Button removeEpisodeButton;
        private System.Windows.Forms.Button addEpisodeButton;
        private System.Windows.Forms.ProgressBar episodeProgressBar;
        private System.Windows.Forms.Label yearLabel;
        private System.Windows.Forms.Label genreLabel;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.ComboBox scoreComboBox;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader progressColumnHeader;
        private System.Windows.Forms.ColumnHeader scoreColumnHeader;
        private System.Windows.Forms.Label connectStatusLabel;
        private System.Windows.Forms.Button addShowButton;
    }
}

