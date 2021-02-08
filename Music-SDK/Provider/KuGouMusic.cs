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
    public class KuGouMusic : Music
    {
        public KuGouMusic() : base()
        {

        }

        public async Task<List<SongItem>> SearchSong(string keyword, int currentPage = 0)
        {
            string url = $"https://songsearch.kugou.com/song_search_v2?keyword={keyword}&page={currentPage}";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            List<SongItem> songItems = new List<SongItem>();
            var obj = JObject.Parse(data);
            foreach (var song in obj["data"]["lists"])
            {
                string fileHash = (string)song["FileHash"];
                long albumId = ToLong((string)song["AlbumID"]);
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.KuGouMusic,
                    SongUrl = $"https://www.kugou.com/song/#hash={fileHash}&album_id={albumId}",
                    SongGId = fileHash,
                    SongFileHash = fileHash,
                    SongName = (string)song["SongName"],
                    SongAlbumId = albumId,
                    SongAlbumName = (string)song["AlbumName"]
                };
                songItem.SongArtistName.AddRange(((string)song["SingerName"]).Split(new string[] { "、" }, StringSplitOptions.None));
                foreach (var id in song["SingerId"])
                {
                    songItem.SongArtistId.Add((long)id);
                }
                songItems.Add(songItem);
            }
            return songItems;
        }

        public async Task<PlayListItem> GetPlayList(long id)
        {
            string url = $"http://m.kugou.com/plist/list/{id}?json=true";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            List<SongItem> songItems = new List<SongItem>();
            var obj = JObject.Parse(data);

            foreach (var song in obj["list"]["list"]["info"])
            {
                string fileHash = (string)song["hash"];
                long albumId = ToLong((string)song["album_id"]);
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.KuGouMusic,
                    SongUrl = $"https://www.kugou.com/song/#hash={fileHash}&album_id={albumId}",
                    SongGId = fileHash,
                    SongFileHash = fileHash,
                    SongName = (string)song["filename"],
                    SongAlbumId = ToLong((string)song["album_id"]),
                };
                songItems.Add(songItem);
            }

            PlayListItem playListItem = new PlayListItem
            {
                PlayListId = (long)obj["info"]["list"]["specialid"],
                PlayListName = (string)obj["info"]["list"]["specialname"],
                PlayListDesc = (string)obj["info"]["list"]["intro"],
                PlayListAuthor = (string)obj["info"]["list"]["nickname"],
                Songs = songItems
            };

            return playListItem;
        }

        public string RandomIntString()
        {
            Random random = new Random();
            int num = random.Next(1, 99);
            return Convert.ToString(num);
        }

        public string TimestampString()
        {
            DateTime dateTime = DateTime.Now;
            return dateTime.ToUniversalTime().ToString("O");
        }

        public async Task<string> GetSongPlayUrl(SongItem song)
        {
            string url = "https://wwwapi.kugou.com/yy/index.php?r=play/getdata";
            httpClient = headerHacker.ApplyHeader(url, httpClient);
            string jqueryHeader = $"jQuery1910{RandomIntString()}_{TimestampString()}";
            url = $"{url}&callback={jqueryHeader}&hash={song.SongFileHash}&album_id={song.SongAlbumId}&_={TimestampString()}";

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            data = data.Substring(jqueryHeader.Length + 1, data.Length - jqueryHeader.Length - 1 - 2 );
            var obj = JObject.Parse(data);
            return (string)obj["data"]["play_url"];
        }

        public async Task<LyricItem> GetLyric(SongItem song)
        {
            string url = $"https://www.kugou.com/yy/index.php?r=play/getdata&hash={song.SongFileHash}&album_id={song.SongAlbumId}";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            var obj = JObject.Parse(data);
            string lyrics = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes((string)obj["data"]["lyrics"]));
            LyricItem lyricItem = new LyricItem(lyrics);
            return lyricItem;
        }

        public async Task<LyricItem> GetLyricById(string id, string albumId)
        {
            SongItem songItem = new SongItem { SongFileHash = id, SongAlbumId = long.Parse(albumId) };
            return await GetLyric(songItem);
        }
    }
}
