using Odev3_Uygulama.Model;
using Odev3_Uygulama.Services;
using System.Collections.ObjectModel;

namespace Odev3_Uygulama.Views;

public partial class HaberlerPage : ContentPage
{
    HaberService _service = new HaberService();

    // En son seçilen kategoriyi hafýzada tutmak için deðiþken (Varsayýlan: Manþet)
    private NewsCategory _selectedCategory;

    public HaberlerPage()
    {
        InitializeComponent();
        lstCategory.ItemsSource = NewsCategories;

        // Uygulama ilk açýldýðýnda boþ kalmasýn, "Manþet" haberlerini otomatik yükle
        _selectedCategory = NewsCategories[0];
        HaberleriYukle();
    }

    // Kategoriler Listesi
    ObservableCollection<NewsCategory> NewsCategories => new()
    {
        new NewsCategory("Manþet", "https://www.trthaber.com/manset_articles.rss"),
        new NewsCategory("Son Dakika", "https://www.trthaber.com/sondakika_articles.rss"),
        new NewsCategory("Gündem", "https://www.trthaber.com/gundem_articles.rss"),
        new NewsCategory("Ekonomi", "https://www.trthaber.com/ekonomi_articles.rss"),
        new NewsCategory("Spor", "https://www.trthaber.com/spor_articles.rss"),
        new NewsCategory("Bilim Teknoloji", "https://www.trthaber.com/bilim_teknoloji_articles.rss")
    };

    // Kategori butonuna basýnca çalýþýr
    private void LoadRSSNews(object sender, EventArgs e)
    {
        var btn = sender as Button;
        var category = btn.CommandParameter as NewsCategory;

        // Seçilen kategoriyi güncelle
        _selectedCategory = category;

        // Haberleri çek
        HaberleriYukle();
    }

    // Sað üstteki YENÝLE butonuna basýnca çalýþýr
    private void OnRefreshClicked(object sender, EventArgs e)
    {
        // Mevcut seçili kategori neyse onu tekrar yükle
        HaberleriYukle();
    }

    // Ortak Haber Yükleme Fonksiyonu
    private async void HaberleriYukle()
    {
        if (_selectedCategory == null) return;

        // Kullanýcýya iþlem yapýldýðýný hissettirmek için listeyi anlýk temizle
        lstNews.ItemsSource = null;

        var news = await _service.GetHaberler(_selectedCategory.Url);
        lstNews.ItemsSource = news;
    }

    // Habere týklayýnca detaya git
    private async void OpenNewsDetail(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

        var news = e.CurrentSelection.FirstOrDefault() as HaberItem;
        lstNews.SelectedItem = null; // Seçimi temizle

        await Navigation.PushAsync(new NewsDetailPage(news));
    }
}