using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Tetris.Core
{
    public class TetrisApiResultOutput
    {
        public enum TetrisOutputType { Error, Exception, Warning, Info }

        [JsonIgnore]
        public TetrisOutputType Type { get; set; }

        public string Key { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public TetrisApiResultOutput(string rawMessage)
        {
            var root = rawMessage.Split("=", StringSplitOptions.RemoveEmptyEntries);

            if (root.Length > 1) 
            { 
                Key = root[0];
                rawMessage = rawMessage.Replace($"{Key}=", string.Empty);
            }

            var regexException = new Regex(@"(\[ex(.*?)\])|(\[exception(.*?)\])", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var regexErro = new Regex(@"(\[e(.*?)\])|(\[error(.*?)\])", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var regexWarning = new Regex(@"(\[w(.*?)\])|(\[warning(.*?)\])", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var regexInfo = new Regex(@"(\[i(.*?)\])|(\[info(.*?)\])", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (regexException.IsMatch(rawMessage))            
                LoadMatch(rawMessage, regexException.Match(rawMessage), TetrisOutputType.Exception);

            else if (regexErro.IsMatch(rawMessage))
                LoadMatch(rawMessage, regexErro.Match(rawMessage), TetrisOutputType.Error);

            else if (regexWarning.IsMatch(rawMessage))
                LoadMatch(rawMessage, regexWarning.Match(rawMessage), TetrisOutputType.Warning);

            else if (regexInfo.IsMatch(rawMessage))
                LoadMatch(rawMessage, regexInfo.Match(rawMessage), TetrisOutputType.Info);
            else
            {
                Type = TetrisOutputType.Error;
                Message = rawMessage;
            }
        }

        private void LoadMatch(string rawMessage, Match match, TetrisOutputType type)
        {
            Type = type;
            Message = rawMessage.Replace(match.Value, string.Empty);

            var tag = match.Value.Replace("[", string.Empty).Replace("]", string.Empty);
            var tagContent = tag.Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (tagContent.Length == 2)
                Code = tagContent[1];
        }
    }
}
