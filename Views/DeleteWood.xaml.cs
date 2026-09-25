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
using WoodAccountingSystem.Infrastructure;

namespace WoodAccountingSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для DeleteWood.xaml
    /// </summary>
    public partial class DeleteWood : Page
    {
        private int idCurrentStorage;

        public DeleteWood(int idCurrentStorage)
        {
            InitializeComponent();

            this.idCurrentStorage = idCurrentStorage;

            TBstorageNumber.Text = "Выгрузка со склада номер " + idCurrentStorage;

            var wood = App.Context.Wood.ToList();

            for (int i = 0; i < wood.Count; i++)
            {
                CBwood.Items.Add(wood[i].name + " " + wood[i].quality + " сорт");
            }
            CBwood.SelectedIndex = 0;
        }

        private void Click_btn_back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_btn_sub(object sender, RoutedEventArgs e)
        {

            var error = CheckHelper.Check(TBvolume.Text, "Объем древесины");
            if (error != null)
            {
                TBerror.Text = error;
                return;
            }

            var wood = App.Context.Wood.ToList();

            var idCurrentWood = wood[CBwood.SelectedIndex].id_Wood;

            var currentWood = App.Context.Wood_in_Storage.FirstOrDefault(A => A.id_Storage == idCurrentStorage && A.id_Wood == idCurrentWood);

            if (currentWood == null)
                MessageBox.Show($"На данном складе отсутствует {CBwood.Text}!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);

            else if (currentWood.volume_wood < int.Parse(TBvolume.Text))
            {
                MessageBoxResult result = MessageBox.Show($"С данного склада можно выгрузить только {currentWood.volume_wood}м.куб.\nОсуществить выгрузку?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    App.Context.Wood_in_Storage.Remove(currentWood);
                    App.Context.SaveChanges();

                    MessageBox.Show($"С данного склада было выгружено {currentWood.volume_wood}м.куб. {CBwood.Text}\nВыгрузите оставшиеся {int.Parse(TBvolume.Text) - currentWood.volume_wood}м.куб. с другого склада", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show($"С данного склада было выгружено {TBvolume.Text}м.куб. {CBwood.Text}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);

                if (currentWood.volume_wood == int.Parse(TBvolume.Text))
                    App.Context.Wood_in_Storage.Remove(currentWood);
                else
                    currentWood.volume_wood -= int.Parse(TBvolume.Text);

                App.Context.SaveChanges();
            }

            NavigationService.Navigate(new Storage(idCurrentStorage));
        }
    }
}
