using Odev3_Uygulama.Model;
using Odev3_Uygulama.Services;

namespace Odev3_Uygulama.Views;

public partial class GorevDetayPage : ContentPage
{
    TodoService _service = new TodoService();
    Gorev _mevcutGorev; // Eðer düzenleme yapýlýyorsa bu dolu olacak

    // Constructor (Yapýcý Metod)
    public GorevDetayPage(Gorev gorev = null)
    {
        InitializeComponent();
        _mevcutGorev = gorev;

        if (_mevcutGorev != null)
        {
            // DÜZENLEME MODU: Verileri kutucuklara doldur
            Title = "Görevi Düzenle";
            txtBaslik.Text = _mevcutGorev.Baslik;
            txtDetay.Text = _mevcutGorev.Detay;

            // Tarih ve Saati string'den çevirip Picker'lara ata
            if (DateTime.TryParse(_mevcutGorev.Tarih, out DateTime tarih))
                dpTarih.Date = tarih;

            if (TimeSpan.TryParse(_mevcutGorev.Saat, out TimeSpan saat))
                tpSaat.Time = saat;
        }
        else
        {
            // EKLEME MODU
            Title = "Yeni Görev Ekle";
        }
    }

    private async void OnKaydetClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtBaslik.Text))
        {
            await DisplayAlert("Uyarý", "Baþlýk boþ olamaz!", "Tamam");
            return;
        }

        if (_mevcutGorev == null)
        {
            // YENÝ KAYIT
            var yeniGorev = new Gorev
            {
                Baslik = txtBaslik.Text,
                Detay = txtDetay.Text,
                Tarih = dpTarih.Date.ToShortDateString(),
                Saat = tpSaat.Time.ToString(@"hh\:mm"),
                YapildiMi = false
            };
            await _service.AddGorev(yeniGorev);
        }
        else
        {
            // GÜNCELLEME
            _mevcutGorev.Baslik = txtBaslik.Text;
            _mevcutGorev.Detay = txtDetay.Text;
            _mevcutGorev.Tarih = dpTarih.Date.ToShortDateString();
            _mevcutGorev.Saat = tpSaat.Time.ToString(@"hh\:mm");

            await _service.UpdateGorev(_mevcutGorev);
        }

        // Listeye geri dön
        await Navigation.PopAsync();
    }

    private async void OnIptalClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}