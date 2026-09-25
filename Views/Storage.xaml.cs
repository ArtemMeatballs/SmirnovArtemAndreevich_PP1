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
    /// Логика взаимодействия для Storage.xaml
    /// </summary>
    public partial class Storage : Page
    {

        private int idCurrentStorage;

        public Storage(int idCurrentStorage)
        {
            InitializeComponent();

            this.idCurrentStorage = idCurrentStorage;

            var storage = App.Context.Storage.First(A => A.id_Storage == idCurrentStorage);
            var infoStorage = App.Context.Wood_in_Storage.Where(A => A.id_Storage == idCurrentStorage).ToList();

            TBstorageNumber.Text = "Склад номер " + idCurrentStorage;

            TBstorageVolume.Text = $"Вместимость: {storage.volume}м.куб.";

            int sum = 0;
            for (int i = 0; i < infoStorage.Count; i++)
            {
                sum += (int)infoStorage[i].volume_wood;
            }
            TBstorageFree.Text = $"Свободно: {storage.volume - sum}м.куб.";

            if (infoStorage.Count != 0)
            {
                TBwoodNotFound.Visibility = Visibility.Collapsed;

                var wood = App.Context.Wood.ToList();

                for (int i = 0; i < infoStorage.Count; i++)
                {
                    TextBlock tb = new TextBlock();
                    tb.FontSize = 17;
                    tb.Margin = new Thickness(0, 0, 0, 5);

                    for (int j = 0; j < wood.Count; j++)
                    {
                        if (wood[j].id_Wood == infoStorage[i].id_Wood)
                        {
                            tb.Text = $"{i + 1}.    {wood[j].name} {wood[j].quality} сорт - {infoStorage[i].volume_wood}м.куб.";
                            break;
                        }
                    }
                    
                    SPinfoStorage.Children.Add(tb);
                }
            }
        }

        private void Click_btn_back(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListStorage());
        }
        private void Click_btn_add(object sender, RoutedEventArgs e)        
        {
            NavigationService.Navigate(new AddWood(idCurrentStorage));
        }
        private void Click_btn_sub(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DeleteWood(idCurrentStorage));
        }
    }
}
