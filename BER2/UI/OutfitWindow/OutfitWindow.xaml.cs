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

namespace BER2.UI.OutfitWindow
{
    /// <summary>
    /// Interaktionslogik für OutfitWindow.xaml
    /// </summary>
    public partial class OutfitWindow : Window
    {

        private IDictionary<string, OutfitItemButton> _slotButton;

        public OutfitWindow()
        {
            InitializeComponent();

            _slotButton = new Dictionary<string, OutfitItemButton>();
            _slotButton["Body"] = ButtonBody;
            _slotButton["Bra"] = ButtonBra;
            _slotButton["Panties"] = ButtonPanties;
            _slotButton["Shoes"] = ButtonShoes;

            foreach (KeyValuePair<string,OutfitItemButton> keyValue in _slotButton)
            {
                keyValue.Value.Label = keyValue.Key;
            }
            /*ButtonBody.Label = "Body";
            ButtonBra.Label = "Bra";
            ButtonPanties.Label = "Panties";
            ButtonShoes.Label = "Shoes";*/
        }

        public OutfitWindow(PC character) : this()
        {
            /*ButtonBody.SetItem(character.CurrentOutfit["Body"]);
            ButtonBra.SetItem(character.CurrentOutfit["Bra"]);
            ButtonPanties.SetItem(character.CurrentOutfit["Panties"]);
            ButtonShoes.SetItem(character.CurrentOutfit["Shoes"]);*/
            foreach (KeyValuePair<string, OutfitItemButton> keyValue in _slotButton)
            {
                /*Item item = character.CurrentOutfit[keyValue.Key];
                if (item == null)
                {
                    keyValue.Value.Texture = character.GetClothingslotTexture(keyValue.Key);
                    keyValue.Value.ItemName = "";
                    return;
                }*/
                keyValue.Value.Texture = character.GetClothingslotTexture(keyValue.Key);
                keyValue.Value.ItemName = "";


            }
        }
    }
}
