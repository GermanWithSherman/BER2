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
using System.Windows.Shapes;

namespace BER2.UI.ItemSelectWindow
{
    /// <summary>
    /// Interaktionslogik für ItemSelectWindow.xaml
    /// </summary>
    public partial class ItemSelectWindow : Window
    {

        public Item Result;

        public ItemSelectWindow(ItemsFilter itemsFilter,PC character)
        {
            InitializeComponent();

            //IEnumerable<Item> items = GameManager.Instance.ItemsLibrary.items();
            ItemsCollection ownedItems = character.items;

            IEnumerable<Item> filteredItems = itemsFilter.filter(ownedItems);

            foreach (Item item in filteredItems)
            {
                ItemSelectItem isi = new ItemSelectItem(this,item);
                ItemsContainer.Items.Add(isi);
            }
        }


        public void CloseAndReturn(Item result)
        {
            Result = result;
            this.DialogResult = true;
            Close();
        }

        private void ButtonNoItem_Click(object sender, RoutedEventArgs e)
        {
            Result = null;
            this.DialogResult = true;
            Close();
        }
    }
}
