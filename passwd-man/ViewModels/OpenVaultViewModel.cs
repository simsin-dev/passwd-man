using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using DynamicData;
using ReactiveUI;

namespace passwd_man.ViewModels;

public partial class OpenVaultViewModel : ReactiveObject
{

    //built like this bc of ctrl+c & ctrl+v, with the excuse that someday functionality can be extended
    public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

    public OpenVaultViewModel()
    {
        string[] names = Config.ListVaultNames();
        Items.AddRange(names.Select(name => new Item{Name = name}).ToArray());
    }

    public class Item
    {
        public string Name {get; set;}
    }
}