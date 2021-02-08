using Music.SDK.Models;
using Music.SDK.Models.Enums;
using Music_SDK;
using System;
using System.Collections.Generic;

namespace MusicSDK_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 测试QQ音乐，歌曲名 = 星辰大海
            //music.GetSongPlayUrl(new Music.SDK.Models.SongItem { SongMId = "002LNOds0rYvpK" });
            // 测试网抑云音乐，歌曲名 = 经济舱（Live）
            //music.GetSongPlayUrl(new Music.SDK.Models.SongItem { SongId = 1487528112 });
            // 测试网抑云音乐，歌单Id = 5376368685
            //music.GetPlayList(5376368685);
            // 测试酷我音乐, 歌曲名 = 白月光与朱砂痣
            //music.GetSongPlayUrl(new Music.SDK.Models.SongItem { SongId = 160783739 });
            // 测试酷狗音乐, 歌曲名 = 四季予你
            //music.GetSongPlayUrl(new Music.SDK.Models.SongItem { SongFileHash = "D3ED05ADA906113520BE598E784C038E", SongAlbumId = 40693398 });
            // 测试B站音乐，歌曲名 = 处处零
            //music.GetSongPlayUrl(new Music.SDK.Models.SongItem { SongId = 1438481 });
        }
    }
}
