namespace Odev3_Uygulama.Model
{
    public class Gorev
    {
        public string Key { get; set; }      // Firebase ID'si (Silme/Güncelleme için lazım)
        public string Baslik { get; set; }   // Görev Başlığı
        public string Detay { get; set; }    // Görev Detayı
        public string Tarih { get; set; }    // Tarih
        public string Saat { get; set; }     // Saat
        public bool YapildiMi { get; set; }  // Tamamlandı mı?
    }
}