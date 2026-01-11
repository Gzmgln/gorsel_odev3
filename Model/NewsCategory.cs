namespace Odev3_Uygulama.Model
{
    public class NewsCategory
    {
        public string Title { get; set; } // Kategori Adı (Örn: Spor)
        public string Url { get; set; }   // RSS Linki

        public NewsCategory(string title, string url)
        {
            Title = title;
            Url = url;
        }
    }
}