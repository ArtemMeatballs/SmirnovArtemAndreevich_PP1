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
    /// Логика взаимодействия для AddWood.xaml
    /// </summary>
    public partial class AddWood : Page
    {

        private int idCurrentStorage;

        public AddWood(int idCurrentStorage)
        {
            InitializeComponent();

            this.idCurrentStorage = idCurrentStorage;

            TBstorageNumber.Text = "Загрузка на склад номер" + idCurrentStorage;

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

        private void Click_btn_add(object sender, RoutedEventArgs e)
        {

            var error = CheckHelper.Check(TBvolume.Text, "Объем древесины");
            if (error != null)
            {
                TBerror.Text = error;
                return;
            }

            var storage = App.Context.Storage.First(A => A.id_Storage == idCurrentStorage);

            var infoStorage = App.Context.Wood_in_Storage.Where(A => A.id_Storage == idCurrentStorage).ToList();

            int sum = 0;
            for (int i = 0; i < infoStorage.Count; i++)
            {
                sum += (int)infoStorage[i].volume_wood;
            }

            if (storage.volume == sum)
                MessageBox.Show("Склад полностью заполнен!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);

            else if (storage.volume < sum + int.Parse(TBvolume.Text))
            {
                MessageBoxResult result = MessageBox.Show($"На данный склад поместится только {storage.volume - sum}м.куб.\nОсуществить загрузку?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SaveWood((int)(storage.volume - sum));
                    MessageBox.Show($"На данный склад было загружено {storage.volume - sum}м.куб. {CBwood.Text}\nЗагрузите оставшиеся {int.Parse(TBvolume.Text) - (storage.volume - sum)}м.куб. на другой склад", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                SaveWood(int.Parse(TBvolume.Text));
                MessageBox.Show($"На данный склад было загружено {TBvolume.Text}м.куб. {CBwood.Text}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            NavigationService.Navigate(new Storage(idCurrentStorage));
        }

        private void SaveWood(int volume)
        {
            var wood = App.Context.Wood.ToList();

            var idCurrentWood = wood[CBwood.SelectedIndex].id_Wood;

            var currentWood = App.Context.Wood_in_Storage.FirstOrDefault(A => A.id_Storage == idCurrentStorage && A.id_Wood == idCurrentWood);

            if (currentWood == null)
            {
                var newWood = new Wood_in_Storage
                {
                    id_Storage = idCurrentStorage,
                    id_Wood = idCurrentWood,
                    volume_wood = volume
                };
                App.Context.Wood_in_Storage.Add(newWood);
            }
            else
                currentWood.volume_wood += volume;

            App.Context.SaveChanges();
        }
    }
}
