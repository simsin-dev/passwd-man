using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace passwd_man.Views;

public partial class GenerateCredentialsWindow : Window
{
    bool shouldReturn = true;

    public GenerateCredentialsWindow()
    {
        InitializeComponent();
        CharAmmount.ValueChanged += GeneratePass;
        CharAmmount.Value = 8;
        GenPass();

        Password.CopyingToClipboard += PassClicked;
    }

    private void PassClicked(object? sender, RoutedEventArgs e)
    {
        shouldReturn = false;
    }

    private void GeneratePass(object? sender, RangeBaseValueChangedEventArgs e)
    {
        GenPass();
    }

    void GenPass()
    {
        Password.Text = PasswdGen.Generate((int)CharAmmount.Value);
        CharAmmountText.Text = "Length: " + (int)CharAmmount.Value;
    }

    public async void CopyPassword(object? sender, RoutedEventArgs e)
    {

        await Clipboard.SetTextAsync(Password.Text);
        shouldReturn = false;
    }

    public async void AddCredsToVault(object? sender, RoutedEventArgs e)
    {

        if (string.IsNullOrEmpty(CredName.Text) || VaultHandler.ListCreds().Contains(CredName.Text))
        {
            CredName.Background = new SolidColorBrush(Colors.Red);
            return;
        }
        else
        {
            CredName.Background = new SolidColorBrush(Colors.Black);
        }

        if (shouldReturn)
        {
            Password.Background = new SolidColorBrush(Colors.Red);
            return;
        }

        VaultHandler.AddCreds(CredName.Text, Password.Text, LinkBox.Text, UsernameBox.Text);

        this.Close();
    }
}