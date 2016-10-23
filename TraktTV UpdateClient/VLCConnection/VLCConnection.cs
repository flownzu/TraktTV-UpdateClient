using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TraktTVUpdateClient
{
    public class VLCConnection
    {
        public int port { get; set; }
        public TcpClient client { get; set; }        
        public StreamWriter writeStream { get; set; }

        public EventHandler ConnectionLost;
        public EventHandler MediaItemChanged;
        public EventHandler PlaybackPaused;
        public EventHandler PlaybackResumed;
        public EventHandler PlaybackStopped;
        public EventHandler WatchedPercentReached;

        private string CurrentMediaItem = String.Empty;
        private string CurrentMediaTitle = String.Empty;
        private string CurrentMediaState = String.Empty;
        private uint CurrentMediaLength = 0;
        private bool CurrentMediaWatchedPercentReached = false;
        
        public VLCConnection(int _port)
        {
            client = new TcpClient();
            port = _port;

            client.Connect("localhost", port);
            writeStream = new StreamWriter(client.GetStream());
            read();
        }

        protected virtual void OnConnectionLost()
        {
            CurrentMediaItem = String.Empty;
            CurrentMediaTitle = String.Empty;
            CurrentMediaState = String.Empty;
            CurrentMediaLength = 0;
            CurrentMediaWatchedPercentReached = false;
            ConnectionLost?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMediaItemChanged()
        {
            CurrentMediaItem = String.Empty;
            CurrentMediaTitle = String.Empty;
            CurrentMediaState = String.Empty;
            CurrentMediaLength = 0;
            CurrentMediaWatchedPercentReached = false;
            MediaItemChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPlaybackPaused()
        {
            PlaybackPaused?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPlaybackResumed()
        {
            PlaybackResumed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPlaybackStopped()
        {
            PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnWatchedPercentReached()
        {
            CurrentMediaWatchedPercentReached = true;
            WatchedPercentReached?.Invoke(this, EventArgs.Empty);
        }

        public async Task write(String msg)
        {
            await writeStream.WriteLineAsync(msg);
            await writeStream.FlushAsync();
        }

        public async Task<string> read()
        {
            if (client.Connected)
            {
                NetworkStream stream = client.GetStream();
                {
                    byte[] data = new byte[1024];
                    StringBuilder completeMessage = new StringBuilder();
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = await stream.ReadAsync(data, 0, data.Length);
                        completeMessage.AppendFormat("{0}", Encoding.UTF8.GetString(data, 0, data.Length));
                    } while (stream.DataAvailable);
                    return Regex.Replace(completeMessage.ToString(), @"([\S\s]*)> ([\S\s]*)", "$1");
                }
            }
            return String.Empty;
        }

        public async void ConnectionThread()
        {
            while (client.Connected)
            {
                try
                {
                    await write("status");
                    string statusResponse = await read();
                    await write("get_time");
                    string getTimeResponse = await read();
                    if (CurrentMediaLength == 0)
                    {
                        await write("get_length");
                        string getLengthResponse = await read();
                        if (!String.IsNullOrEmpty(getLengthResponse.Trim()) && !String.IsNullOrWhiteSpace(getLengthResponse.Trim())) { UInt32.TryParse(getLengthResponse.Trim(), out CurrentMediaLength); }
                    }

                    Match m = Regex.Match(statusResponse, @"new input\: (.*) \)(?:[\S\s].*){3}state (\w+)");
                    if (m.Success)
                    {
                        string MediaItem = m.Groups[1].Value;
                        if (!CurrentMediaItem.Equals(MediaItem))
                        {
                            Debug.WriteLine("Current Media: " + CurrentMediaItem + Environment.NewLine + "New Media: " + MediaItem);
                            OnMediaItemChanged();
                            CurrentMediaItem = MediaItem;
                            await write("get_title");
                            string getTitleResponse = await read();
                            if (!String.IsNullOrEmpty(getTitleResponse)) { CurrentMediaTitle = getTitleResponse; }
                        }
                        string MediaState = m.Groups[2].Value;
                        if (CurrentMediaState.Equals(String.Empty)) { CurrentMediaState = MediaState; }
                        else
                        {
                            if (!CurrentMediaState.Equals(MediaState))
                            {
                                CurrentMediaState = MediaState;
                                if (CurrentMediaState.Equals("playing"))
                                {
                                    OnPlaybackResumed();
                                }
                                else if (CurrentMediaState.Equals("paused"))
                                {
                                    OnPlaybackPaused();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (statusResponse.Contains("state stopped"))
                        {
                            CurrentMediaState = "stopped";
                            OnPlaybackStopped();
                        }
                    }

                    if(!String.IsNullOrWhiteSpace(getTimeResponse) && CurrentMediaLength > 0)
                    {
                        uint currentTime = 0;
                        UInt32.TryParse(getTimeResponse.Trim(), out currentTime);
                        float watchedPercent = (float)currentTime / (float)CurrentMediaLength;
                        if(!CurrentMediaWatchedPercentReached && watchedPercent >= Properties.Settings.Default.WatchedPercent)
                        {
                            OnWatchedPercentReached();
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch (IOException ex) { break; }
            }
            OnConnectionLost();
        }
    }
}
