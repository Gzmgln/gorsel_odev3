namespace Odev3_Uygulama.Model
{
    public class HavaDurumu
    {
        public string SehirAd { get; set; }

        // MGM sitesinden widget almak için gereken URL
        public string ResimUrl => $"https://www.mgm.gov.tr/sunum/tahmin-klasik-5070.aspx?m={Duzenle(SehirAd)}&basla=1&bitir=5&rC=111&rZ=fff";

        //Türkçe karakterleri İngilizceye çeviren metot
        private string Duzenle(string sehir)
        {
            if (string.IsNullOrEmpty(sehir)) return "";

            return sehir.ToUpper()
                .Replace("Ç", "C")
                .Replace("Ğ", "G")
                .Replace("İ", "I")
                .Replace("Ö", "O")
                .Replace("Ş", "S")
                .Replace("Ü", "U");
        }
    }
}