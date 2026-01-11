using Firebase.Database;
using Firebase.Database.Query;
using Odev3_Uygulama.Model;

namespace Odev3_Uygulama.Services
{
    public class TodoService
    {
        // Firebase URL
        private const string BaseUrl = "https://odev3-projesi-default-rtdb.firebaseio.com/";

        private readonly FirebaseClient client;

        public TodoService()
        {
            client = new FirebaseClient(BaseUrl);
        }

        // Listele
        public async Task<List<Gorev>> GetGorevler()
        {
            try
            {
                var items = await client.Child("Gorevler").OnceAsync<Gorev>();

                return items.Select(x => new Gorev
                {
                    Key = x.Key,
                    Baslik = x.Object.Baslik,
                    Detay = x.Object.Detay,
                    Tarih = x.Object.Tarih,
                    Saat = x.Object.Saat,
                    YapildiMi = x.Object.YapildiMi
                }).ToList();
            }
            catch
            {
                return new List<Gorev>();
            }
        }

        // Ekle
        public async Task AddGorev(Gorev gorev)
        {
            await client.Child("Gorevler").PostAsync(gorev);
        }

        // Sil
        public async Task DeleteGorev(string key)
        {
            await client.Child("Gorevler").Child(key).DeleteAsync();
        }

        // GÜNCELLEME
        public async Task UpdateGorev(Gorev gorev)
        {
            // Key'i kullanarak ilgili veriyi ezer (günceller)
            await client.Child("Gorevler").Child(gorev.Key).PutAsync(gorev);
        }
    }
}