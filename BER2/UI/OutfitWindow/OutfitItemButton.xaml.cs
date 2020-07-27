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

namespace BER2.UI.OutfitWindow
{
    /// <summary>
    /// Interaktionslogik für OutfitItemButton.xaml
    /// </summary>
    public partial class OutfitItemButton : UserControl
    {

        public string ItemSlot;

        public string Label
        {
            get => TextTitle.Text;
            set => TextTitle.Text = value;
        }

        public ImageSource Texture
        {
            get => Image.Source;
            set => Image.Source = value;
        }

        public string ItemName
        {
            get => TextName.Text;
            set => TextName.Text = value;
        }

        public OutfitItemButton()
        {
            InitializeComponent();
        }


    }
}
