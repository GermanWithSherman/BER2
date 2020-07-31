using BER2.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BER2.UI.Shopwindow
{
    /// <summary>
    /// Interaktionslogik für ShopWindow.xaml
    /// </summary>
    public partial class ShopWindow : Window
    {
        private ShopWindow()
        {
            InitializeComponent();
        }

        public ShopWindow(Shop shop) : this()
        {
            using (Dispatcher.DisableProcessing())
            {
                IEnumerable<Item> items = shop.Items.getItems();

                foreach (Item item in items)
                {
                    ShopWindowItem shopWindowItem = new ShopWindowItem(this, item);
                    ItemsContainer.Items.Add(shopWindowItem);
                }
                Update();
            }
        }

        public void Update()
        {
            UpdateItems();
            TextMoneyLeft.Text = GameManager.Instance.PC.moneyCash.ToStringMoney();
        }

        private void UpdateItems()
        {
            foreach (object obj in ItemsContainer.Items)
            {
                ShopWindowItem shopWindowItem = obj as ShopWindowItem;
                shopWindowItem.HideIfOwned(GameManager.Instance.PC.items);
            }
        }
    }
}
