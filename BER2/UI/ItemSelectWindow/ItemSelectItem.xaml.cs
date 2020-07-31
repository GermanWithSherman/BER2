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

namespace BER2.UI.ItemSelectWindow
{
    /// <summary>
    /// Interaktionslogik für ItemSelectItem.xaml
    /// </summary>
    public partial class ItemSelectItem : UserControl
    {
        private ItemSelectItem()
        {
            InitializeComponent();
        }

        public ItemSelectItem(ItemSelectWindow itemSelectWindow,Item item):this()
        {
            TextLabel.Text = item.Label;
            Texture.Source = item.Texture;
            Button.Click += delegate {
                itemSelectWindow.CloseAndReturn(item);
            };
        }
    }
}
