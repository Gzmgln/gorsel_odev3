using Newtonsoft.Json;
using Odev3_Uygulama.Model;

namespace Odev3_Uygulama.Views;

public partial class HavaDurumuPage : ContentPage
{
    List<HavaDurumu> Sehirler = new List<HavaDurumu>();
    // Yerel depolama yolu
    string dosyaYolu = Path.Combine(FileSystem.AppDataDirectory, "sehirler.json");

    public HavaDurumuPage()
    {
        InitializeComponent();
        Yukle(); // Uygulama açýlýnca kayýtlýlarý getir
    }

    void Yukle()
    {
        if (File.Exists(dosyaYolu))
        {
            try
            {
                var json = File.ReadAllText(dosyaYolu);
                Sehirler = JsonConvert.DeserializeObject<List<HavaDurumu>>(json) ?? new List<HavaDurumu>();
                ListeGuncelle();
            }
            catch
            {
                // Json bozuksa listeyi sýfýrla
                Sehirler = new List<HavaDurumu>();
            }
        }
    }

    void Kaydet()
    {
        var json = JsonConvert.SerializeObject(Sehirler);
        File.WriteAllText(dosyaYolu, json);
    }

    void ListeGuncelle()
    {
        lstHava.ItemsSource = null;
        lstHava.ItemsSource = Sehirler;
    }

    private void OnEkleClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtSehir.Text))
        {
            // Yeni þehri listeye ekle
            Sehirler.Add(new HavaDurumu { SehirAd = txtSehir.Text.Trim() });
            Kaydet();
            ListeGuncelle();
            txtSehir.Text = ""; // Kutuyu temizle
        }
    }

    private void OnYenileClicked(object sender, EventArgs e)
    {
        // Sayfayý yenileme (WebView'leri reload yapar)
        ListeGuncelle();
    }

    // YENÝ EKLENEN SÝLME METODU
    private async void OnSilClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var silinecekSehir = button.CommandParameter as HavaDurumu;

        if (silinecekSehir != null)
        {
            // Kullanýcýya sorar: Emin misin?
            bool cevap = await DisplayAlert("Sil", $"{silinecekSehir.SehirAd} þehrini silmek istiyor musunuz?", "Evet", "Hayýr");

            if (cevap)
            {
                Sehirler.Remove(silinecekSehir); // Listeden sil
                Kaydet(); // Json'ý güncelle
                ListeGuncelle(); // Ekraný güncelle
            }
        }
    }
}