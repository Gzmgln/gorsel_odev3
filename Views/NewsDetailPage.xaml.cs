using Odev3_Uygulama.Model;

namespace Odev3_Uygulama.Views;

public partial class NewsDetailPage : ContentPage
{
    HaberItem _haber; // Seçilen haber verisini tutar

    public NewsDetailPage(HaberItem haber)
    {
        InitializeComponent();
        _haber = haber;

        // WebView aracýlýðýyla haberin linkini ekranda gösterir
        webView.Source = haber.Link;

        // Sayfa baþlýðýný haberin baþlýðý yapar
        Title = haber.Title;
    }

    // Sað üstteki "Paylaþ" butonuna basýlýnca çalýþýr
    private async void ShareClicked(object sender, EventArgs e)
    {
        // Telefonun paylaþým penceresini açarak haberin linkini paylaþýr
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Uri = _haber.Link,
            Title = _haber.Title
        });
    }
}