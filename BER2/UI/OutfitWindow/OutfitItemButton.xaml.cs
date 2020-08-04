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
using BER2.UI.ItemSelectWindow;

namespace BER2.UI.OutfitWindow
{

    public partial class OutfitItemButton : UserControl
    {

        //public string ItemSlot;

        private string Label
        {
            get => TextTitle.Text;
            set => TextTitle.Text = value;
        }

        private ImageSource Texture
        {
            get => Image.Source;
            set => Image.Source = value;
        }

        private OutfitWindow _outfitWindow;

        public OutfitItemButton()
        {
            InitializeComponent();
        }

        /*private string ItemName
        {
            get => TextName.Text;
            set => TextName.Text = value;
        }*/


        private PC _character;
        private string _slot;

        public void Initialize(OutfitWindow outfitWindow, string slot, PC character)
        {
            _outfitWindow = outfitWindow;
            _slot = slot;
            _character = character;

            Update();
        }

        private void Update()
        {
            Label = _slot;
            Texture = _character.GetClothingslotTexture(_slot);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ItemsFilter itemsFilter = new ItemsFilter();
            itemsFilter.Slots = new ModableStringList() { _slot };

            ItemSelectWindow.ItemSelectWindow itemSelectWindow = new ItemSelectWindow.ItemSelectWindow(itemsFilter,_character);

            if(itemSelectWindow.ShowDialog() == true)
            {
                Item item = itemSelectWindow.Result;
                _character.CurrentOutfit.setItem(_slot, item);
                Update();
                _outfitWindow.Update();
                GameManager.Instance.Update();
            }
        }
    }
}
