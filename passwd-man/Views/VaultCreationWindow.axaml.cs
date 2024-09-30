using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using Avalonia.Logging;
using System.Diagnostics;
using System.Linq;
using Avalonia.Interactivity;
using System.IO;
using Avalonia.Media;
using passwd_man.ViewModels;

namespace passwd_man.Views;

public partial class VaultCreationWindow : Window
{
    public VaultCreationWindow()
    {
        InitializeComponent();
    }

    private async void PressedHandler(object? sender, RoutedEventArgs e)
    {
        var pickerOptions = new Avalonia.Platform.Storage.FolderPickerOpenOptions();
        pickerOptions.AllowMultiple = false;
        pickerOptions.Title = "Pick vault location";

        try
        {
            var dialog = await StorageProvider.OpenFolderPickerAsync(pickerOptions);


            if (dialog != null)
            {
                FolderPathTextBox.Text = Path.Combine(dialog.First().Path.AbsolutePath, VaultName.Text + ".vault");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in file picking");
        }
    }

    private async void CreateVault(object? sender, RoutedEventArgs e)
    {
        bool shouldReturn = false;

        if (VaultName.Text == null)
        {
            VaultName.Background = new SolidColorBrush(Colors.Red);
            shouldReturn = true;
        }
        else
        {
            VaultName.Background = new SolidColorBrush(Colors.Black);
        }

        if (FolderPathTextBox.Text == null)
        {
            FolderPathTextBox.Background = new SolidColorBrush(Colors.Red);
            shouldReturn = true;
        }
        else
        {
            FolderPathTextBox.Background = new SolidColorBrush(Colors.Black);
        }

        if (Password.Text == null || Password.Text.Length < 10)
        {
            Password.Background = new SolidColorBrush(Colors.Red);
            shouldReturn = true;
        }
        else
        {
            Password.Background = new SolidColorBrush(Colors.Black);
        }

        if (PasswordCheck.Text == null || PasswordCheck.Text.Length < 10)
        {
            PasswordCheck.Background = new SolidColorBrush(Colors.Red);
            shouldReturn = true;
        }
        else
        {
            PasswordCheck.Background = new SolidColorBrush(Colors.Black);
        }

        if (Password.Text != PasswordCheck.Text)
        {
            PasswordCheck.Background = new SolidColorBrush(Colors.Red);
            Password.Background = new SolidColorBrush(Colors.Red);
            shouldReturn = true;
        }

        if (shouldReturn)
        { return; }

        try
        {
            File.Create(FolderPathTextBox.Text).Close();

            IsOk.Text = "";
        }
        catch (Exception ex)
        {
            IsOk.Foreground = new SolidColorBrush(Colors.Red);
            IsOk.Text = "Incorrect file path!!";
            FolderPathTextBox.Background = new SolidColorBrush(Colors.Red);

            return;
        }

        Config.AddVault(VaultName.Text, FolderPathTextBox.Text);

        VaultHandler.CreateVault(FolderPathTextBox.Text, PasswordCheck.Text);

        var mainWind = new MainWindow{
            DataContext = new MainWindowViewModel()
        };

        mainWind.Show();

        this.Close();
    }
}