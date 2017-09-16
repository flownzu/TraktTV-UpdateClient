using System.Text.RegularExpressions;

namespace TraktTVUpdateClient.Extension
{
    class FilenameParser
    {
        public static Regex[] Patterns = new Regex[]
        {
            new Regex(@"^\[(?<group>.+?)\][ ]?(?<seriesname>.*)[ ]?[-_ ][ ]?(?<episodenumberstart>\d+)[ ]?[-_][ ]?(?<episodenumberend>\d+)([\.\- ].*\d+[Pp])(?=.*\[(?<crc>.+?)\])?"),
            new Regex(@"^\[(?<group>.+?)\][ ]?(?<seriesname>.*)[ ]?[-_ ][ ]?(?<episodenumberstart>\d+)[ ]?[-_][ ]?(?<episodenumberend>\d+)(?=.*\[(?<crc>.+?)\])?"),
            new Regex(@"^\[(?<group>.+?)\][ ]?(?<seriesname>.*)[ ]?[-_ ][ ]?(?<episodenumber>\d+)([\.\- ].*\d+[Pp])(?=.*\[(?<crc>.+?)\])?"),
            new Regex(@"^\[(?<group>.+?)\][ ]?(?<seriesname>.*)[ ]?[-_ ][ ]?(?<episodenumber>\d+)(?=.*\[(?<crc>.+?)\])?"),
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
            new Regex(@"^(?<seriesname>.*?)[ \._\-](?<episodenumber>\d+)")
        };

        public static Match Parse(string fileName)
        {
            foreach (Regex r in Patterns)
            {
                Match m = r.Match(fileName);
                if (m.Success)
                {
                    return m;
                }
            }
            return default;
        }
    }
}
