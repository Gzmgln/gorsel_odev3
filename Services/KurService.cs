using Newtonsoft.Json.Linq;
using Odev3_Uygulama.Model;
using System.Diagnostics;
using System.Globalization;

namespace Odev3_Uygulama.Services;

public class KurService
{
    // 1. KAYNAK: Döviz ve Altınlar için 
    private const string TruncgilUrl = "https://finans.truncgil.com/today.json";

    // 2. KAYNAK: Kripto Paralar için (Yedek)
    private const string BinanceBtcUrl = "https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT";
    private const string BinanceEthUrl = "https://api.binance.com/api/v3/ticker/price?symbol=ETHUSDT";

    public async Task<List<KurItem>> GetKurlar()
    {
        // Önce Truncgil'den ana verileri çekilir
        var anaListe = await GetFromTruncgil();

        // Eğer Truncgil'den veri geldiyse ama içinde BTC yoksa, Binance'den tamamlanır
        if (anaListe.Count > 0)
        {
            // BTC kontrolü
            if (!anaListe.Any(x => x.Tur.Contains("BTC")))
            {
                var btc = await GetCryptoFromBinance("BTC");
                if (btc != null) anaListe.Add(btc);
            }

            // ETH kontrolü
            if (!anaListe.Any(x => x.Tur.Contains("ETH")))
            {
                var eth = await GetCryptoFromBinance("ETH");
                if (eth != null) anaListe.Add(eth);
            }
        }

        // BIST 100, Truncgil içinde bazen 'XU100' bazen 'bist-100' olarak gelir.
        

        return Sirala(anaListe);
    }

    // --- TRUNCGIL (Döviz + Altın + BIST) ---
    private async Task<List<KurItem>> GetFromTruncgil()
    {
        var list = new List<KurItem>();
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // SSL hatası yoksayma
                var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (m, c, ch, e) => true };
                using (var safeClient = new HttpClient(handler))
                {
                    safeClient.Timeout = TimeSpan.FromSeconds(10);
                    safeClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                    var json = await safeClient.GetStringAsync(TruncgilUrl);
                    var root = JObject.Parse(json);

                   
                    // Truncgil 'bitcoin' ve 'bist-100' anahtarlarını kullanır.
                    string[] hedefler = { "USD", "EUR", "GBP", "gram-altin", "ceyrek-altin", "gumus", "bist-100", "XU100" };

                    foreach (var prop in root.Properties())
                    {
                        // İsim eşleşiyor mu?
                        string key = prop.Name;
                        if (!hedefler.Contains(key) && key != "bitcoin" && key != "etherium") continue;

                        var val = prop.Value as JObject;
                        if (val == null) continue;

                        var alis = val["Alış"]?.ToString();
                        var satis = val["Satış"]?.ToString();
                        var degisim = val["Değişim"]?.ToString() ?? "0";

                        // BIST için özel durum: Alış boşsa Satış'ı kullan
                        if (string.IsNullOrEmpty(alis) && !string.IsNullOrEmpty(satis)) alis = satis;

                        if (string.IsNullOrEmpty(alis)) continue;

                        string tur = IsimDuzenle(key);
                        bool dustuMu = degisim.Contains("-");

                        list.Add(new KurItem
                        {
                            Tur = tur,
                            Alis = alis,
                            Satis = satis,
                            Degisim = degisim,
                            Fark = "%" + degisim.Replace("%", "").Trim(),
                            Yon = dustuMu ? "⬇" : "⬆",
                            Renk = dustuMu ? "Red" : "Green"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Truncgil Hatası: {ex.Message}");
            }
        }
        return list;
    }

    // --- BINANCE (Kripto Yedek) ---
    private async Task<KurItem> GetCryptoFromBinance(string symbol)
    {
        try
        {
            string url = symbol == "BTC" ? BinanceBtcUrl : BinanceEthUrl;

            using (HttpClient client = new HttpClient())
            {
                var json = await client.GetStringAsync(url);
                var obj = JObject.Parse(json);

                string price = obj["price"]?.ToString();
                if (price != null)
                {
                    // Binance sadece fiyat verir, küsürat kesilir
                    double fiyat = Convert.ToDouble(price, CultureInfo.InvariantCulture);
                    string fiyatStr = fiyat.ToString("N2"); // 2 ondalık basamak

                    return new KurItem
                    {
                        Tur = symbol == "BTC" ? "BTC (USD)" : "ETH (USD)",
                        Alis = fiyatStr,
                        Satis = fiyatStr, // Kripto borsasında alış/satış çok yakındır, aynı yazılabilir
                        Degisim = "0", // Anlık değişim verisi basit API'de yok
                        Fark = "%0.5", // Temsili değişim
                        Yon = "⬆",
                        Renk = "Green"
                    };
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Binance Hatası ({symbol}): {ex.Message}");
        }
        return null;
    }

    private string IsimDuzenle(string key)
    {
        if (key == "USD") return "Dolar";
        if (key == "EUR") return "Euro";
        if (key == "GBP") return "Sterlin";
        if (key == "gram-altin") return "Gram Altın";
        if (key == "ceyrek-altin") return "Çeyrek Altın";
        if (key == "gumus") return "Gümüş";
        if (key == "bitcoin") return "BTC (USD)";
        if (key == "etherium" || key == "ethereum") return "ETH (USD)";
        if (key == "bist-100" || key == "XU100") return "BIST 100";
        return key;
    }

    private List<KurItem> Sirala(List<KurItem> liste)
    {
        // Listeyi tekilleştirme (Aynı türden iki tane olmasın)
        var tekilListe = liste.GroupBy(x => x.Tur).Select(g => g.First()).ToList();

        string[] sira = { "Dolar", "Euro", "Sterlin", "Gram Altın", "Çeyrek Altın", "Gümüş", "BTC (USD)", "ETH (USD)", "BIST 100" };

        return tekilListe.OrderBy(x =>
        {
            int index = Array.IndexOf(sira, x.Tur);
            return index == -1 ? 99 : index;
        }).ToList();
    }
}