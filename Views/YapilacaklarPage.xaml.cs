using Odev3_Uygulama.Model;
using Odev3_Uygulama.Services;

namespace Odev3_Uygulama.Views;

public partial class YapilacaklarPage : ContentPage
{
    TodoService _service = new TodoService();

    public YapilacaklarPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await VerileriGetir();
    }

    async Task VerileriGetir()
    {
        // Listeyi yenile
        var liste = await _service.GetGorevler();
        lstTodo.ItemsSource = null;
        lstTodo.ItemsSource = liste;
    }

    // EKLE BUTONU
    private async void OnEkleClicked(object sender, EventArgs e)
    {
        // Yeni sayfaya boþ gidiyoruz
        await Navigation.PushAsync(new GorevDetayPage());
    }

    // DÜZENLE BUTONU (Kalem)
    private async void OnDuzenleClicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        var gorev = btn.CommandParameter as Gorev;

        // Yeni sayfaya seçili görevle gidiyoruz
        await Navigation.PushAsync(new GorevDetayPage(gorev));
    }

    // SÝL BUTONU (X)
    private async void OnSilClicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        var gorev = btn.CommandParameter as Gorev;

        bool cevap = await DisplayAlert("Silinsin mi?", $"{gorev.Baslik} silinecek. Onaylýyor musunuz?", "Evet", "Hayýr");
        if (cevap)
        {
            await _service.DeleteGorev(gorev.Key);
            await VerileriGetir();
        }
    }

    // CHECKBOX ÝÞARETLENÝNCE (Otomatik Kaydet)
    private async void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var cb = sender as CheckBox;
        // BindingContext, o satýrdaki Gorev nesnesidir
        var gorev = cb.BindingContext as Gorev;

        if (gorev != null)
        {
            // Modeldeki durumu güncelle
            gorev.YapildiMi = e.Value;
            // Veritabanýna kaydet 
            await _service.UpdateGorev(gorev);
        }
    }
}