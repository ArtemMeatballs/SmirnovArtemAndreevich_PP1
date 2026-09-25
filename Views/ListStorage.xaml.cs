using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WoodAccountingSystem.Entities;

namespace WoodAccountingSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для ListStorage.xaml
    /// </summary>
    public partial class ListStorage : Page
    {
        public ListStorage()
        {
            InitializeComponent();

            var storage = App.Context.Storage.ToList();

            if (storage.Count != 0)
            {
                TBstorageNotFound.Visibility = Visibility.Collapsed;

                for (int i = 0; i < storage.Count; i++)
                {
                    Button btn = new Button();
                    btn.Width = 150;
                    btn.Height = 50;
                    btn.Background = new SolidColorBrush(Colors.Brown);
                    btn.Foreground = new SolidColorBrush(Colors.White);
                    btn.FontSize = 18;
                    btn.Margin = new Thickness(0, 15, 0, 0);
                    btn.Content = "Склад " + storage[i].id_Storage;
                    btn.Tag = storage[i].id_Storage;
                    btn.Click += Click_btn_storage;
                    SPlistStorage.Children.Add(btn);
                }
            }
        }

        private void Click_btn_storage(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int idCurrentStorage = (int)btn.Tag;

            NavigationService.Navigate(new Storage(idCurrentStorage));
        }

        private void Click_btn_add(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddStorage());
        }

        private void Click_btn_delete(object sender, RoutedEventArgs e)
        {
            var storage = App.Context.Storage.ToList();

            if (storage.Count != 0)
                NavigationService.Navigate(new DeleteStorage());
            else
                MessageBox.Show($"Складов не обнаружено", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
