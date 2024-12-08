using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using passwd_man.ViewModels;

namespace passwd_man.Views;

public partial class OpenVaultWindow : Window
{
    public OpenVaultWindow()
    {
        InitializeComponent();
    }

    public async void CreateVault(object? sender, RoutedEventArgs e)
    {
        var mainWind = new VaultCreationWindow
        {
            DataContext = new VaultCreationViewModel()
        };

        mainWind.Show();
        this.Close();
    }
}