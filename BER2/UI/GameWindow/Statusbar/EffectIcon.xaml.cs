using BER2.GameObjects.Effects;
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
    public partial class EffectIcon : UserControl
    {
        private EffectIcon()
        {
            InitializeComponent();
        }

        public EffectIcon(Effect effect) : this()
        {
            LabelMain.Content = effect.Label.Substring(0, 1);

            switch (effect.Color)
            {
                case GameObjects.Effects.Effect.EffectColor.green:
                    LabelMain.Background = Brushes.Green;
                    break;
                case GameObjects.Effects.Effect.EffectColor.red:
                    LabelMain.Background = Brushes.Red;
                    break;
                case GameObjects.Effects.Effect.EffectColor.yellow:
                    LabelMain.Background = Brushes.Yellow;
                    break;
            }

            TextTooltipLabel.Text = effect.Label;
        }
    }
}
