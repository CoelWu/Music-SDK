<p align="center">
    <img src="https://i.loli.net/2021/02/09/R56uAvenEaFthbO.png" align="center" height="80"/>
</p>

<div align="center">

# 国内音乐平台 .NET SDK

[![Nuget](https://img.shields.io/nuget/v/CoelWu.Music.SDK)](https://www.nuget.org/packages/CoelWu.Music.SDK/)

该SDK在.NET Standard 2.0上构建

</div>

## 简单的开始

使用前，你需要先安装 **CoelWu.Music.SDK** nuget包

```csharp
// 创建Client
MusicClient _client = new MusicClient();

// 搜索歌曲
List<SongItem> songItems = await _client.SearchSong(PlatformType.QQMusic, "星晨大海");

// 获取播放url
string url = await _client.GetSongPlayUrl(PlatformType.QQMusic, songItems[0]);

// 获取歌词
LyricItem lyricItem = await _client.GetLyric(PlatformType.QQMusic, songItems[0]);
```

## 更多例子

如需要更多使用案例，请参考 **MusicSDK-ConsoleApp**

## 版权说明

“QQ”、“QQ音乐”及企鹅形象等文字、图形和商业标识，其著作权或商标权归腾讯公司所有。
QQ音乐享有对其平台授权音乐的版权，请勿随意下载，复制版权内容。具体内容请参考QQ音乐用户协议。

“网易云”、“网易云音乐”等文字、图形和商业标识，其著作权或商标权归网易公司所有。
网易云音乐享有对其平台授权音乐的版权，请勿随意下载，复制版权内容。具体内容请参考网易云音乐用户协议。

“酷我”、“酷我音乐”等文字、图形和商业标识，其著作权或商标权归北京酷我科技有限公司所有。
酷我音乐享有对其平台授权音乐的版权，请勿随意下载，复制版权内容。具体内容请参考酷我音乐用户协议。

“酷狗”、“酷狗音乐”等文字、图形和商业标识，其著作权或商标权归腾讯音乐娱乐（广州）有限公司所有。
酷狗音乐享有对其平台授权音乐的版权，请勿随意下载，复制版权内容。具体内容请参考酷狗音乐用户协议。

“哔哩哔哩”、“哔哩哔哩音乐”等文字、图形和商业标识，其著作权或商标权归上海宽娱数码科技有限公司所有。
哔哩哔哩音乐享有对其平台授权音乐的版权，请勿随意下载，复制版权内容。具体内容请参考哔哩哔哩音乐用户协议。

“千千”、“千千音乐”等文字、图形和商业标识，其著作权或商标权归北京太合音乐文化发展有限公司所有。
千千音乐享有对其平台授权音乐的版权，请勿随意下载，复制版权内容。具体内容请参考千千音乐用户协议。

## 图标版权

图标来自于Freepik.

- [Freepik](https://www.freepik.com)

## 鸣谢

- [listen1-api](https://github.com/listen1/listen1-api)