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

namespace BER2.UI.GameWindow.Locations
{
    /// <summary>
    /// Interaktionslogik für LocationButton.xaml
    /// </summary>
    public partial class LocationButton : UserControl
    {
        public LocationButton()
        {
            InitializeComponent();
        }

        public LocationButton(LocationConnection locationConnection):this()
        {
            Image.Source = locationConnection.Texture;
            TextDestinationLabel.Text = locationConnection.Label;
            TextDuration.Text = locationConnection.Duration.ToString();
            Button.IsEnabled = locationConnection.IsEnabled();
            Button.Click += delegate { locationConnection.execute(); };
        }
    }
}
