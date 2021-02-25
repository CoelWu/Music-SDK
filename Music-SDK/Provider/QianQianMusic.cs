using Music.SDK.Models;
using Music.SDK.Models.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Music.SDK.Provider
{
    public class QianQianMusic : Music
    {
        public QianQianMusic() : base()
        {

        }

        private string CalculateSign(Dictionary<string, string> dict)
        {
            string secret = "0b50b02fd0d73a9c4c8c3a781c30845f";
            List<string> keys = new List<string>(dict.Keys);
            keys.Sort();
            string result = $"{keys[0]}={dict[keys[0]]}";
            for (int i = 1; i < keys.Count; i++)
            {
                result += $"&{keys[i]}={dict[keys[i]]}";
            }
            byte[] resultArr = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(Encoding.UTF8.GetBytes(result + secret));
            return BitConverter.ToString(resultArr).Replace("-", string.Empty).ToLower();
        }

        public async Task<List<SongItem>> SearchSong(string keyword, int currentPage = 0)
        {
            string url = "https://music.taihe.com/v1/search";

            var dict = new Dictionary<string, string>();
            dict.Add("word", keyword);
            dict.Add("timestamp", Convert.ToString(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()));
            dict.Add("sign", CalculateSign(dict));

            url += ConvertToQuery(dict);
            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();

            List<SongItem> songItems = new List<SongItem>();
            var obj = JObject.Parse(data);
            foreach (var song in obj["data"]["typeTrack"])
            {
                long songId = ToLong(((string)song["TSID"]).Replace("T", ""));
                long albumId = ToLong(((string)song["albumAssetCode"]).Replace("P", ""));
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.QianQianMusic,
                    SongUrl = (string)song["songurl"],
                    SongId = songId,
                    SongGId = Convert.ToString(songId),
                    SongName = (string)song["title"],
                    SongAlbumId = albumId,
                    SongAlbumName = (string)song["albumTitle"]
                };
                foreach (var artist in song["artist"])
                {
                    songItem.SongArtistId.Add(ToLong(((string)artist["artistCode"]).Replace("A", "")));
                    songItem.SongArtistName.Add((string)artist["name"]);
                }
                songItems.Add(songItem);
            }
            return songItems;
        }

        public async Task<PlayListItem> GetPlayList(long id)
        {
            string url = $"https://music.taihe.com/v1/tracklist/info?id={id}&pageSize=200";

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();

            List<SongItem> songItems = new List<SongItem>();
            var obj = JObject.Parse(data);

            foreach (var song in obj["data"]["trackList"])
            {
                long songId = ToLong(((string)song["TSID"]).Replace("T", ""));
                long albumId = ToLong(((string)song["albumAssetCode"]).Replace("P", ""));
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.QianQianMusic,
                    SongUrl = (string)song["songurl"],
                    SongId = songId,
                    SongGId = Convert.ToString(songId),
                    SongName = (string)song["title"],
                    SongAlbumId = albumId,
                    SongAlbumName = (string)song["albumTitle"]
                };
                foreach (var artist in song["artist"])
                {
                    songItem.SongArtistId.Add(ToLong(((string)artist["artistCode"]).Replace("A", "")));
                    songItem.SongArtistName.Add((string)artist["name"]);
                }
                songItems.Add(songItem);
            }

            PlayListItem playListItem = new PlayListItem
            {
                PlayListId = (long)obj["data"]["id"],
                PlayListName = (string)obj["data"]["title"],
                PlayListDesc = (string)obj["data"]["desc"],
                Songs = songItems
            };

            return playListItem;
        }

        public async Task<string> GetSongPlayUrl(SongItem song)
        {
            string url = $"https://music.taihe.com/v1/song/tracklink?TSID=T{song.SongId}";

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            var obj = JObject.Parse(data);
            string songPlayUrl = string.Empty;
            try
            {
                songPlayUrl = (string)obj["data"]["path"];
            }
            catch
            {
                songPlayUrl = (string)obj["data"]["trail_audio_info"]["path"];
            }
            return songPlayUrl;
        }

        public async Task<LyricItem> GetLyric(SongItem song)
        {
            string url = $"https://music.taihe.com/v1/song/tracklink?TSID=T{song.SongId}";

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            var obj = JObject.Parse(data);
            string lyricUrl = (string)obj["data"]["lyric"];
            string lyricData = await httpClient.GetAsync(lyricUrl).Result.Content.ReadAsStringAsync();
            LyricItem lyricItem = new LyricItem(lyricData);

            return lyricItem;
        }

        public async Task<LyricItem> GetLyricById(string id)
        {
            SongItem songItem = new SongItem { SongId = long.Parse(id) };
            return await GetLyric(songItem);
        }
    }
}
