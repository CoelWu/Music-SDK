using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Music.SDK.Tools
{
    public class HeaderHacker
    {
        public HeaderHacker()
        {

        }

        public HttpClient ApplyHeader(string url, HttpClient httpClient)
        {
            if (url.IndexOf("://music.163.com/") != -1)
            {
                httpClient = AddReferer("http://music.163.com/", httpClient);
                httpClient = AddCookie("appver=1.5.0.75771", httpClient);
            }

            if (url.IndexOf("c.y.qq.com") != -1)
            {
                httpClient = AddRefererAndOrigin("https://y.qq.com", httpClient);
            }

            if (url.IndexOf("i.y.qq.com/") != -1 || url.IndexOf("qqmusic.qq.com/") != -1 || url.IndexOf("music.qq.com/") != -1 || url.IndexOf("imgcache.qq.com/") != -1)
            {
                httpClient = AddRefererAndOrigin("https://y.qq.com/", httpClient);
            }

            if (url.IndexOf(".kugou.com/") != -1)
            {
                httpClient = AddRefererAndOrigin("https://www.kugou.com/", httpClient);
                httpClient = AddCookie("kg_mid=c4ca4238a0b923820dcc509a6f75849b;", httpClient);
            }

            if (url.IndexOf(".kuwo.cn/") != -1)
            {
                httpClient = AddRefererAndOrigin("https://www.kuwo.cn/", httpClient);
            }

            if (url.IndexOf(".bilibili.com/") != -1)
            {
                httpClient = AddReferer("https://www.bilibili.com/", httpClient);
            }

            if (url.IndexOf(".migu.cn/") != -1)
            {
                httpClient = AddRefererAndOrigin("https://music.migu.cn/", httpClient);
            }

            return httpClient;
        }

        public HttpClient AddCookie(string cookie, HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Remove("Cookie");
            httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
            return httpClient;
        }

        public HttpClient AddReferer(string url, HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Remove("Referer");
            httpClient.DefaultRequestHeaders.Add("Referer", url);
            return httpClient;
        }

        public HttpClient AddRefererAndOrigin(string url, HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Remove("Referer");
            httpClient.DefaultRequestHeaders.Remove("Origin");
            httpClient.DefaultRequestHeaders.Add("Referer", url);
            httpClient.DefaultRequestHeaders.Add("Origin", url);
            return httpClient;
        }
    }
}
