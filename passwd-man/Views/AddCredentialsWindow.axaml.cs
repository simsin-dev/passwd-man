using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Splat.ApplicationPerformanceMonitoring;

namespace passwd_man.Views
{
    public partial class AddCredentialsWindow : Window
    {
        public AddCredentialsWindow()
        {
            InitializeComponent();
            var credName = VaultHandler.CredentialsSetToEdit;
            if(credName == null) return;

            CredName.Text = credName;
            CredName.IsReadOnly = true;
            PasswdBox.Text = VaultHandler.GetPassword(credName);
            UsernameBox.Text = VaultHandler.GetUsername(credName);
            Link.Text = VaultHandler.GetLink(credName);
        }

        public async void AddCredentials(object? sender, RoutedEventArgs e)
        {
            bool shouldReturn = false;

            if (string.IsNullOrEmpty(CredName.Text) || (VaultHandler.ListCreds().Contains(CredName.Text) && VaultHandler.CredentialsSetToEdit == null))
            {
                CredName.Background = new SolidColorBrush(Colors.Red);
                shouldReturn = true;
            }
            else
            {
                CredName.Background = new SolidColorBrush(Colors.Black);
            }

            if (string.IsNullOrEmpty(PasswdBox.Text))
            {
                PasswdBox.Background = new SolidColorBrush(Colors.Red); 
                shouldReturn = true;
            }
            else
            {
                PasswdBox.Background = new SolidColorBrush(Colors.Black);
            }
            
            if(shouldReturn)
            {
                return;
            }

            VaultHandler.AddCreds(CredName.Text, PasswdBox.Text, Link.Text, UsernameBox.Text);

            VaultHandler.CredentialsSetToEdit = null;

            this.Close(); 
        }
    }
}
