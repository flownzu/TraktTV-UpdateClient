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
            this.vlcPortLbl = new System.Windows.Forms.Label();
            this.vlcPortTxtBox = new System.Windows.Forms.TextBox();
            this.markEpisodeLbl = new System.Windows.Forms.Label();
            this.watchedPercentTrackBar = new System.Windows.Forms.TrackBar();
            this.watchedPercentLbl = new System.Windows.Forms.Label();
            this.saveSettingsBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.watchedPercentTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // vlcPortLbl
            // 
            this.vlcPortLbl.AutoSize = true;
            this.vlcPortLbl.Location = new System.Drawing.Point(12, 9);
            this.vlcPortLbl.Name = "vlcPortLbl";
            this.vlcPortLbl.Size = new System.Drawing.Size(55, 13);
            this.vlcPortLbl.TabIndex = 0;
            this.vlcPortLbl.Text = "VLC Port: ";
            // 
            // vlcPortTxtBox
            // 
            this.vlcPortTxtBox.Location = new System.Drawing.Point(73, 6);
            this.vlcPortTxtBox.Name = "vlcPortTxtBox";
            this.vlcPortTxtBox.Size = new System.Drawing.Size(65, 20);
            this.vlcPortTxtBox.TabIndex = 1;
            this.vlcPortTxtBox.Text = "2150";
            // 
            // markEpisodeLbl
            // 
            this.markEpisodeLbl.AutoSize = true;
            this.markEpisodeLbl.Location = new System.Drawing.Point(11, 38);
            this.markEpisodeLbl.Name = "markEpisodeLbl";
            this.markEpisodeLbl.Size = new System.Drawing.Size(127, 13);
            this.markEpisodeLbl.TabIndex = 2;
            this.markEpisodeLbl.Text = "Mark episode watched at";
            // 
            // watchedPercentTrackBar
            // 
            this.watchedPercentTrackBar.Location = new System.Drawing.Point(144, 12);
            this.watchedPercentTrackBar.Maximum = 100;
            this.watchedPercentTrackBar.Minimum = 50;
            this.watchedPercentTrackBar.Name = "watchedPercentTrackBar";
            this.watchedPercentTrackBar.Size = new System.Drawing.Size(127, 45);
            this.watchedPercentTrackBar.TabIndex = 3;
            this.watchedPercentTrackBar.Value = 50;
            this.watchedPercentTrackBar.ValueChanged += new System.EventHandler(this.watchedPercentTrackBar_ValueChanged);
            // 
            // watchedPercentLbl
            // 
            this.watchedPercentLbl.AutoSize = true;
            this.watchedPercentLbl.Location = new System.Drawing.Point(274, 38);
            this.watchedPercentLbl.Name = "watchedPercentLbl";
            this.watchedPercentLbl.Size = new System.Drawing.Size(76, 13);
            this.watchedPercentLbl.TabIndex = 4;
            this.watchedPercentLbl.Text = "90% of the title";
            // 
            // saveSettingsBtn
            // 
            this.saveSettingsBtn.Location = new System.Drawing.Point(12, 59);
            this.saveSettingsBtn.Name = "saveSettingsBtn";
            this.saveSettingsBtn.Size = new System.Drawing.Size(75, 23);
            this.saveSettingsBtn.TabIndex = 5;
            this.saveSettingsBtn.Text = "Save";
            this.saveSettingsBtn.UseVisualStyleBackColor = true;
            this.saveSettingsBtn.Click += new System.EventHandler(this.saveSettingsBtn_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 94);
            this.Controls.Add(this.saveSettingsBtn);
            this.Controls.Add(this.watchedPercentLbl);
            this.Controls.Add(this.watchedPercentTrackBar);
            this.Controls.Add(this.markEpisodeLbl);
            this.Controls.Add(this.vlcPortTxtBox);
            this.Controls.Add(this.vlcPortLbl);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.watchedPercentTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label vlcPortLbl;
        private System.Windows.Forms.TextBox vlcPortTxtBox;
        private System.Windows.Forms.Label markEpisodeLbl;
        private System.Windows.Forms.TrackBar watchedPercentTrackBar;
        private System.Windows.Forms.Label watchedPercentLbl;
        private System.Windows.Forms.Button saveSettingsBtn;
    }
}