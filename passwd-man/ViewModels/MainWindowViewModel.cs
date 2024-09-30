using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;

namespace passwd_man.ViewModels
{
    public partial class MainWindowViewModel : ReactiveObject
    {
        public ObservableCollection<Item> Credentials { get; } = new ObservableCollection<Item>();

        // Commands for editing and deleting

        public ICommand GetUsernameCommand { get; }
        public ICommand GetPasswordCommand { get; }
        public ICommand GetLinkCommand { get; }

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainWindowViewModel()
        {
            Credentials.Add(new Item("meow", "GetUname"));
            // Define commands
        }

        public class Item
        {
            public string Name { get; set; }

            public Item(string name, string a1)
            {
                Name = name;
            }

            public async void GetLink()
            {
                var clip = Clipboard.Get();
                await clip.SetTextAsync("moew");
            }

            private void GetPassword()
            {
                throw new NotImplementedException();
            }

            private void GetUsername()
            {
                throw new NotImplementedException();
            }

            public void EditItem()
            {
                Console.WriteLine($"Editing: {Name}");
            }

        }
    }
}