namespace Odev3_Uygulama.Model
{
    public class HaberItem
    {
        public string Title { get; set; }        // Haber Başlığı
        public string Link { get; set; }         // Haberin web linki
        public string Description { get; set; }  // Kısa açıklama
        public string PubDate { get; set; }      // Tarih
        public string ImageUrl { get; set; }     // Haber görseli
    }
}