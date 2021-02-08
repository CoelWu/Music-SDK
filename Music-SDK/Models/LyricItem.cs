using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Music.SDK.Models
{
    public class LyricItem
    {
        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public string LrcBy { get; set; }

        public string Offset { get; set; }

        public List<LineLyricItem> Lyric { get; set; }

        public LyricItem()
        {
            Lyric = new List<LineLyricItem>();
        }

        public LyricItem(string LrcText) : this()
        {
            string[] lines = LrcText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (line.StartsWith("[ti:"))
                {
                    Title = SplitInfo(line);
                }
                else if (line.StartsWith("[ar:"))
                {
                    Artist = SplitInfo(line);
                }
                else if (line.StartsWith("[al:"))
                {
                    Album = SplitInfo(line);
                }
                else if (line.StartsWith("[by:"))
                {
                    LrcBy = SplitInfo(line);
                }
                else if (line.StartsWith("[offset:"))
                {
                    Offset = SplitInfo(line);
                }
                else
                {
                    try
                    {
                        Regex regexword = new Regex(@".*\](.*)");
                        Match mcw = regexword.Match(line);
                        string word = mcw.Groups[1].Value;
                        if (word.Replace(" ", "") == "")
                            continue; // 如果为空歌词则跳过不处理
                        Regex regextime = new Regex(@"\[([0-9.:]*)\]", RegexOptions.Compiled);
                        MatchCollection mct = regextime.Matches(line);
                        foreach (Match item in mct)
                        {
                            LineLyricItem lineLyricItem = new LineLyricItem
                            {
                                Lyric = word,
                                Time = item.Groups[1].Value
                            };
                            Lyric.Add(lineLyricItem);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// 处理信息(私有方法)
        /// </summary>
        /// <param name="line"></param>
        /// <returns>返回基础信息</returns>
        static string SplitInfo(string line)
        {
            return line.Substring(line.IndexOf(":") + 1).TrimEnd(']');
        }

        public override string ToString()
        {
            string result = string.Empty;
            result += $"[ti:{Title}]\n";
            result += $"[ar:{Artist}]\n";
            result += $"[al:{Album}]\n";
            result += $"[by:{LrcBy}]\n";
            result += $"[offset:{Offset}]\n";
            foreach (var lineLyricItem in Lyric)
            {
                result += $"[{lineLyricItem.Time}]{lineLyricItem.Lyric}\n";
            }
            return result;
        }
    }
}
