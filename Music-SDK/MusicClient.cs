using Music.SDK.Models;
using Music.SDK.Models.Enums;
using Music.SDK.Provider;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Music_SDK
{
    public class MusicClient
    {
        internal QQMusic qqMusic;
        internal NeteaseMusic neteaseMusic;
        internal KuWoMusic kuWoMusic;
        internal KuGouMusic kuGouMusic;
        internal BiliBiliMusic biliMusic;
        internal QianQianMusic qianMusic;

        public MusicClient()
        {
            qqMusic = new QQMusic();
            neteaseMusic = new NeteaseMusic();
            kuWoMusic = new KuWoMusic();
            kuGouMusic = new KuGouMusic();
            biliMusic = new BiliBiliMusic();
            qianMusic = new QianQianMusic();
        }

        public async Task<List<SongItem>> SearchSong(PlatformType platform, string name)
        {
            switch (platform)
            {
                case PlatformType.QQMusic:
                    return await qqMusic.SearchSong(name);
                case PlatformType.NeteaseMusic:
                    return await neteaseMusic.SearchSong(name);
                case PlatformType.KuWoMusic:
                    return await kuWoMusic.SearchSong(name);
                case PlatformType.KuGouMusic:
                    return await kuGouMusic.SearchSong(name);
                case PlatformType.BiliBiliMusic:
                    return await biliMusic.SearchSong(name);
                case PlatformType.QianQianMusic:
                    return await qianMusic.SearchSong(name);
                default:
                    return null;
            }
        }

        public async Task<PlayListItem> GetPlayList(PlatformType platform, long id)
        {
            switch (platform)
            {
                case PlatformType.QQMusic:
                    return await qqMusic.GetPlayList(id);
                case PlatformType.NeteaseMusic:
                    return await neteaseMusic.GetPlayList(id);
                case PlatformType.KuWoMusic:
                    return await kuWoMusic.GetPlayList(id);
                case PlatformType.KuGouMusic:
                    return await kuGouMusic.GetPlayList(id);
                case PlatformType.BiliBiliMusic:
                    return await biliMusic.GetPlayList(id);
                case PlatformType.QianQianMusic:
                    return await qianMusic.GetPlayList(id);
                default:
                    return null;
            }
        }

        public async Task<LyricItem> GetLyric(PlatformType platform, SongItem song)
        {
            switch (platform)
            {
                case PlatformType.QQMusic:
                    return await qqMusic.GetLyric(song);
                case PlatformType.NeteaseMusic:
                    return await neteaseMusic.GetLyric(song);
                case PlatformType.KuWoMusic:
                    return await kuWoMusic.GetLyric(song);
                case PlatformType.KuGouMusic:
                    return await kuGouMusic.GetLyric(song);
                case PlatformType.BiliBiliMusic:
                    return await biliMusic.GetLyric(song);
                case PlatformType.QianQianMusic:
                    return await qianMusic.GetLyric(song);
                default:
                    return null;
            }
        }

        public async Task<LyricItem> GetLyricById(PlatformType platform, string id, string albumId = "")
        {
            switch (platform)
            {
                case PlatformType.QQMusic:
                    return await qqMusic.GetLyricById(id);
                case PlatformType.NeteaseMusic:
                    return await neteaseMusic.GetLyricById(id);
                case PlatformType.KuWoMusic:
                    return await kuWoMusic.GetLyricById(id);
                case PlatformType.KuGouMusic:
                    return await kuGouMusic.GetLyricById(id, albumId);
                case PlatformType.BiliBiliMusic:
                    return await biliMusic.GetLyricById(id);
                case PlatformType.QianQianMusic:
                    return await qianMusic.GetLyricById(id);
                default:
                    return null;
            }
        }

        public async Task<string> GetSongPlayUrl(PlatformType platform, SongItem song)
        {
            switch (platform)
            {
                case PlatformType.QQMusic:
                    return await qqMusic.GetSongPlayUrl(song);
                case PlatformType.NeteaseMusic:
                    return await neteaseMusic.GetSongPlayUrl(song);
                case PlatformType.KuWoMusic:
                    return await kuWoMusic.GetSongPlayUrl(song);
                case PlatformType.KuGouMusic:
                    return await kuGouMusic.GetSongPlayUrl(song);
                case PlatformType.BiliBiliMusic:
                    return await biliMusic.GetSongPlayUrl(song);
                case PlatformType.QianQianMusic:
                    return await qianMusic.GetSongPlayUrl(song);
                default:
                    return null;
            }
        }
    }
}
