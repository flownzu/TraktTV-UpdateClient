namespace TraktTVUpdateClient.Forms.VLCMediaPlayerForm
{
    partial class VLCMediaPlayerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VLCMediaPlayerForm));
            this.vlcControl = new Vlc.DotNet.Forms.VlcControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mediaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.slowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audiospurToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deactivateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.louderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quiterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videotrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deactivateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subtitlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subtitleTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deactivateToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.currentPlaytimeLabel = new System.Windows.Forms.Label();
            this.maximumPlaytimeLabel = new System.Windows.Forms.Label();
            this.playButton = new System.Windows.Forms.Button();
            this.volumeProgressBar = new System.Windows.Forms.ProgressBar();
            this.volumeLabel = new System.Windows.Forms.Label();
            this.currentlyPlayingLabel = new System.Windows.Forms.Label();
            this.currentPlayTimeSlider = new TraktTVUpdateClient.Slider();
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).BeginInit();
            this.SuspendLayout();
            // 
            // vlcControl
            // 
            this.vlcControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vlcControl.BackColor = System.Drawing.Color.Black;
            this.vlcControl.Location = new System.Drawing.Point(0, 27);
            this.vlcControl.Name = "vlcControl";
            this.vlcControl.Size = new System.Drawing.Size(640, 480);
            this.vlcControl.Spu = -1;
            this.vlcControl.TabIndex = 0;
            this.vlcControl.Text = "vlcControl";
            this.vlcControl.VlcLibDirectory = null;
            this.vlcControl.VlcMediaplayerOptions = null;
            this.vlcControl.VlcLibDirectoryNeeded += new System.EventHandler<Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs>(this.vlcControl_VlcLibDirectoryNeeded);
            this.vlcControl.EncounteredError += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerEncounteredErrorEventArgs>(this.vlcControl_EncounteredError);
            this.vlcControl.EndReached += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs>(this.vlcControl_EndReached);
            this.vlcControl.LengthChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs>(this.vlcControl_LengthChanged);
            this.vlcControl.MediaChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerMediaChangedEventArgs>(this.vlcControl_MediaChanged);
            this.vlcControl.Paused += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs>(this.vlcControl_Paused);
            this.vlcControl.Playing += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs>(this.vlcControl_Playing);
            this.vlcControl.PositionChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs>(this.vlcControl_PositionChanged);
            this.vlcControl.TimeChanged += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs>(this.vlcControl_TimeChanged);
            this.vlcControl.Stopped += new System.EventHandler<Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs>(this.vlcControl_Stopped);
            this.vlcControl.DoubleClick += new System.EventHandler(this.vlcControl_DoubleClick);
            this.vlcControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.vlcControl_KeyPress);
            this.vlcControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.vlcControl_MouseDoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(640, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mediaToolStripMenuItem
            // 
            this.mediaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.mediaToolStripMenuItem.Name = "mediaToolStripMenuItem";
            this.mediaToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.mediaToolStripMenuItem.Text = "Media";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openFileToolStripMenuItem.Text = "Open File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // playbackToolStripMenuItem
            // 
            this.playbackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.speedToolStripMenuItem});
            this.playbackToolStripMenuItem.Name = "playbackToolStripMenuItem";
            this.playbackToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.playbackToolStripMenuItem.Text = "Playback";
            // 
            // speedToolStripMenuItem
            // 
            this.speedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fasterToolStripMenuItem,
            this.toolStripMenuItem2,
            this.slowerToolStripMenuItem});
            this.speedToolStripMenuItem.Name = "speedToolStripMenuItem";
            this.speedToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.speedToolStripMenuItem.Text = "Speed";
            // 
            // fasterToolStripMenuItem
            // 
            this.fasterToolStripMenuItem.Name = "fasterToolStripMenuItem";
            this.fasterToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.fasterToolStripMenuItem.Text = "Faster";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem2.Text = "Normal";
            // 
            // slowerToolStripMenuItem
            // 
            this.slowerToolStripMenuItem.Name = "slowerToolStripMenuItem";
            this.slowerToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.slowerToolStripMenuItem.Text = "Slower";
            // 
            // audioToolStripMenuItem
            // 
            this.audioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audiospurToolStripMenuItem,
            this.volumeToolStripMenuItem});
            this.audioToolStripMenuItem.Name = "audioToolStripMenuItem";
            this.audioToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.audioToolStripMenuItem.Text = "Audio";
            // 
            // audiospurToolStripMenuItem
            // 
            this.audiospurToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deactivateToolStripMenuItem});
            this.audiospurToolStripMenuItem.Name = "audiospurToolStripMenuItem";
            this.audiospurToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.audiospurToolStripMenuItem.Text = "Audiospur";
            // 
            // deactivateToolStripMenuItem
            // 
            this.deactivateToolStripMenuItem.Name = "deactivateToolStripMenuItem";
            this.deactivateToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.deactivateToolStripMenuItem.Text = "Deactivate";
            // 
            // volumeToolStripMenuItem
            // 
            this.volumeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.louderToolStripMenuItem,
            this.quiterToolStripMenuItem,
            this.oFFToolStripMenuItem});
            this.volumeToolStripMenuItem.Name = "volumeToolStripMenuItem";
            this.volumeToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.volumeToolStripMenuItem.Text = "Volume";
            // 
            // louderToolStripMenuItem
            // 
            this.louderToolStripMenuItem.Name = "louderToolStripMenuItem";
            this.louderToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.louderToolStripMenuItem.Text = "Louder";
            // 
            // quiterToolStripMenuItem
            // 
            this.quiterToolStripMenuItem.Name = "quiterToolStripMenuItem";
            this.quiterToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.quiterToolStripMenuItem.Text = "Quiter";
            // 
            // oFFToolStripMenuItem
            // 
            this.oFFToolStripMenuItem.Name = "oFFToolStripMenuItem";
            this.oFFToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.oFFToolStripMenuItem.Text = "OFF";
            // 
            // videoToolStripMenuItem
            // 
            this.videoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.videotrackToolStripMenuItem,
            this.fullScreenToolStripMenuItem});
            this.videoToolStripMenuItem.Name = "videoToolStripMenuItem";
            this.videoToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.videoToolStripMenuItem.Text = "Video";
            // 
            // videotrackToolStripMenuItem
            // 
            this.videotrackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deactivateToolStripMenuItem1});
            this.videotrackToolStripMenuItem.Name = "videotrackToolStripMenuItem";
            this.videotrackToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.videotrackToolStripMenuItem.Text = "Videotrack";
            // 
            // deactivateToolStripMenuItem1
            // 
            this.deactivateToolStripMenuItem1.Name = "deactivateToolStripMenuItem1";
            this.deactivateToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
            this.deactivateToolStripMenuItem1.Text = "Deactivate";
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.fullScreenToolStripMenuItem.Text = "Full Screen";
            // 
            // subtitlesToolStripMenuItem
            // 
            this.subtitlesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subtitleTrackToolStripMenuItem});
            this.subtitlesToolStripMenuItem.Name = "subtitlesToolStripMenuItem";
            this.subtitlesToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.subtitlesToolStripMenuItem.Text = "Subtitles";
            // 
            // subtitleTrackToolStripMenuItem
            // 
            this.subtitleTrackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deactivateToolStripMenuItem2});
            this.subtitleTrackToolStripMenuItem.Name = "subtitleTrackToolStripMenuItem";
            this.subtitleTrackToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.subtitleTrackToolStripMenuItem.Text = "Subtitle Track";
            // 
            // deactivateToolStripMenuItem2
            // 
            this.deactivateToolStripMenuItem2.Name = "deactivateToolStripMenuItem2";
            this.deactivateToolStripMenuItem2.Size = new System.Drawing.Size(129, 22);
            this.deactivateToolStripMenuItem2.Text = "Deactivate";
            // 
            // currentPlaytimeLabel
            // 
            this.currentPlaytimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currentPlaytimeLabel.AutoSize = true;
            this.currentPlaytimeLabel.Location = new System.Drawing.Point(6, 514);
            this.currentPlaytimeLabel.Name = "currentPlaytimeLabel";
            this.currentPlaytimeLabel.Size = new System.Drawing.Size(49, 13);
            this.currentPlaytimeLabel.TabIndex = 2;
            this.currentPlaytimeLabel.Text = "00:00:00";
            // 
            // maximumPlaytimeLabel
            // 
            this.maximumPlaytimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.maximumPlaytimeLabel.AutoSize = true;
            this.maximumPlaytimeLabel.Location = new System.Drawing.Point(584, 514);
            this.maximumPlaytimeLabel.Name = "maximumPlaytimeLabel";
            this.maximumPlaytimeLabel.Size = new System.Drawing.Size(49, 13);
            this.maximumPlaytimeLabel.TabIndex = 4;
            this.maximumPlaytimeLabel.Text = "00:00:00";
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playButton.Location = new System.Drawing.Point(12, 554);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(43, 23);
            this.playButton.TabIndex = 5;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // volumeProgressBar
            // 
            this.volumeProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeProgressBar.Location = new System.Drawing.Point(528, 554);
            this.volumeProgressBar.Maximum = 200;
            this.volumeProgressBar.Name = "volumeProgressBar";
            this.volumeProgressBar.Size = new System.Drawing.Size(100, 23);
            this.volumeProgressBar.TabIndex = 7;
            this.volumeProgressBar.Value = 100;
            this.volumeProgressBar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.volumeProgressBar_MouseClick);
            // 
            // volumeLabel
            // 
            this.volumeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.Location = new System.Drawing.Point(477, 558);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(45, 13);
            this.volumeLabel.TabIndex = 8;
            this.volumeLabel.Text = "Volume:";
            // 
            // currentlyPlayingLabel
            // 
            this.currentlyPlayingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currentlyPlayingLabel.AutoSize = true;
            this.currentlyPlayingLabel.Location = new System.Drawing.Point(61, 558);
            this.currentlyPlayingLabel.Name = "currentlyPlayingLabel";
            this.currentlyPlayingLabel.Size = new System.Drawing.Size(239, 13);
            this.currentlyPlayingLabel.TabIndex = 9;
            this.currentlyPlayingLabel.Text = "Currently Playing:  [Test dafsdfasjkdfa titltejaksdjf]";
            // 
            // currentPlayTimeSlider
            // 
            this.currentPlayTimeSlider.AllowQuickTracking = true;
            this.currentPlayTimeSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentPlayTimeSlider.KnobImage = ((System.Drawing.Bitmap)(resources.GetObject("currentPlayTimeSlider.KnobImage")));
            this.currentPlayTimeSlider.Location = new System.Drawing.Point(61, 513);
            this.currentPlayTimeSlider.Name = "currentPlayTimeSlider";
            this.currentPlayTimeSlider.Size = new System.Drawing.Size(517, 17);
            this.currentPlayTimeSlider.SmartLockAmount = 1;
            this.currentPlayTimeSlider.SmartLockingFlags = ((TraktTVUpdateClient.SmartLocking)((TraktTVUpdateClient.SmartLocking.Left | TraktTVUpdateClient.SmartLocking.Middle)));
            this.currentPlayTimeSlider.TabIndex = 6;
            this.currentPlayTimeSlider.MouseClick += new System.Windows.Forms.MouseEventHandler(this.currentPlayTimeSlider_MouseClick);
            // 
            // VLCMediaPlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 589);
            this.Controls.Add(this.currentlyPlayingLabel);
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.volumeProgressBar);
            this.Controls.Add(this.currentPlayTimeSlider);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.maximumPlaytimeLabel);
            this.Controls.Add(this.currentPlaytimeLabel);
            this.Controls.Add(this.vlcControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(656, 627);
            this.Name = "VLCMediaPlayerForm";
            this.Text = "Media Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VLCMediaPlayerForm_FormClosing);
            this.SizeChanged += new System.EventHandler(this.VLCMediaPlayerForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.vlcControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Vlc.DotNet.Forms.VlcControl vlcControl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mediaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem slowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audiospurToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deactivateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem volumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem louderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quiterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oFFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videotrackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deactivateToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subtitlesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subtitleTrackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deactivateToolStripMenuItem2;
        private System.Windows.Forms.Label currentPlaytimeLabel;
        private System.Windows.Forms.Label maximumPlaytimeLabel;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.ProgressBar volumeProgressBar;
        private System.Windows.Forms.Label volumeLabel;
        private System.Windows.Forms.Label currentlyPlayingLabel;
        private Slider currentPlayTimeSlider;
    }
}