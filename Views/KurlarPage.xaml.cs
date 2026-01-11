using Odev3_Uygulama.Services;

namespace Odev3_Uygulama.Views;

public partial class KurlarPage : ContentPage
{
    KurService _service = new KurService();

    public KurlarPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await VerileriGetir();
    }

    // Sað üstteki Yenile butonuna basýnca çalýþýr
    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await VerileriGetir();
    }

    private async Task VerileriGetir()
    {
        loading.IsRunning = true; // Dönme animasyonu baþla
        lstKurlar.ItemsSource = null; // Listeyi boþalt

        // Ýnternet gecikmesini simüle etmek veya UI'ýn donmasýný engellemek için ufak bekleme
        await Task.Delay(500);

        var kurlar = await _service.GetKurlar();
        lstKurlar.ItemsSource = kurlar;

        loading.IsRunning = false; // Dönme animasyonu dur
    }
}