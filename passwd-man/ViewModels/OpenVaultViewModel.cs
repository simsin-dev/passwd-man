using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using DynamicData;
using passwd_man.Views;
using ReactiveUI;

namespace passwd_man.ViewModels;

public partial class OpenVaultViewModel : Window 
{

    public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();
    public string Password { get; set; }

    public OpenVaultViewModel()
    {
        string[] names = Config.ListVaultNames();
        foreach (var item in names)
        {
            if(File.Exists(Config.GetVaultPath(item)))
            {
                Items.Add(new Item{ Name = item, GetPasswd = () => Password});
            }
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public Func<string> GetPasswd { get; set; }

        public async void OpenVault()
        {
            if(!await VaultHandler.Open(GetPasswd(), Config.GetVaultPath(Name)))
            {
                return;
            }

            var mainWind = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

            mainWind.Show();
        }
    }
}