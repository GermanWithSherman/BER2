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

namespace BER2.UI.Character
{
    /// <summary>
    /// Interaktionslogik für CharacterWindow.xaml
    /// </summary>
    public partial class CharacterWindow : Window
    {

        private PC _character;

        private CharacterWindow()
        {
            InitializeComponent();
        }

        public CharacterWindow(PC character):this()
        {
            _character = character;
            Update();
        }

        private void Update()
        {
            TextNameFirst.Text = _character.NameFirst;
            TextNameLast.Text = _character.NameLast;

            TextAppearanceAge.Text = _character.Age.ToString();
            TextAppearanceGender.Text = _character.BodyData.GenderVisible;
        }
    }
}
