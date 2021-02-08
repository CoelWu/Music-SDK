using Music.SDK.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music.SDK.Models
{
    public class SongItem
    {
        /// <summary>
        /// 歌曲平台
        /// </summary>
        public PlatformType SongPlatform { get; set; }

        /// <summary>
        /// 歌曲Url链接
        /// </summary>
        public string SongUrl { get; set; }

        /// <summary>
        /// 歌曲ID
        /// </summary>
        public long SongId { get; set; }
        
        /// <summary>
        /// 歌曲MID
        /// </summary>
        public string SongMId { get; set; }

        /// <summary>
        /// 歌曲GID (通用ID)
        /// </summary>
        public string SongGId { get; set; }

        /// <summary>
        /// 歌曲文件Hash
        /// </summary>
        public string SongFileHash { get; set; }

        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string SongName { get; set; }

        /// <summary>
        /// 歌曲艺术家ID
        /// </summary>
        public List<long> SongArtistId { get; set; } = new List<long>();

        /// <summary>
        /// 歌曲艺术家MID
        /// </summary>
        public List<string> SongArtistMId { get; set; } = new List<string>();

        /// <summary>
        /// 歌曲艺术家名称
        /// </summary>
        public List<string> SongArtistName { get; set; } = new List<string>();

        /// <summary>
        /// 歌曲专辑ID
        /// </summary>
        public long SongAlbumId { get; set; }

        /// <summary>
        /// 歌曲专辑MID
        /// </summary>
        public string SongAlbumMId { get; set; }

        /// <summary>
        /// 歌曲专辑名称
        /// </summary>
        public string SongAlbumName { get; set; }

        /// <summary>
        /// 歌曲图像Url
        /// </summary>
        public string SongImageUrl { get; set; }

        /// <summary>
        /// 歌曲歌词Url
        /// </summary>
        public string SongLyricUrl { get; set; }
    }
}
