using BER2.GameObjects.Services;
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

namespace BER2.UI.Services
{
    /// <summary>
    /// Interaktionslogik für ServicesWindow.xaml
    /// </summary>
    public partial class ServicesWindow : Window
    {
        private ServicesWindow()
        {
            InitializeComponent();
        }

        public ServicesWindow(Servicepoint servicepoint) : this()
        {
            ServiceCategoriesContainer.Items.Clear();

            foreach (BER2.GameObjects.Services.ServiceCategory serviceCategory in servicepoint.ServiceCategories)
            {
                var uiSC = new ServiceCategory(this,serviceCategory);
                ServiceCategoriesContainer.Items.Add(uiSC);
            }
        }
    }
}
