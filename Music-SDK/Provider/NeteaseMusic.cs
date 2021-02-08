using Music.SDK.Extensions;
using Music.SDK.Models;
using Music.SDK.Models.Enums;
using Music.SDK.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Music.SDK.Provider
{
    public class NeteaseMusic : Music
    {
        public NeteaseMusic() : base()
        {

        }

        public string RandomHexString(int digits)
        {
            Random random = new Random();
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result.ToLower();
            return result + random.Next(16).ToString("X").ToLower();
        }

        public string RsaEncrypt(string text, string pubKey, string modulus)
        {
            string reversedText = text.Reverse();
            BigInteger baseNum = BigInteger.Parse(reversedText.ToHex(), NumberStyles.AllowHexSpecifier);
            BigInteger baseExp = BigInteger.Parse(pubKey, NumberStyles.AllowHexSpecifier);
            BigInteger basePow = BigInteger.Parse(modulus, NumberStyles.AllowHexSpecifier);
            BigInteger bigInteger = BigInteger.ModPow(baseNum, baseExp, basePow);
            return bigInteger.ToRadixString(16).PadLeft(256, '0').ToLower();
        }

        public Dictionary<string, string> EncryptRequest(object originalData)
        {
            string modulus = "00e0b509f6259df8642dbc35662901477df22677ec152b5ff68ace615bb7b725152b3ab17a876aea8a5aa76d2e417629ec4ee341f56135fccf695280104e0312ecbda92557c93870114af6c9d05c4f7f0c3685b7a46bee255932575cce10b424d813cfe4875d3e82047b97ddef52741d546b8e289dc6935b3ece0462db0a22b8e7";
            string nonce = "0CoJUm6Qyw8W8jud";
            string pubKey = "010001";
            string secKey = RandomHexString(16);
            string ivString = "0102030405060708";
            string encText = JsonConvert.SerializeObject(originalData);
            encText = AESTools.Encrypt(encText, nonce, ivString);
            encText = AESTools.Encrypt(encText, secKey, ivString);
            string encSecKey = RsaEncrypt(secKey, pubKey, modulus);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("params", encText);
            dict.Add("encSecKey", encSecKey);
            return dict;
        }
        
        public async Task<List<SongItem>> SearchSong(string keyword, int currentPage = 0)
        {
            string url = $"https://music.163.com/api/search/pc?s={keyword}&limit=20&offset=0&type=1";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.PostAsync(url, null).Result.Content.ReadAsStringAsync();
            List<SongItem> songItems = new List<SongItem>();

            var obj = JObject.Parse(data);
            foreach (var song in obj["result"]["songs"])
            {
                long songId = (long)song["id"];
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.NeteaseMusic,
                    SongUrl = $"http://music.163.com/#/song?id={songId}",
                    SongId = songId,
                    SongGId = Convert.ToString(songId),
                    SongName = (string)song["name"],
                    SongAlbumId = (long)song["album"]["id"],
                    SongAlbumName = (string)song["album"]["name"]
                };
                foreach (var artist in song["artists"])
                {
                    songItem.SongArtistId.Add((long)artist["id"]);
                    songItem.SongArtistName.Add((string)artist["name"]);
                }
                songItems.Add(songItem);
            }
            return songItems;
        }

        public async Task<PlayListItem> GetPlayList(long id)
        {
            string url = "http://music.163.com/weapi/v3/playlist/detail";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            var postData = new
            {
                id = id,
                offset = 0,
                total = true,
                limit = 1000,
                n = 1000,
                csrf_token = ""
            };
            var dict = EncryptRequest(postData);

            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
            string data = await httpClient.SendAsync(req).Result.Content.ReadAsStringAsync();

            List<SongItem> songItems = new List<SongItem>();
            var obj = JObject.Parse(data);
            foreach (var song in obj["playlist"]["tracks"])
            {
                long songId = (long)song["id"];
                SongItem songItem = new SongItem
                {
                    SongPlatform = PlatformType.NeteaseMusic,
                    SongUrl = $"http://music.163.com/#/song?id={songId}",
                    SongId = songId,
                    SongGId = Convert.ToString(songId),
                    SongName = (string)song["name"],
                    SongAlbumId = (long)song["al"]["id"],
                    SongAlbumName = (string)song["al"]["name"]
                };
                foreach (var artist in song["ar"])
                {
                    songItem.SongArtistId.Add((long)artist["id"]);
                    songItem.SongArtistName.Add((string)artist["name"]);
                }
                songItems.Add(songItem);
            }

            PlayListItem playListItem = new PlayListItem
            {
                PlayListId = (long)obj["playlist"]["id"],
                PlayListName = (string)obj["playlist"]["name"],
                PlayListDesc = (string)obj["playlist"]["description"],
                PlayListAuthor = (string)obj["playlist"]["creator"]["nickname"],
                Songs = songItems
            };

            return playListItem;
        }

        public async Task<string> GetSongPlayUrl(SongItem song)
        {
            string url = "https://music.163.com/weapi/song/enhance/player/url/v1?csrf_token=";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            var postData = new
            {
                ids = "[" + song.SongId + "]",
                level = "standard",
                encodeType = "aac",
                csrf_token = ""
            };
            var dict = EncryptRequest(postData);

            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
            string data = await httpClient.SendAsync(req).Result.Content.ReadAsStringAsync();

            var obj = JObject.Parse(data);
            return (string)obj["data"][0]["url"];
        }

        public async Task<LyricItem> GetLyric(SongItem song)
        {
            string url = $"https://music.163.com/api/song/media?id={song.SongId}";
            httpClient = headerHacker.ApplyHeader(url, httpClient);

            string data = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
            var obj = JObject.Parse(data);

            string lyrics = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes((string)obj["lyric"]));
            LyricItem lyricItem = new LyricItem(lyrics);
            return lyricItem;
        }

        public async Task<LyricItem> GetLyricById(string id)
        {
            SongItem songItem = new SongItem { SongId = long.Parse(id) };
            return await GetLyric(songItem);
        }
    }
}
