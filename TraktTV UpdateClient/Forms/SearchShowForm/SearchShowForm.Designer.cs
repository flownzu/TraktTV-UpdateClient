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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.add1stEpisodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAllEpisodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSpecificEpisodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSpecificSeasonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(241, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(259, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 41);
            this.button1.TabIndex = 1;
            this.button1.Text = "Search!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Search Limit: ";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(86, 34);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(44, 20);
            this.textBox2.TabIndex = 3;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Location = new System.Drawing.Point(12, 60);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(331, 190);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 240;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Year";
            this.columnHeader2.Width = 70;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add1stEpisodeToolStripMenuItem,
            this.addAllEpisodesToolStripMenuItem,
            this.addSpecificEpisodeToolStripMenuItem,
            this.addSpecificSeasonToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(184, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
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
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "SearchShowForm";
            this.Text = "Search";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem add1stEpisodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addAllEpisodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSpecificEpisodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSpecificSeasonToolStripMenuItem;
    }
}