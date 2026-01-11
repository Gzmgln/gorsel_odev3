using Newtonsoft.Json;
using Odev3_Uygulama.Model;

namespace Odev3_Uygulama.Services
{
    public class HaberService
    {
        private const string RssToJsonBaseUrl = "https://api.rss2json.com/v1/api.json?rss_url=";

        public async Task<List<HaberItem>> GetHaberler(string rssUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var json = await client.GetStringAsync(RssToJsonBaseUrl + rssUrl);
                    var root = JsonConvert.DeserializeObject<RootObject>(json);

                    // JSON'dan gelen veriyi kendi modelimize çeviriyoruz
                    var haberler = new List<HaberItem>();
                    if (root != null && root.items != null)
                    {
                        foreach (var item in root.items)
                        {
                            haberler.Add(new HaberItem
                            {
                                Title = item.title,
                                Link = item.link,
                                Description = item.description,
                                PubDate = item.pubDate,
                                ImageUrl = item.enclosure?.link ?? "dotnet_bot.png" // Resim yoksa varsayılan
                            });
                        }
                    }
                    return haberler;
                }
                catch
                {
                    return new List<HaberItem>();
                }
            }
        }

        // JSON Deserialization için yardımcı sınıflar (Sadece bu dosya içinde kullanılır)
        public class RootObject { public List<ItemJson> items { get; set; } }
        public class ItemJson
        {
            public string title { get; set; }
            public string link { get; set; }
            public string description { get; set; }
            public string pubDate { get; set; }
            public EnclosureJson enclosure { get; set; }
        }
        public class EnclosureJson { public string link { get; set; } }
    }
}