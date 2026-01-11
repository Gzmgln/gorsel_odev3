namespace Odev3_Uygulama.Views;

public partial class AyarlarPage : ContentPage
{
    public AyarlarPage()
    {
        InitializeComponent();
    }

    // Sayfa her ekrana geldiðinde çalýþýr
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Mevcut tema Dark ise Switch AÇIK (True) olsun, deðilse KAPALI (False) olsun.
        if (Application.Current.UserAppTheme == AppTheme.Dark)
        {
            ThemeSwitch.IsToggled = true;
        }
        else
        {
            ThemeSwitch.IsToggled = false;
        }
    }

    private void OnThemeSwitchToggled(object sender, ToggledEventArgs e)
    {
        // Switch AÇIK ise (True) -> Koyu Tema
        // Switch KAPALI ise (False) -> Açýk Tema
        if (e.Value)
            Application.Current.UserAppTheme = AppTheme.Dark;
        else
            Application.Current.UserAppTheme = AppTheme.Light;
    }
}