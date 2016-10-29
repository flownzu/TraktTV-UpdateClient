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
            this.addEpisodesContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.searchShowNameTxtBox.Location = new System.Drawing.Point(12, 12);
            this.searchShowNameTxtBox.Name = "textBox1";
            this.searchShowNameTxtBox.Size = new System.Drawing.Size(241, 20);
            this.searchShowNameTxtBox.TabIndex = 0;
            // 
            // button1
            // 
            this.searchBtn.Location = new System.Drawing.Point(259, 13);
            this.searchBtn.Name = "button1";
            this.searchBtn.Size = new System.Drawing.Size(84, 41);
            this.searchBtn.TabIndex = 1;
            this.searchBtn.Text = "Search!";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // label1
            // 
            this.searchLimitLbl.AutoSize = true;
            this.searchLimitLbl.Location = new System.Drawing.Point(9, 37);
            this.searchLimitLbl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.searchLimitLbl.Name = "label1";
            this.searchLimitLbl.Size = new System.Drawing.Size(71, 13);
            this.searchLimitLbl.TabIndex = 2;
            this.searchLimitLbl.Text = "Search Limit: ";
            // 
            // textBox2
            // 
            this.searchLimitTxtBox.Location = new System.Drawing.Point(86, 34);
            this.searchLimitTxtBox.Name = "textBox2";
            this.searchLimitTxtBox.Size = new System.Drawing.Size(44, 20);
            this.searchLimitTxtBox.TabIndex = 3;
            // 
            // listView1
            // 
            this.foundShowsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.yearColumnHeader});
            this.foundShowsListView.ContextMenuStrip = this.addEpisodesContextMenu;
            this.foundShowsListView.Location = new System.Drawing.Point(12, 60);
            this.foundShowsListView.MultiSelect = false;
            this.foundShowsListView.Name = "listView1";
            this.foundShowsListView.ShowItemToolTips = true;
            this.foundShowsListView.Size = new System.Drawing.Size(331, 190);
            this.foundShowsListView.TabIndex = 4;
            this.foundShowsListView.UseCompatibleStateImageBehavior = false;
            this.foundShowsListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 240;
            // 
            // columnHeader2
            // 
            this.yearColumnHeader.Text = "Year";
            this.yearColumnHeader.Width = 70;
            // 
            // contextMenuStrip1
            // 
            this.addEpisodesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add1stEpisodeToolStripMenuItem,
            this.addAllEpisodesToolStripMenuItem,
            this.addSpecificEpisodeToolStripMenuItem,
            this.addSpecificSeasonToolStripMenuItem});
            this.addEpisodesContextMenu.Name = "contextMenuStrip1";
            this.addEpisodesContextMenu.Size = new System.Drawing.Size(184, 92);
            this.addEpisodesContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.addEpisodesContextMenu_Opening);
            // 
            // add1stEpisodeToolStripMenuItem
            // 
            this.add1stEpisodeToolStripMenuItem.Name = "add1stEpisodeToolStripMenuItem";
            this.add1stEpisodeToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.add1stEpisodeToolStripMenuItem.Text = "Add 1st episode";
            this.add1stEpisodeToolStripMenuItem.Click += new System.EventHandler(this.add1stEpisodeToolStripMenuItem_Click);
            // 
            // addAllEpisodesToolStripMenuItem
            // 
            this.addAllEpisodesToolStripMenuItem.Name = "addAllEpisodesToolStripMenuItem";
            this.addAllEpisodesToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.addAllEpisodesToolStripMenuItem.Text = "Add all episodes";
            this.addAllEpisodesToolStripMenuItem.Click += new System.EventHandler(this.addAllEpisodesToolStripMenuItem_Click);
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
            // SearchShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 262);
            this.Controls.Add(this.foundShowsListView);
            this.Controls.Add(this.searchLimitTxtBox);
            this.Controls.Add(this.searchLimitLbl);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.searchShowNameTxtBox);
            this.Name = "SearchShowForm";
            this.Text = "Search";
            this.addEpisodesContextMenu.ResumeLayout(false);
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
    }
}