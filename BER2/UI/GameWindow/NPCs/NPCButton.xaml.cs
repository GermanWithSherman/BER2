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

namespace BER2.UI.GameWindow.NPCs
{

    public partial class NPCButton : UserControl
    {
        public NPCButton()
        {
            InitializeComponent();
        }

        public NPCButton(NPC npc) : this()
        {
            Image.Source = npc.Texture;
        }
    }
}
