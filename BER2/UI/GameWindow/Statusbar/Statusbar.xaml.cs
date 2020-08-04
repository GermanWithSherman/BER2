using BER2.GameObjects.Effects;
using BER2.UI.Character;
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

namespace BER2.UI.GameWindow.Statusbar
{
    /// <summary>
    /// Interaktionslogik für Statusbar.xaml
    /// </summary>
    public partial class Statusbar : UserControl
    {
        public Statusbar()
        {
            InitializeComponent();
        }

        public void Update(GameData gameData, Preferences preferences)
        {
            StatusTime.Content = gameData.WorldData.DateTime.ToString("F", preferences.CultureInfo);
            StatusHunger.Content = ((decimal)gameData.CharacterData.PC.statHunger / 1000000m).ToString("P");
            StatusSleep.Content = ((decimal)gameData.CharacterData.PC.statSleep / 1000000m).ToString("P");

            StatusDistress.Content = gameData.CharacterData.PC.Distress;

            EffectIconContainer.Children.Clear();
            foreach (Effect effect in gameData.CharacterData.PC.Effects)
            {
                var effectIcon = new EffectIcon(effect);
                EffectIconContainer.Children.Add(effectIcon);
            }
        }

        private void ButtonCharacter_Click(object sender, RoutedEventArgs e)
        {
            CharacterWindow characterWindow = new CharacterWindow(GameManager.Instance.PC);
            characterWindow.ShowDialog();
        }
    }
}
