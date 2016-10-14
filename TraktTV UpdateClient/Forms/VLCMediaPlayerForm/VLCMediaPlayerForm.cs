using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktTVUpdateClient.Extension;
using Vlc.DotNet.Core;

namespace TraktTVUpdateClient.Forms.VLCMediaPlayerForm
{
    public partial class VLCMediaPlayerForm : Form
    {
        public VLCMediaPlayerForm()
        {
            InitializeComponent();
        }

        private void vlcControl_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null) return;
            var installDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\VideoLAN\VLC", "InstallDir", null);
            if (installDir != null)
            {
                e.VlcLibDirectory = new DirectoryInfo((string)installDir);
                return;
            }
        }

        private void vlcControl_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void vlcControl_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {

        }

        private void vlcControl_MediaChanged(object sender, VlcMediaPlayerMediaChangedEventArgs e)
        {

        }

        private void vlcControl_Paused(object sender, VlcMediaPlayerPausedEventArgs e)
        {

        }

        private void vlcControl_Stopped(object sender, VlcMediaPlayerStoppedEventArgs e)
        {

        }

        private void vlcControl_EncounteredError(object sender, VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            MessageBox.Show("The VLC control encountered an error...the player will now shutdown.");
            this.Close();
        }

        private void vlcControl_PositionChanged(object sender, VlcMediaPlayerPositionChangedEventArgs e)
        {

        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (vlcControl.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.NothingSpecial)
            {
                vlcControl.Play(new FileInfo(@"L:\VLC Remote Test\Breaking Bad\TestDatei S05E04.flv"));
                vlcControl.Audio.Volume = volumeProgressBar.Value;
            }
            else if (vlcControl.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Playing)
            {
                vlcControl.Pause();
            }
            else if (vlcControl.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Paused)
            {
                vlcControl.Play();
            }
            else if (vlcControl.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Stopped || vlcControl.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Ended)
            {
                vlcControl.Play(vlcControl.GetCurrentMedia().Mrl);
            }
        }

        private void volumeProgressBar_MouseClick(object sender, MouseEventArgs e)
        {
            volumeProgressBar.Value = e.X*2;
            vlcControl.Audio.Volume = volumeProgressBar.Value;
        }

        private void vlcControl_Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            currentlyPlayingLabel.InvokeIfRequired(l => l.Text = "Currently playing: " + vlcControl.GetCurrentMedia().Title);
        }

        private void vlcControl_LengthChanged(object sender, VlcMediaPlayerLengthChangedEventArgs e)
        {
            maximumPlaytimeLabel.InvokeIfRequired(l => l.Text = new DateTime(new TimeSpan((long)e.NewLength).Ticks).ToString("T"));
            currentPlayTimeSlider.InvokeIfRequired(s => s.Maximum = (int)(e.NewLength/10000));
        }

        private void vlcControl_TimeChanged(object sender, VlcMediaPlayerTimeChangedEventArgs e)
        {
            currentPlaytimeLabel.InvokeIfRequired(l => l.Text = new DateTime(new TimeSpan((long)e.NewTime).Ticks * 10000).ToString("T"));
            currentPlayTimeSlider.InvokeIfRequired(s => s.Value = (int)e.NewTime);
        }

        private void currentPlayTimeSlider_MouseClick(object sender, MouseEventArgs e)
        {
            float percent = ((float)currentPlayTimeSlider.KnobX / (float)currentPlayTimeSlider.Width);
            if(vlcControl.State != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Error && vlcControl.State != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Buffering)
            {
                if (vlcControl.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Stopped || vlcControl.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Ended)
                {
                    var mediaObject = vlcControl.GetCurrentMedia().Mrl;
                    vlcControl.Play(mediaObject);
                    vlcControl.Position = percent;
                }
                else
                {
                    if (vlcControl.Length > 0)
                    {
                        vlcControl.Position = percent;
                    }
                }
            }
        }

        private void VLCMediaPlayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeVLC();
            this.Dispose();
        }

        private void currentPlayTimeSlider_ValueChanged(object sender, EventArgs e)
        {
            if ((sender as Slider)._draggingKnob)
            {
                float percent = ((float)currentPlayTimeSlider.KnobX / (float)currentPlayTimeSlider.Width);
                if (vlcControl.State != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Error && vlcControl.State != Vlc.DotNet.Core.Interops.Signatures.MediaStates.Buffering)
                {
                    if (vlcControl.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Stopped || vlcControl.State == Vlc.DotNet.Core.Interops.Signatures.MediaStates.Ended)
                    {
                        vlcControl.Play(vlcControl.GetCurrentMedia().Mrl);
                        vlcControl.Position = percent;
                    }
                    else
                    {
                        if (vlcControl.Length > 0)
                        {
                            vlcControl.Position = percent;
                        }
                    }
                }
            }
        }

        private void DisposeVLC()
        {
            vlcControl.Position = 1f;
            this.Controls.Remove(vlcControl);
            GC.SuppressFinalize(vlcControl);
            vlcControl = null;
        }

        private void currentlyPlayingLabel_DoubleClick(object sender, EventArgs e)
        {
            vlcControl.GoFullScreen();
        }
    }
}
