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
        private PC _character;
        private IDictionary<string, OutfitItemButton> _slotButton;
        private OutfitRequirement _outfitRequirement;
        private CommandsCollection _onClose;

        public OutfitWindow()
        {
            InitializeComponent();

            _slotButton = new Dictionary<string, OutfitItemButton>();
            _slotButton["Clothes"] = ButtonBody;
            _slotButton["Bra"] = ButtonBra;
            _slotButton["Panties"] = ButtonPanties;
            _slotButton["Shoes"] = ButtonShoes;

        }

        private OutfitWindow(PC character) : this()
        {
            _character = character;
            foreach (KeyValuePair<string, OutfitItemButton> keyValue in _slotButton)
            {
                keyValue.Value.Initialize(this,keyValue.Key, character);
            }
            Update();
        }

        public OutfitWindow(PC character, OutfitRequirement outfitRequirement, CommandsCollection onClose) : this(character)
        {
            _outfitRequirement = outfitRequirement;
            _onClose = onClose;
        }

        public void Update()
        {
            TextGender.Text = _character.GenderDress;
            TextStyle.Text = _character.CurrentOutfit.Style;
            TextGenderAppear.Text = _character.GenderPerceived;

            if(_character.CurrentOutfit.Skimpiness > 0)
            {
                WidgetSkimpiness.Visibility = Visibility.Visible;
                TextSkimpiness.Text = BER2.Language.Language.Skimpiness(_character.CurrentOutfit.Skimpiness);
            }
            else
            {
                WidgetSkimpiness.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_outfitRequirement.TryOrError(_character.CurrentOutfit))
            {
                e.Cancel = true;
                return;
            }

            _onClose.execute();

        }
    }
}
