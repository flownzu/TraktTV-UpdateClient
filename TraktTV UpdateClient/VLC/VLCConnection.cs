using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TraktTVUpdateClient.VLC
{
    public class VLCConnection
    {
        public VLCMediaItem CurrentMediaItem = new VLCMediaItem();
        public bool Connected { get { return Client.Connected; } }

        public EventHandler ConnectionLost;
        public EventHandler<MediaItemChangedEventArgs> MediaItemChanged;
        public EventHandler PlaybackPaused;
        public EventHandler PlaybackResumed;
        public EventHandler PlaybackStopped;
        public EventHandler WatchedPercentReached;

        private int Port { get; set; }
        private StreamWriter WriteStream { get; set; }
        private TcpClient Client { get; set; }

        public VLCConnection(int _port)
        {
            Client = new TcpClient();
            Port = _port;

            Client.Connect("localhost", Port);
            WriteStream = new StreamWriter(Client.GetStream());
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Read();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        protected virtual void OnConnectionLost()
        {
            CurrentMediaItem = null;
            ConnectionLost?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnMediaItemChanged(string mediaItem)
        {
            CurrentMediaItem.Path = mediaItem;
            CurrentMediaItem.State = String.Empty;
            CurrentMediaItem.WatchedPercentReached = false;
            MediaItemChanged?.Invoke(this, new MediaItemChangedEventArgs(CurrentMediaItem));
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
            CurrentMediaItem.WatchedPercentReached = true;
            WatchedPercentReached?.Invoke(this, EventArgs.Empty);
        }

        public async Task Write(String msg)
        {
            await WriteStream.WriteLineAsync(msg);
            await WriteStream.FlushAsync();
        }

        public async Task<string> Read()
        {
            if (Client.Connected)
            {
                NetworkStream stream = Client.GetStream();
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

        private async Task UpdateCurrentMediaLength()
        {
            await Write("get_length");
            string getLengthResponse = await Read();
            if (!String.IsNullOrEmpty(getLengthResponse.Trim()) && !String.IsNullOrWhiteSpace(getLengthResponse.Trim())) { UInt32.TryParse(getLengthResponse.Trim(), out CurrentMediaItem.Length); }
        }

        private async Task UpdateCurrentMediaTitle()
        {
            await Write("get_title");
            string getTitleResponse = await Read();
            if (!String.IsNullOrEmpty(getTitleResponse.Trim())) { CurrentMediaItem.Title = getTitleResponse.Trim(); }
        }

        public async void ConnectionThread()
        {
            while (Client.Connected)
            {
                try
                {
                    await Write("status");
                    string statusResponse = await Read();
                    await Write("get_time");
                    string getTimeResponse = await Read();
                    if (CurrentMediaItem.Length == 0)
                    {
                        await UpdateCurrentMediaLength();
                    }

                    Match m = Regex.Match(statusResponse, @"new input\: (.*) \)(?:[\S\s].*){3}state (\w+)");
                    if (m.Success)
                    {
                        string MediaItem = m.Groups[1].Value;
                        if (!CurrentMediaItem.Path.Equals(MediaItem))
                        {
                            await UpdateCurrentMediaTitle();
                            await UpdateCurrentMediaLength();
                            OnMediaItemChanged(MediaItem);
                        }
                        string MediaState = m.Groups[2].Value;
                        if (CurrentMediaItem.State.Equals(String.Empty)) { CurrentMediaItem.State = MediaState; }
                        else
                        {
                            if (!CurrentMediaItem.State.Equals(MediaState))
                            {
                                CurrentMediaItem.State = MediaState;
                                if (CurrentMediaItem.State.Equals("playing"))
                                {
                                    OnPlaybackResumed();
                                }
                                else if (CurrentMediaItem.State.Equals("paused"))
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
                            CurrentMediaItem.State = "stopped";
                            OnPlaybackStopped();
                        }
                    }

                    if(!String.IsNullOrWhiteSpace(getTimeResponse) && CurrentMediaItem.Length > 0)
                    {
                        UInt32.TryParse(getTimeResponse.Trim(), out uint currentTime);
                        float watchedPercent = (float)currentTime / (float)CurrentMediaItem.Length;
                        if(!CurrentMediaItem.WatchedPercentReached && watchedPercent >= Properties.Settings.Default.WatchedPercent)
                        {
                            OnWatchedPercentReached();
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch (IOException) { break; }
            }
            OnConnectionLost();
        }
    }
}
