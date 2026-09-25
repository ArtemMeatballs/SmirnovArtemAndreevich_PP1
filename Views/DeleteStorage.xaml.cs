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

namespace WoodAccountingSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для DeleteStorage.xaml
    /// </summary>
    public partial class DeleteStorage : Page
    {
        public DeleteStorage()
        {
            InitializeComponent();

            var storage = App.Context.Storage.ToList();

            for (int i = 0; i < storage.Count; i++)
            {
                CBnumber.Items.Add("Склад " + storage[i].id_Storage);
            }
            CBnumber.SelectedIndex = 0;
        }

        private void Click_btn_back(object sender, RoutedEventArgs e)
        {
           NavigationService.GoBack();
        }

        private void Click_btn_delete(object sender, RoutedEventArgs e)
        { 
            int idDeleteStorage = int.Parse(CBnumber.Text.Replace("Склад ", ""));

            var infoStorage = App.Context.Wood_in_Storage.Where(A => A.id_Storage == idDeleteStorage).ToList();

            if (infoStorage.Count != 0)
            {
                MessageBox.Show("Склад нельзя удалить, так как на нем находится древесина!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var deleteStorage = App.Context.Storage.First(A => A.id_Storage == idDeleteStorage);
            App.Context.Storage.Remove(deleteStorage);
            App.Context.SaveChanges();
            MessageBox.Show("Склад успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.Navigate(new ListStorage());
        }
    }
}
