using Firebase.Auth;
using Newtonsoft.Json;

namespace Odev3_Uygulama.Services
{
    public class AuthService
    {
        private const string ApiKey = "AIzaSyCTuVqAHGpoDQHNtqrsxswWf2SfJASGTd0";

        public async Task<string> Login(string email, string password)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                return auth.FirebaseToken;
            }
            catch
            {
                return "";
            }
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                // Firebase'e kullanıcı oluşturma isteği
                await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (Exception ex)
            {
                // Hata mesajını konsola yazdırma (Debug için)
                System.Diagnostics.Debug.WriteLine("Kayıt Hatası: " + ex.Message);
                return false;
            }
        }
    }
}