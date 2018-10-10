using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using TraktTVUpdateClient.Extension;

namespace TraktTVUpdateClient.Media
{
    public class MediaLibrary
    {
        [JsonProperty(PropertyName = "libraryPath")]
        public string LibraryPath { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public Dictionary<string, List<MediaItem>> VideoFiles { get; set; }

        public MediaLibrary(string path, string lang)
        {
            LibraryPath = path;
            Language = lang;
            VideoFiles = new Dictionary<string, List<MediaItem>>(StringComparer.OrdinalIgnoreCase);
        }

        public void GetAllFiles(string path = "")
        {
            if (VideoFiles.Count > 0 && string.IsNullOrEmpty(path)) VideoFiles.Clear();
            if (string.IsNullOrEmpty(path)) path = LibraryPath;
            foreach (string file in Directory.GetFiles(path))
            {
                string fileExtension = Path.GetExtension(file);
                if (Regex.IsMatch(fileExtension, @"\.(webm|mkv|flv|vob|ogv|ogg|avi|mts|m2ts|mov|wmv|yuv|rm|rmvb|amv|mp4|m4p|m4v|mpg|mp2|mpeg|mpe|mpv|m2v|f4v|f4p|f4a|f4b|3gp|3g2)", RegexOptions.IgnoreCase))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    Match m = FilenameParser.Parse(fileName.CleanFilename());
                    if (m.Success)
                    {
                        string showName = m.Groups["seriesname"].Value.CleanString();
                        if (!string.IsNullOrEmpty(showName))
                        {
                            int seasonNumber = m.Groups["seasonnumber"].Success ? int.Parse(m.Groups["seasonnumber"].Value) : 0;
                            int episodeNumberStart = m.Groups["episodenumberstart"].Success ? int.Parse(m.Groups["episodenumberstart"].Value) : m.Groups["episodenumber"].Success ? int.Parse(m.Groups["episodenumber"].Value) : 0;
                            int episodeNumberEnd = m.Groups["episodenumberend"].Success ? int.Parse(m.Groups["episodenumberend"].Value) : 0;
                            Console.WriteLine(showName + ": Season " + seasonNumber + " Episode" + (episodeNumberEnd == 0 ? " " + episodeNumberStart : "s " + episodeNumberStart + " - " + episodeNumberEnd));
                            if (seasonNumber == 0)
                            {
                                var mediaItem = new MediaItem
                                {
                                    AbsoluteNumber = true,
                                    AbsoluteNumberStart = episodeNumberStart,
                                    AbsoluteNumberEnd = episodeNumberEnd,
                                    FilePath = file,
                                    Season = 0
                                };
                                if (VideoFiles.ContainsKey(showName))
                                {
                                    VideoFiles[showName].Add(mediaItem);
                                }
                                else VideoFiles.Add(showName, new List<MediaItem>() { mediaItem });
                            }
                            else
                            {
                                var mediaItem = new MediaItem
                                {
                                    AbsoluteNumber = false,
                                    EpisodeNumberStart = episodeNumberStart,
                                    EpisodeNumberEnd = episodeNumberEnd,
                                    FilePath = file,
                                    Season = seasonNumber
                                };
                                if (VideoFiles.ContainsKey(showName))
                                {
                                    VideoFiles[showName].Add(mediaItem);
                                }
                                else VideoFiles.Add(showName, new List<MediaItem>() { mediaItem });
                            }
                        }
                    }
                }
            }
            foreach (string d in Directory.GetDirectories(path))
            {
                GetAllFiles(d);
            }
        }

        /*public bool PlayEpisode(string showName, int seasonNumber, int episodeNumber)
        {
            if (VideoFiles.ContainsKey(showName))
            {
                var episode = VideoFiles[showName].Where(x => x.AbsoluteNumber == (seasonNumber == 0) && ((x.AbsoluteNumberStart == episodeNumber || x.AbsoluteNumberEnd == episodeNumber) || (x.EpisodeNumberStart == episodeNumber || x.EpisodeNumberEnd == episodeNumber))).FirstOrDefault();
                if (episode != null)
                {
                    episode.Play();
                    return true;
                }
                else return false;
            }
            else return false;
        }*/
    }
}
