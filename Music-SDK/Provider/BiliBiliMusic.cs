using Music.SDK.Models;
using Music.SDK.Models.Enums;
using Music.SDK.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Music.SDK.Provider
{
    public class BiliBiliMusic : Music
    {

        public BiliBiliMusic() : base()
        {
        }

        public async Task<List<SongItem>> SearchSong(string keyword, int currentPage = 0)
        {
            string url = $"https://api.bilibili.com/audio/music-service-c/s?search_type=audio&keyword={keyword}&page={currentPage}&pagesize=20";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            var obj = JObject.Parse(data);
            List<SongItem> songItems = new List<SongItem>();
            foreach (var song in obj["data"]["result"])
            {
                long songId = (long)song["id"];
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.BiliBiliMusic,
                    SongUrl = $"https://www.bilibili.com/audio/au{songId}",
                    SongId = songId,
                    SongGId = Convert.ToString(songId),
                    SongName = (string)song["title"],
                    SongArtistName = new List<string>() { (string)song["up_name"] }
                };
                songItems.Add(songItem);
            }
            return songItems;
        }

        public async Task<PlayListItem> GetPlayList(long id)
        {
            string url = $"https://www.bilibili.com/audio/music-service-c/web/menu/info?sid={id}";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            var obj = JObject.Parse(data);

            PlayListItem playListItem = new PlayListItem
            {
                PlayListId = (long)obj["data"]["menuId"],
                PlayListName = (string)obj["data"]["title"],
                PlayListDesc = (string)obj["data"]["intro"],
                PlayListAuthor = (string)obj["data"]["uname"]
            };

            url = $"https://www.bilibili.com/audio/music-service-c/web/song/of-menu?sid={id}&pn=1&ps=100";
            data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            List<SongItem> songItems = new List<SongItem>();
            obj = JObject.Parse(data);

            foreach (var song in obj["data"]["data"])
            {
                long songId = (long)song["id"];
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.BiliBiliMusic,
                    SongUrl = $"https://www.bilibili.com/audio/au{songId}",
                    SongId = songId,
                    SongGId = Convert.ToString(songId),
                    SongName = (string)song["title"],
                    SongArtistName = new List<string>() { (string)song["author"] }
                };
                songItems.Add(songItem);
            }

            playListItem.Songs = songItems;
            return playListItem;
        }

        public async Task<string> GetSongPlayUrl(SongItem song)
        {
            string url = $"https://www.bilibili.com/audio/music-service-c/web/url?sid={song.SongId}";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            var obj = JObject.Parse(data);
            return (string)obj["data"]["cdns"][0];
        }

        public async Task<LyricItem> GetLyric(SongItem song)
        {
            string url = $"https://www.bilibili.com/audio/music-service-c/web/song/info?sid={song.SongId}";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            var obj = JObject.Parse(data);
            string lyricUrl = (string)obj["data"]["lyric"];
            string lyricData = await httpClient.GetAsync(lyricUrl).Result.Content.ReadAsStringAsync();
            LyricItem lyricItem = new LyricItem(lyricData);
            if (string.IsNullOrWhiteSpace(lyricItem.Title))
            {
                lyricItem.Title = (string)obj["data"]["title"];
            }
            if (string.IsNullOrWhiteSpace(lyricItem.Artist))
            {
                lyricItem.Artist = (string)obj["data"]["author"];
            }
            return lyricItem;
        }

        public async Task<LyricItem> GetLyricById(string id)
        {
            SongItem songItem = new SongItem { SongId = long.Parse(id) };
            return await GetLyric(songItem);
        }
    }
}
