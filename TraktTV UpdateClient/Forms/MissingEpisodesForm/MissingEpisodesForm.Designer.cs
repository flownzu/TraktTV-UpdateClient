namespace TraktTVUpdateClient.Forms
{
    partial class MissingEpisodeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingEpisodeForm));
            this.exportBtn = new System.Windows.Forms.Button();
            this.treeViewMissingEpisodes = new TraktTVUpdateClient.TreeViewEx();
            this.checkBoxSeasonExport = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // exportBtn
            // 
            this.exportBtn.Location = new System.Drawing.Point(212, 453);
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(209, 41);
            this.exportBtn.TabIndex = 2;
            this.exportBtn.Text = "CSV Export";
            this.exportBtn.UseVisualStyleBackColor = true;
            this.exportBtn.Click += new System.EventHandler(this.ExportBtn_Click);
            // 
            // treeViewMissingEpisodes
            // 
            this.treeViewMissingEpisodes.Location = new System.Drawing.Point(12, 12);
            this.treeViewMissingEpisodes.Name = "treeViewMissingEpisodes";
            this.treeViewMissingEpisodes.Size = new System.Drawing.Size(409, 435);
            this.treeViewMissingEpisodes.TabIndex = 3;
            // 
            // checkBoxSeasonExport
            // 
            this.checkBoxSeasonExport.AutoSize = true;
            this.checkBoxSeasonExport.Location = new System.Drawing.Point(12, 466);
            this.checkBoxSeasonExport.Name = "checkBoxSeasonExport";
            this.checkBoxSeasonExport.Size = new System.Drawing.Size(194, 17);
            this.checkBoxSeasonExport.TabIndex = 4;
            this.checkBoxSeasonExport.Text = "Export whole season as single entry";
            this.checkBoxSeasonExport.UseVisualStyleBackColor = true;
            // 
            // MissingEpisodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 504);
            this.Controls.Add(this.checkBoxSeasonExport);
            this.Controls.Add(this.treeViewMissingEpisodes);
            this.Controls.Add(this.exportBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MissingEpisodeForm";
            this.Text = "Missing Episodes in Collection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button exportBtn;
        private TreeViewEx treeViewMissingEpisodes;
        private System.Windows.Forms.CheckBox checkBoxSeasonExport;
    }
}