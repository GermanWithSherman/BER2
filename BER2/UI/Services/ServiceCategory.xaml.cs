using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BER2.UI.Services
{
    /// <summary>
    /// Interaktionslogik für ServiceCategory.xaml
    /// </summary>
    public partial class ServiceCategory : TabItem
    {
        private GameObjects.Services.ServiceCategory _serviceCategory;
        private ServicesWindow _servicesWindow;

        private ServiceCategory()
        {
            InitializeComponent();
        }

        public ServiceCategory(ServicesWindow servicesWindow,GameObjects.Services.ServiceCategory serviceCategory) : this()
        {
            _serviceCategory = serviceCategory;
            _servicesWindow = servicesWindow;

            Header = _serviceCategory.Title;

            ServiceContainer.Children.Clear();

            foreach (GameObjects.Services.Service service in _serviceCategory.Services)
            {
                var button = new System.Windows.Controls.Button();
                button.Content = service.Label;
                button.Click += delegate{ service.onBuy.execute(); _servicesWindow.Close(); GameManager.Instance.Update(); };
                ServiceContainer.Children.Add(button);
            }
        }
    }
}
