using Music.SDK.Models;
using Music.SDK.Models.Enums;
using Music.SDK.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Music.SDK.Provider
{
    public class QQMusic : Music
    {
        public QQMusic() : base()
        {

        }

        public async Task<List<SongItem>> SearchSong(string keyword, int currentPage = 0)
        {
            string url = $"http://i.y.qq.com/s.music/fcgi-bin/search_for_qq_cp?g_tk=938407465&uin=0&format=jsonp&inCharset=utf-8&outCharset=utf-8&notice=0&platform=h5&needNewCode=1&w={keyword}&zhidaqu=1&catZhida=1&t=0&flag=1&ie=utf-8&sem=1&aggr=0&perpage=20&n=20&p={currentPage}&remoteplace=txt.mqq.all&_=1459991037831&jsonpCallback=jsonp4";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            data = data.Substring("jsonp4(".Length, data.Length - "jsonp4()".Length);

            List<SongItem> songItems = new List<SongItem>();
            var obj = JObject.Parse(data);
            foreach (var song in obj["data"]["song"]["list"])
            {
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.QQMusic,
                    SongUrl = (string)song["songurl"],
                    SongId = (long)song["songid"],
                    SongMId = (string)song["songmid"],
                    SongGId = (string)song["songmid"],
                    SongName = (string)song["songname"],
                    SongAlbumId = (long)song["albumid"],
                    SongAlbumMId = (string)song["alubmmid"],
                    SongAlbumName = (string)song["albumname"]
                };
                foreach (var artist in song["singer"])
                {
                    songItem.SongArtistId.Add((long)artist["id"]);
                    songItem.SongArtistMId.Add((string)artist["mid"]);
                    songItem.SongArtistName.Add((string)artist["name"]);
                }
                songItems.Add(songItem);
            }
            return songItems;
        }

        public async Task<PlayListItem> GetPlayList(long id)
        {
            string url = $"http://i.y.qq.com/qzone-music/fcg-bin/fcg_ucc_getcdinfo_byids_cp.fcg?type=1&json=1&utf8=1&onlysong=0&jsonpCallback=jsonCallback&nosign=1&disstid={id}&g_tk=5381&loginUin=0&hostUin=0&format=jsonp&inCharset=GB2312&outCharset=utf-8&notice=0&platform=yqq&jsonpCallback=jsonCallback&needNewCode=0";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            data = data.Substring("jsonCallback(".Length, data.Length - "jsonCallback()".Length);

            List<PlayListItem> playListItems = new List<PlayListItem>();
            List<SongItem> songItems = new List<SongItem>();
            var obj = JObject.Parse(data);
            foreach (var playlist in obj["cdlist"])
            {
                foreach (var song in playlist["songlist"])
                {
                    SongItem songItem = new SongItem
                    {
                        SongPlatform = PlatformType.QQMusic,
                        SongUrl = (string)song["songurl"],
                        SongId = (long)song["songid"],
                        SongMId = (string)song["songmid"],
                        SongGId = (string)song["songmid"],
                        SongName = (string)song["songname"],
                        SongAlbumId = (long)song["albumid"],
                        SongAlbumMId = (string)song["alubmmid"],
                        SongAlbumName = (string)song["albumname"]
                    };
                    foreach (var artist in song["singer"])
                    {
                        songItem.SongArtistId.Add((long)artist["id"]);
                        songItem.SongArtistMId.Add((string)artist["mid"]);
                        songItem.SongArtistName.Add((string)artist["name"]);
                    }
                    songItems.Add(songItem);
                }

                PlayListItem playListItem = new PlayListItem
                {
                    PlayListId = ToLong((string)playlist["disstid"]),
                    PlayListName = (string)playlist["dissname"],
                    PlayListDesc = (string)playlist["desc"],
                    PlayListAuthor = (string)playlist["nickname"],
                    Songs = songItems
                };
                playListItems.Add(playListItem);
                songItems.Clear();
            }

            return playListItems[0];
        }

        public async Task<string> GetSongPlayUrl(SongItem song)
        {
            string url = "https://u.y.qq.com/cgi-bin/musicu.fcg?data%3D%7B%22req%22%3A%7B%22module%22%3A%22CDN.SrfCdnDispatchServer%22%2C%22method%22%3A%22GetCdnDispatch%22%2C%22param%22%3A%7B%22guid%22%3A%221535153710%22%2C%22calltype%22%3A0%2C%22userip%22%3A%22%22%7D%7D%2C%22req_0%22%3A%7B%22module%22%3A%22vkey.GetVkeyServer%22%2C%22method%22%3A%22CgiGetVkey%22%2C%22param%22%3A%7B%22guid%22%3A%221535153710%22%2C%22songmid%22%3A%5B%22" + song.SongMId + "%22%5D%2C%22songtype%22%3A%5B0%5D%2C%22uin%22%3A%220%22%2C%22loginflag%22%3A1%2C%22platform%22%3A%2220%22%7D%7D%2C%22comm%22%3A%7B%22uin%22%3A0%2C%22format%22%3A%22json%22%2C%22ct%22%3A24%2C%22cv%22%3A0%7D%7D";
            url = HttpUtility.UrlDecode(url);
            httpClient = headerHacker.ApplyHeader(url, httpClient);
            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            var obj = JObject.Parse(data);
            return (string)obj["req"]["data"]["sip"][0] + (string)obj["req_0"]["data"]["midurlinfo"][0]["purl"];
        }

        public async Task<LyricItem> GetLyric(SongItem song)
        {
            string url = $"http://i.y.qq.com/lyric/fcgi-bin/fcg_query_lyric.fcg?songmid={song.SongMId}&loginUin=0&hostUin=0&format=jsonp&inCharset=GB2312&outCharset=utf-8&notice=0&platform=yqq&jsonpCallback=MusicJsonCallback&needNewCode=0";
            httpClient = headerHacker.ApplyHeader(url, httpClient);
            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            data = data.Substring("MusicJsonCallback(".Length, data.Length - "MusicJsonCallback()".Length);

            var obj = JObject.Parse(data);
            string lyrics = Encoding.UTF8.GetString(Convert.FromBase64String((string)obj["lyric"]));
            LyricItem lyricItem = new LyricItem(lyrics);
            return lyricItem;
        }

        public async Task<LyricItem> GetLyricById(string id)
        {
            SongItem songItem = new SongItem { SongMId = id };
            return await GetLyric(songItem);
        }
    }
}
