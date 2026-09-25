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
using WoodAccountingSystem.Infrastructure;

namespace WoodAccountingSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для AddStorage.xaml
    /// </summary>
    public partial class AddStorage : Page
    {
        public AddStorage()
        {
            InitializeComponent();
        }

        private void Click_btn_back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_btn_add(object sender, RoutedEventArgs e)
        {

            var error = CheckHelper.Check(TBvolume.Text, "Вместимость склада");
            if (error != null)
            {
                TBerror.Text = error;
                return;
            }

            var newStorage = new Entities.Storage
            {
                volume = int.Parse(TBvolume.Text)
            };

            App.Context.Storage.Add(newStorage);
            App.Context.SaveChanges();

            MessageBox.Show("Склад успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.Navigate(new ListStorage());
        }
    }
}
