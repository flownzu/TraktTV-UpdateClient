using System;
using System.Text.RegularExpressions;

namespace TraktTVUpdateClient.Extension
{
    class FilenameParser
    {
        public static Lazy<Regex[]> CompletePatterns = new Lazy<Regex[]>
        (() =>
                new Regex[]
                {
                    new Regex(@"^[\(\[](?<group>.+?)[\)\]][ ]?(?<seriesname>.*)[ ]?[-_ ][ ]?(?<episodenumberstart>\d+)[ ]?[-_][ ]?(?<episodenumberend>\d+)([\.\- ].*\d+[Pp])(?=.*\[(?<crc>.+?)\])?"),
                    new Regex(@"^[\(\[](?<group>.+?)[\)\]][ ]?(?<seriesname>.*)[ ]?[-_ ][ ]?(?<episodenumberstart>\d+)[ ]?[-_][ ]?(?<episodenumberend>\d+)(?=.*\[(?<crc>.+?)\])?"),
                    new Regex(@"^[\(\[](?<group>.+?)[\)\]][ ]?(?<seriesname>.*)[ ]?[-_ ][ ]?(?<episodenumber>\d+)([\.\- ].*\d+[Pp])(?=.*\[(?<crc>.+?)\])?"),
                    new Regex(@"^[\(\[](?<group>.+?)[\)\]][ ]?(?<seriesname>.*)[ ]?[-_ ][ ]?(?<episodenumber>\d+)(?=.*\[(?<crc>.+?)\])?"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?[Ss](?<seasonnumber>[0-9]+)[\.\- ]?[Ee](?<episodenumberstart>[0-9]+)([\.\- ]+[Ss](\k<seasonnumber>)[\.\- ]?[Ee][0-9]+)*([\.\- ]+[Ss](\k<seasonnumber>)[\.\- ]?[Ee](?<episodenumberend>[0-9]+))"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?[Ss](?<seasonnumber>[0-9]+)[\.\- ]?[Ee](?<episodenumberstart>[0-9]+)([\.\- ]?[Ee][0-9]+)*[\.\- ]?[Ee](?<episodenumberend>[0-9]+)"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?(?<seasonnumber>[0-9]+)[xX](?<episodenumberstart>[0-9]+)([ \._\-]+(\k<seasonnumber>)[xX][0-9]+)*([ \._\-]+(\k<seasonnumber>)[xX](?<episodenumberend>[0-9]+))"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?(?<seasonnumber>[0-9]+)[xX](?<episodenumberstart>[0-9]+)([xX][0-9]+)*[xX](?<episodenumberend>[0-9]+)"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?[Ss](?<seasonnumber>[0-9]+)[\.\- ]?[Ee](?<episodenumberstart>[0-9]+)([\-][Ee]?[0-9]+)*[\-][Ee]?(?<episodenumberend>[0-9]+)([\.\- ]\d+[Pp])"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?[Ss](?<seasonnumber>[0-9]+)[\.\- ]?[Ee](?<episodenumberstart>[0-9]+)([\-][Ee]?[0-9]+)*[\-][Ee]?(?<episodenumberend>[0-9]+)"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?(?<seasonnumber>[0-9]+)[xX](?<episodenumberstart>[0-9]+)([\-+][0-9]+)*[\-+](?<episodenumberend>[0-9]+)([\.\- ]\d+[Pp])"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?(?<seasonnumber>[0-9]+)[xX](?<episodenumberstart>[0-9]+)([\-+][0-9]+)*[\-+](?<episodenumberend>[0-9]+)"),
                    new Regex(@"^(?<seriesname>.*?)[ \._\-]\[?(?<seasonnumber>[0-9]+)[xX](?<episodenumberstart>[0-9]+)([\-+] [0-9]+)*[\-+](?<episodenumberend>[0-9]+)\]"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?\[(?<episodenumber>[0-9]+)\]"),
                    new Regex(@"^(?<seriesname>.*?)[ \._\-][Ss](?<seasonnumber>[0-9]{2})[\.\- ]?(?<episodenumber>[0-9]{2})"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?\[?(?<seasonnumber>[0-9]+)[xX](?<episodenumber>[0-9]+)\]?"),
                    new Regex(@"^((?<seriesname>.*?)[ \._\-])?\[?[Ss](?<seasonnumber>[0-9]+)[ ]?[\._\- ]?[ ]?[Ee]?(?<episodenumber>[0-9]+)\]?"),
                    new Regex(@"^((?<seriesname>.*?))[ \._\-]?\[(?<seasonnumber>[0-9]+?)[.](?<episodenumber>[0-9]+?)\][ \._\-]?"),
                    new Regex(@"^(?<seriesname>.*?)[ ]?[ \._\-][ ]?[Ss](?<seasonnumber>[0-9]+)[\.\- ]?[Ee]?[ ]?(?<episodenumber>[0-9]+)"),
                    new Regex(@"^(?<seriesname>.*?)[ ]?[ \._\-][ ]?([Ee]pisode[ ]\d+)?[\[]?[sS][ ]?(?<seasonnumber>\d+)[ -]*?([eE][pP])[ ]?(?<episodenumber>\d+)"),
                    new Regex(@"^(?<seriesname>.*?)[ \._\-](?<episodenumber>[0-9]+)[ \._\-]?of[ \._\-]?\d+"),
                    new Regex(@"^(?<seriesname>.*?)[ \._\-]?(?<episodenumberstart>\d+)[ ]?[-_ ][ ]?(?<episodenumberend>\d+)"),
                    new Regex(@"^(?i)(?<seriesname>.*?)[ \._\-](?:part|pt)[\._ -](?<episodenumberstart>[0-9]+)(?:[ \._-](?:and|&|to)[ \._-](?:part|pt)?[ \._-](?:[0-9]+))*[ \._-](?:and|&|to)[ \._-]?(?:part|pt)?[ \._-](?<episodenumberend>[0-9]+)"),
                    new Regex(@"^(?i)(?<seriesname>.*?)[ \._\-](?:part|pt)[\._ -](?<episodenumber>[0-9]+)"),
                    new Regex(@"^(?<seriesname>.*?)[ ]?[Ss]eason[ ]?(?<seasonnumber>[0-9]+)[ ]?[Ee]pisode[ ]?(?<episodenumber>[0-9]+)"),
                    new Regex(@"^(?<seriesname>.*?)[ \._\-](?<seasonnumber>[0-9]{1})(?<episodenumber>[0-9]{2})$"),
                    new Regex(@"^(?<seriesname>.*?)[ \._\-](?<seasonnumber>[0-9]{2})(?<episodenumber>[0-9]{2,3})"),
                    new Regex(@"^(?<seriesname>.*?)[ \._\-][Ee](?<episodenumber>[0-9]+)"),
                    new Regex(@"^(?<seriesname>.*?)[sS](?<seasonnumber>\d+)[eE](?<episodenumber>\d+)"),
                    new Regex(@"^(?<seriesname>.*?)[ \._\-](?<episodenumber>\d+)"),
                    new Regex(@"^[\[\(](?<group>.+?)[\]\)][_\.\- ](?<seriesname>.*)[_\.\- ][Ee][Pp](?<episodenumber>\d+)"),
                    new Regex(@"(^[\[\(](?<group>.+?)[\]\)][_\.\- ])?(?<seriesname>.*)[_\.\- ][Ee][Pp](?<episodenumber>\d+)")
                }
        );

        public static Lazy<Regex[]> EpisodePatterns = new Lazy<Regex[]>
        (() =>
            new Regex[]
            {
                new Regex(@"(?:(?:\b|_)(?:ep?[ .]?)?(?<episodenumberstart>\d{1,4})(?:\.(0?[a-i1-9]))?(?:[_ ]?v\d)?[\s_.-]+)(?![^([{]*\b\d{1,4}(?:[_\s]?v\d+)?\b)(?:\w+[\s_.-]*)*?(?:(?:\[[^]]+\]|\([^)]+\)|\{[^}]+\})(?:[\s_]*))*(?:[[({][\da-f]{8}[])}])?"),
                new Regex(@"(?:\b|(?:[e][p]?))\s?(?<episodenumberstart>[0-9]+)\b(?:\s?-\s?|\b)?(?<episodenumberend>[0-9]+)?")
            }
        );

        public static Match Parse(string fileName)
        {
            foreach (Regex r in CompletePatterns.Value)
            {
                Match m = r.Match(fileName);
                if (m.Success)
                {
                    return m;
                }
            }
            return Match.Empty;
        }

        public static Match ParseEpisode(string fileName)
        {
            foreach (Regex r in EpisodePatterns.Value)
            {
                Match m = r.Match(fileName);
                if (m.Success)
                {
                    return m;
                }
            }
            return Match.Empty;
        }
    }
}
