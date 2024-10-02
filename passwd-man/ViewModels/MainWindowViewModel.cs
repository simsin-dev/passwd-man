using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using DynamicData;
using ReactiveUI;

namespace passwd_man.ViewModels
{
    public partial class MainWindowViewModel : ReactiveObject
    {
        public ObservableCollection<Item> Credentials { get; } = new ObservableCollection<Item>();

        public MainWindowViewModel()
        {
            string[] names = VaultHandler.ListCreds();
            Credentials.AddRange(names.Select(name => new Item { Name = name }).ToArray());
        }

        public class Item
        {
            public string Name { get; set; }
            public async void GetLink()
            {
                var clip = Clipboard.Get();
                await clip.SetTextAsync(VaultHandler.GetLink(Name));
            }

            private async void GetPassword()
            {
                var clip = Clipboard.Get();
                await clip.SetTextAsync(VaultHandler.GetPassword(Name));
            }

            private async void GetUsername()
            {
                var clip = Clipboard.Get();
                await clip.SetTextAsync(VaultHandler.GetUsername(Name));
            }

            public void EditItem()
            {
                Console.WriteLine($"Editing: {Name}");
            }

        }
    }
}