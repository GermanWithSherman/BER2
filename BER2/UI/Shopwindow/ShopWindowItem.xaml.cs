using BER2.Extensions;
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

namespace BER2.UI.Shopwindow
{
    /// <summary>
    /// Interaktionslogik für ShopWindowItem.xaml
    /// </summary>
    public partial class ShopWindowItem : UserControl
    {
        private Item _item;
        private ShopWindow _shopWindow;

        private ShopWindowItem()
        {
            InitializeComponent();
        }

        public ShopWindowItem(ShopWindow shopWindow,Item item) : this()
        {
            _item = item;
            _shopWindow = shopWindow;
            TextLabel.Text = item.Label;
            Texture.Source = item.Texture;

            TextPrice.Text = ((long)item.Price).ToStringMoney();
            /*Button.Click += delegate {
                itemSelectWindow.CloseAndReturn(item);
            };*/
        }

        public void HideIfOwned(ItemsCollection itemsCollection)
        {
            if (itemsCollection.Contains(_item))
                Visibility = Visibility.Collapsed;
            else
                Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(GameManager.Instance.TryBuyItem(_item, _item.Price))
                _shopWindow.Update();
        }
    }
}
