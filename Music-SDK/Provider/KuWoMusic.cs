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
    public class KuWoMusic : Music
    {
        public KuWoMusic() : base()
        {

        }

        public async Task<List<SongItem>> SearchSong(string keyword, int currentPage = 0)
        {
            string url = $"https://search.kuwo.cn/r.s?ft=music&itemset=web_2013&client=kt&rformat=json&encoding=utf8&all={keyword}&pn={currentPage}&rn=20";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            List<SongItem> songItems = new List<SongItem>();
            var obj = JObject.Parse(data);
            foreach (var song in obj["abslist"])
            {
                int songId = int.Parse(((string)song["MUSICRID"]).Split('_')[1]);
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.KuWoMusic,
                    SongUrl = $"https://www.kuwo.cn/yinyue/{songId}",
                    SongId = songId,
                    SongGId = Convert.ToString(songId),
                    SongName = (string)song["NAME"],
                    SongArtistId = new List<long> { (long)song["ARTISTID"] },
                    SongArtistName = new List<string> { (string)song["ARTIST"] },
                    SongAlbumId = (long)song["ALBUMID"],
                    SongAlbumName = (string)song["ALBUM"]
                };
                songItems.Add(songItem);
            }
            return songItems;
        }

        public async Task<PlayListItem> GetPlayList(long id)
        {
            string url = $"https://nplserver.kuwo.cn/pl.svc?op=getlistinfo&pn=0&rn=200&encode=utf-8&keyset=pl2012&pcmp4=1&pid={id}&vipver=MUSIC_9.0.2.0_W1&newver=1";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            List<SongItem> songItems = new List<SongItem>();
            var obj = JObject.Parse(data);

            foreach (var song in obj["musiclist"])
            {
                long songId = ToLong((string)song["id"]);
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.KuWoMusic,
                    SongUrl = $"https://www.kuwo.cn/yinyue/{songId}",
                    SongId = songId,
                    SongGId = Convert.ToString(songId),
                    SongName = (string)song["name"],
                    SongArtistId = new List<long> { (long)song["artistid"] },
                    SongArtistName = new List<string> { (string)song["artist"] },
                    SongAlbumId = (long)song["albumid"],
                    SongAlbumName = (string)song["album"]
                };
                songItems.Add(songItem);
            }

            PlayListItem playListItem = new PlayListItem
            {
                PlayListId = (long)obj["id"],
                PlayListName = (string)obj["title"],
                PlayListDesc = (string)obj["info"],
                PlayListAuthor = (string)obj["uname"],
                Songs = songItems
            };

            return playListItem;
        }

        public async Task<string> GetSongPlayUrl(SongItem song)
        {
            string url = $"https://antiserver.kuwo.cn/anti.s?uesless=/resource/&format=mp3&rid=MUSIC_{song.SongId}&response=url&type=convert_url&";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<LyricItem> GetLyric(SongItem song)
        {
            string url = $"https://m.kuwo.cn/newh5/singles/songinfoandlrc?musicId={song.SongId}";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            var obj = JObject.Parse(data);
            LyricItem lyricItem = new LyricItem
            {
                Title = (string)obj["data"]["songinfo"]["songName"],
                Artist = (string)obj["data"]["songinfo"]["artist"],
                Album = (string)obj["data"]["songinfo"]["album"],
                Offset = "0"
            };
            foreach (var lyric in obj["data"]["lrclist"])
            {
                LineLyricItem lineLyricItem = new LineLyricItem
                {
                    Lyric = (string)lyric["lineLyric"],
                    Time = lrcTools.FormatTime(double.Parse((string)lyric["time"]))
                };
                lyricItem.Lyric.Add(lineLyricItem);
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
