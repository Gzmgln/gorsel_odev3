namespace Odev3_Uygulama;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Uygulama her zaman AÇIK TEMA (Light) ile başlasın.
        Application.Current.UserAppTheme = AppTheme.Light;

        // Başlangıç Sayfası
        MainPage = new Views.LoginPage();
    }
}