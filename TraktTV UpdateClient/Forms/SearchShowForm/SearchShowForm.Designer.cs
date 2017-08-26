namespace TraktTVUpdateClient.Forms
{
    partial class SearchShowForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchShowForm));
            this.searchShowNameTxtBox = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.searchLimitLbl = new System.Windows.Forms.Label();
            this.searchLimitTxtBox = new System.Windows.Forms.TextBox();
            this.foundShowsListView = new System.Windows.Forms.ListView();
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.yearColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.addEpisodesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.add1stEpisodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAllEpisodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSpecificEpisodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSpecificSeasonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSummaryAndSeasonOverviewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.showSummaryRTxtBox = new System.Windows.Forms.RichTextBox();
            this.seasonOverviewTreeView = new TraktTVUpdateClient.TreeViewEx();
            this.addCompleteShowBtn = new System.Windows.Forms.Button();
            this.addSelectedEpisodes = new System.Windows.Forms.Button();
            this.addEpisodesContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.showSummaryAndSeasonOverviewSplitContainer)).BeginInit();
            this.showSummaryAndSeasonOverviewSplitContainer.Panel1.SuspendLayout();
            this.showSummaryAndSeasonOverviewSplitContainer.Panel2.SuspendLayout();
            this.showSummaryAndSeasonOverviewSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchShowNameTxtBox
            // 
            this.searchShowNameTxtBox.Location = new System.Drawing.Point(12, 12);
            this.searchShowNameTxtBox.Name = "searchShowNameTxtBox";
            this.searchShowNameTxtBox.Size = new System.Drawing.Size(241, 20);
            this.searchShowNameTxtBox.TabIndex = 0;
            this.searchShowNameTxtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchShowNameTxtBox_KeyDown);
            // 
            // searchBtn
            // 
            this.searchBtn.Location = new System.Drawing.Point(259, 13);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(84, 41);
            this.searchBtn.TabIndex = 1;
            this.searchBtn.Text = "Search!";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // searchLimitLbl
            // 
            this.searchLimitLbl.AutoSize = true;
            this.searchLimitLbl.Location = new System.Drawing.Point(9, 37);
            this.searchLimitLbl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.searchLimitLbl.Name = "searchLimitLbl";
            this.searchLimitLbl.Size = new System.Drawing.Size(71, 13);
            this.searchLimitLbl.TabIndex = 2;
            this.searchLimitLbl.Text = "Search Limit: ";
            // 
            // searchLimitTxtBox
            // 
            this.searchLimitTxtBox.Location = new System.Drawing.Point(86, 34);
            this.searchLimitTxtBox.Name = "searchLimitTxtBox";
            this.searchLimitTxtBox.Size = new System.Drawing.Size(44, 20);
            this.searchLimitTxtBox.TabIndex = 3;
            // 
            // foundShowsListView
            // 
            this.foundShowsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.foundShowsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.yearColumnHeader});
            this.foundShowsListView.ContextMenuStrip = this.addEpisodesContextMenu;
            this.foundShowsListView.HideSelection = false;
            this.foundShowsListView.Location = new System.Drawing.Point(12, 60);
            this.foundShowsListView.MultiSelect = false;
            this.foundShowsListView.Name = "foundShowsListView";
            this.foundShowsListView.ShowItemToolTips = true;
            this.foundShowsListView.Size = new System.Drawing.Size(331, 190);
            this.foundShowsListView.TabIndex = 4;
            this.foundShowsListView.UseCompatibleStateImageBehavior = false;
            this.foundShowsListView.View = System.Windows.Forms.View.Details;
            this.foundShowsListView.SelectedIndexChanged += new System.EventHandler(this.FoundShowsListView_SelectedIndexChanged);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 240;
            // 
            // yearColumnHeader
            // 
            this.yearColumnHeader.Text = "Year";
            this.yearColumnHeader.Width = 70;
            // 
            // addEpisodesContextMenu
            // 
            this.addEpisodesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add1stEpisodeToolStripMenuItem,
            this.addAllEpisodesToolStripMenuItem,
            this.addSpecificEpisodeToolStripMenuItem,
            this.addSpecificSeasonToolStripMenuItem});
            this.addEpisodesContextMenu.Name = "contextMenuStrip1";
            this.addEpisodesContextMenu.Size = new System.Drawing.Size(184, 92);
            this.addEpisodesContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.AddEpisodesContextMenu_Opening);
            // 
            // add1stEpisodeToolStripMenuItem
            // 
            this.add1stEpisodeToolStripMenuItem.Name = "add1stEpisodeToolStripMenuItem";
            this.add1stEpisodeToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.add1stEpisodeToolStripMenuItem.Text = "Add 1st episode";
            this.add1stEpisodeToolStripMenuItem.Click += new System.EventHandler(this.Add1stEpisodeToolStripMenuItem_Click);
            // 
            // addAllEpisodesToolStripMenuItem
            // 
            this.addAllEpisodesToolStripMenuItem.Name = "addAllEpisodesToolStripMenuItem";
            this.addAllEpisodesToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.addAllEpisodesToolStripMenuItem.Text = "Add all episodes";
            this.addAllEpisodesToolStripMenuItem.Click += new System.EventHandler(this.AddAllEpisodesToolStripMenuItem_Click);
            // 
            // addSpecificEpisodeToolStripMenuItem
            // 
            this.addSpecificEpisodeToolStripMenuItem.Name = "addSpecificEpisodeToolStripMenuItem";
            this.addSpecificEpisodeToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.addSpecificEpisodeToolStripMenuItem.Text = "Add specific episode";
            // 
            // addSpecificSeasonToolStripMenuItem
            // 
            this.addSpecificSeasonToolStripMenuItem.Name = "addSpecificSeasonToolStripMenuItem";
            this.addSpecificSeasonToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.addSpecificSeasonToolStripMenuItem.Text = "Add specific season";
            // 
            // showSummaryAndSeasonOverviewSplitContainer
            // 
            this.showSummaryAndSeasonOverviewSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showSummaryAndSeasonOverviewSplitContainer.Location = new System.Drawing.Point(349, 12);
            this.showSummaryAndSeasonOverviewSplitContainer.Name = "showSummaryAndSeasonOverviewSplitContainer";
            // 
            // showSummaryAndSeasonOverviewSplitContainer.Panel1
            // 
            this.showSummaryAndSeasonOverviewSplitContainer.Panel1.Controls.Add(this.showSummaryRTxtBox);
            // 
            // showSummaryAndSeasonOverviewSplitContainer.Panel2
            // 
            this.showSummaryAndSeasonOverviewSplitContainer.Panel2.Controls.Add(this.seasonOverviewTreeView);
            this.showSummaryAndSeasonOverviewSplitContainer.Size = new System.Drawing.Size(277, 204);
            this.showSummaryAndSeasonOverviewSplitContainer.SplitterDistance = 138;
            this.showSummaryAndSeasonOverviewSplitContainer.TabIndex = 5;
            // 
            // showSummaryRTxtBox
            // 
            this.showSummaryRTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showSummaryRTxtBox.Location = new System.Drawing.Point(0, 0);
            this.showSummaryRTxtBox.Name = "showSummaryRTxtBox";
            this.showSummaryRTxtBox.ReadOnly = true;
            this.showSummaryRTxtBox.Size = new System.Drawing.Size(138, 204);
            this.showSummaryRTxtBox.TabIndex = 0;
            this.showSummaryRTxtBox.Text = "";
            // 
            // seasonOverviewTreeView
            // 
            this.seasonOverviewTreeView.CheckBoxes = true;
            this.seasonOverviewTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seasonOverviewTreeView.Location = new System.Drawing.Point(0, 0);
            this.seasonOverviewTreeView.Name = "seasonOverviewTreeView";
            this.seasonOverviewTreeView.Size = new System.Drawing.Size(135, 204);
            this.seasonOverviewTreeView.TabIndex = 0;
            this.seasonOverviewTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.SeasonOverviewTreeView_AfterCheck);
            // 
            // addCompleteShowBtn
            // 
            this.addCompleteShowBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addCompleteShowBtn.Location = new System.Drawing.Point(349, 222);
            this.addCompleteShowBtn.Name = "addCompleteShowBtn";
            this.addCompleteShowBtn.Size = new System.Drawing.Size(138, 28);
            this.addCompleteShowBtn.TabIndex = 6;
            this.addCompleteShowBtn.Text = "Add complete show";
            this.addCompleteShowBtn.UseVisualStyleBackColor = true;
            this.addCompleteShowBtn.Click += new System.EventHandler(this.AddCompleteShowBtn_Click);
            // 
            // addSelectedEpisodes
            // 
            this.addSelectedEpisodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addSelectedEpisodes.Location = new System.Drawing.Point(491, 222);
            this.addSelectedEpisodes.Name = "addSelectedEpisodes";
            this.addSelectedEpisodes.Size = new System.Drawing.Size(135, 28);
            this.addSelectedEpisodes.TabIndex = 7;
            this.addSelectedEpisodes.Text = "Add selected Episodes";
            this.addSelectedEpisodes.UseVisualStyleBackColor = true;
            this.addSelectedEpisodes.Click += new System.EventHandler(this.AddSelectedEpisodes_Click);
            // 
            // SearchShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 262);
            this.Controls.Add(this.addSelectedEpisodes);
            this.Controls.Add(this.addCompleteShowBtn);
            this.Controls.Add(this.showSummaryAndSeasonOverviewSplitContainer);
            this.Controls.Add(this.foundShowsListView);
            this.Controls.Add(this.searchLimitTxtBox);
            this.Controls.Add(this.searchLimitLbl);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.searchShowNameTxtBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(654, 300);
            this.Name = "SearchShowForm";
            this.Text = "Search";
            this.addEpisodesContextMenu.ResumeLayout(false);
            this.showSummaryAndSeasonOverviewSplitContainer.Panel1.ResumeLayout(false);
            this.showSummaryAndSeasonOverviewSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.showSummaryAndSeasonOverviewSplitContainer)).EndInit();
            this.showSummaryAndSeasonOverviewSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchShowNameTxtBox;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.Label searchLimitLbl;
        private System.Windows.Forms.TextBox searchLimitTxtBox;
        private System.Windows.Forms.ListView foundShowsListView;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader yearColumnHeader;
        private System.Windows.Forms.ContextMenuStrip addEpisodesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem add1stEpisodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addAllEpisodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSpecificEpisodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSpecificSeasonToolStripMenuItem;
        private System.Windows.Forms.RichTextBox showSummaryRTxtBox;
        private TreeViewEx seasonOverviewTreeView;
        private System.Windows.Forms.SplitContainer showSummaryAndSeasonOverviewSplitContainer;
        private System.Windows.Forms.Button addCompleteShowBtn;
        private System.Windows.Forms.Button addSelectedEpisodes;
    }
}