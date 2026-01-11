namespace Odev3_Uygulama.Model
{
    public class KurItem
    {
        public string Tur { get; set; }      // Dolar, Euro vb.
        public string Alis { get; set; }     // Alış Fiyatı
        public string Satis { get; set; }    // Satış Fiyatı
        public string Degisim { get; set; }  // Değişim Oranı
        public string Fark { get; set; }     // Fark
        public string Yon { get; set; }      // Ok İşareti (↑ veya ↓)
        public string Renk { get; set; }     // Yazı Rengi (Green veya Red)
    }
}