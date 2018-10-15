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
                if (Regex.IsMatch(fileExtension, @"(.m4v|.3g2|.3gp|.nsv|.tp|.ts|.ty|.strm|.pls|.rm|.rmvb|.mpd|.m3u|.m3u8|.ifo|.mov|.qt|.divx|.xvid|.bivx|.vob|.nrg|.img|.iso|.udf|.pva|.wmv|.asf|.asx|.ogm|.m2v|.avi|.bin|.dat|.mpg|.mpeg|.mp4|.mkv|.mk3d|.avc|.vp3|.svq3|.nuv|.viv|.dv|.fli|.flv|.001|.wpl|.zip|.vdr|.dvr-ms|.xsp|.mts|.m2t|.m2ts|.evo|.ogv|.sdp|.avs|.rec|.url|.pxml|.vc1|.h264|.rcv|.rss|.mpls|.webm|.bdmv|.wtv|.trp|.f4v)", RegexOptions.IgnoreCase))
                {
                    string showFolder = Regex.Replace(file, @".*\\(.*?)\\.*?\\.*", "$1");
                    string seasonFolder = Regex.Replace(file, @".*\\.*?\\(.*?)\\.*", "$1");
                    Match seasonMatch = Regex.Match(seasonFolder, @"s(?:eason|taffel)?(?:\s?[-_]?\s?)?(\d{1,3})", RegexOptions.IgnoreCase);
                    if (seasonMatch.Success)
                    {
                        int seasonNumber = int.Parse(seasonMatch.Groups[1].Value);
                        Match episodeMatch = FilenameParser.ParseEpisode(Path.GetFileName(file).CleanFilename());
                        if (episodeMatch.Success)
                        {
                            int epEnd = episodeMatch.Groups["episodenumberend"].Success ? int.Parse(episodeMatch.Groups["episodenumberend"].Value) : 0;
                            if (int.Parse(episodeMatch.Groups["episodenumberstart"].Value) is int epStart && epStart > (seasonNumber * 100) && epStart < ((seasonNumber + 1) * 100 - 1))
                            {
                                epStart -= seasonNumber * 100;
                            }
                            var mediaItem = new MediaItem
                            {
                                AbsoluteNumber = false,
                                EpisodeNumberStart = epStart,
                                EpisodeNumberEnd = epEnd,
                                Season = seasonNumber,
                                FilePath = file
                            };
                            if (VideoFiles.ContainsKey(showFolder))
                            {
                                VideoFiles[showFolder].Add(mediaItem);
                            }
                            else VideoFiles.Add(showFolder, new List<MediaItem>() { mediaItem });
                        }
                    }
                    else
                    {
                        // When we dont find the folder appropriate folder structure resort to detecting on a per file basis
                        Match m = FilenameParser.Parse(Path.GetFileNameWithoutExtension(file).CleanFilename());
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
