using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using DynamicData;
using passwd_man.Views;
using ReactiveUI;

namespace passwd_man.ViewModels
{
    public partial class MainWindowViewModel : ReactiveObject
    {
        public ObservableCollection<Item> Credentials { get; } = new ObservableCollection<Item>();

        public MainWindowViewModel()
        {
            UpdateList();

            Updater();
        }

        private async Task Updater()
        {
            while(true)
            {
                await Task.Delay(500);
                UpdateList();
            }
        }

        public async void AddCredentials()
        {
            var addCredsWindow = new AddCredentialsWindow();

            addCredsWindow.Show();

            addCredsWindow.Closed += AddedCreds;
        }

        public async void GenCredentials()
        {
            var genCredsWindow = new GenerateCredentialsWindow();

            genCredsWindow.Show();

            genCredsWindow.Closed += AddedCreds;
        }

        private void AddedCreds(object? sender, EventArgs e)
        {
            UpdateList();
        }

        void UpdateList()
        {
            Credentials.Clear();
            string[] names = VaultHandler.ListCreds();
            Credentials.AddRange(names.Select(name => new Item { Name = name }).ToArray());

            //Console.WriteLine(Credentials.Count);
        }

        public class Item
        {
            public string Name { get; set; }
            public async void GetLink()
            {
                var clip = Clipboard.Get();
                await clip.SetTextAsync(VaultHandler.GetLink(Name));
            }

            public async void GetPassword()
            {
                var clip = Clipboard.Get();
                await clip.SetTextAsync(VaultHandler.GetPassword(Name));
            }

            public async void GetUsername()
            {
                var clip = Clipboard.Get();
                await clip.SetTextAsync(VaultHandler.GetUsername(Name));
            }

            public void EditItem()
            {
                VaultHandler.CredentialsSetToEdit = Name;

                var addCredsWindow = new AddCredentialsWindow();

                addCredsWindow.Show();
            }

            public void RemoveItem()
            {
                VaultHandler.RemoveCreds(Name);
            }
        }
    }
}