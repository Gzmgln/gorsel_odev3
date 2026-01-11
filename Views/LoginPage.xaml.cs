using Odev3_Uygulama.Services;

namespace Odev3_Uygulama.Views;

public partial class LoginPage : ContentPage
{
    AuthService _authService = new AuthService();

    public LoginPage()
    {
        InitializeComponent();
    }

    // "Hesabým Yok" yazýsýna basýnca Kayýt ekranýný açar
    private void OnSwitchToRegister(object sender, EventArgs e)
    {
        LoginView.IsVisible = false;
        RegisterView.IsVisible = true;
    }

    // "Zaten bir hesabým var" yazýsýna basýnca Giriþ ekranýný açar
    private void OnSwitchToLogin(object sender, EventArgs e)
    {
        RegisterView.IsVisible = false;
        LoginView.IsVisible = true;
    }

    // Giriþ Yap Butonu
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EntEmailLogin.Text;
        string password = EntPasswordLogin.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Hata", "Lütfen tüm alanlarý doldurun.", "Tamam");
            return;
        }

        string token = await _authService.Login(email, password);

        if (!string.IsNullOrEmpty(token))
        {
            // Giriþ Baþarýlý -> Ana Menüye Git
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Hata", "Giriþ baþarýsýz. Email veya þifre hatalý.", "Tamam");
        }
    }

    // Kayýt Ol Butonu
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string email = EntEmailRegister.Text;
        string password = EntPasswordRegister.Text;

        // Validasyonlar
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Hata", "Lütfen email ve þifre giriniz.", "Tamam");
            return;
        }

        if (password.Length < 6)
        {
            await DisplayAlert("Uyarý", "Þifre en az 6 karakter olmalýdýr!", "Tamam");
            return;
        }

        // Servise gönder
        bool isCreated = await _authService.RegisterUser(email, password);

        if (isCreated)
        {
            await DisplayAlert("Baþarýlý", "Kayýt oluþturuldu! Þimdi giriþ yapabilirsiniz.", "Tamam");
            // Otomatik olarak giriþ ekranýna dön
            RegisterView.IsVisible = false;
            LoginView.IsVisible = true;
            // Kolaylýk olsun diye email'i oraya taþý
            EntEmailLogin.Text = email;
        }
        else
        {
            await DisplayAlert("Hata", "Kayýt yapýlamadý. Email formatýný kontrol edin veya daha önce alýnmýþ olabilir.", "Tamam");
        }
    }
}