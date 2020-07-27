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

namespace BER2.UI.ModsManagementWindow
{

    public partial class ModsManagementWindow : Window
    {
        private readonly Preferences _preferences;
        private readonly ModsServer _modsServer;

        public ModsManagementWindow()
        {
            InitializeComponent();
        }

        public ModsManagementWindow(Preferences preferences, ModsServer modsServer) : this()
        {
            _preferences = preferences;
            _modsServer = modsServer;


            ListViewsRefresh();
        }

        private void ListViewsRefresh()
        {
            ListViewModsActivated.ItemsSource = _modsServer.ActivatedMods;
            ListViewModsDeactivated.ItemsSource = _modsServer.DeactivatedMods;
        }

        private void ButtonActivate_Click(object sender, RoutedEventArgs e)
        {
            Mod mod = ListViewModsDeactivated.SelectedItem as Mod;

            try
            {
                _modsServer.ModActivate(mod);
            }
            catch (ModDependencyException)
            {
                ErrorMessage.Show($"Mod {mod.DisplayName} can't be activated because it depends on other deactivated or missing mods!");
            }
            finally
            {
                ListViewsRefresh();
            }

            
        }

        private void ButtonDeactivate_Click(object sender, RoutedEventArgs e)
        {
            Mod mod = ListViewModsActivated.SelectedItem as Mod;
            try
            {
                _modsServer.ModDeactivate(mod);
            }
            catch(ModDependencyException)
            {
                ErrorMessage.Show($"Mod {mod.DisplayName} can't be deactivated because other activated mods depend on it!");
            }
            finally
            {
                ListViewsRefresh();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GameManager.Instance.Refresh();
        }

        private void ShowDetails(Mod mod)
        {
            if (mod == null)
            {
                ModDetails.Visibility = Visibility.Hidden;
                return;
            }

            ModDetails.Visibility = Visibility.Visible;

            ModDetailsTitle.Text = mod.DisplayName;
            ModDetailsID.Text = mod.ID;
            ModDetailsVersion.Text = mod.Version;

            ModDetailsDependencies.Children.Clear();

            if(mod.dependencyIds.Count == 0)
            {
                TextBlock textBox = new TextBlock();
                textBox.Inlines.Add(new Italic(new Run("none")));
                ModDetailsDependencies.Children.Add(textBox);
            }

            foreach(string dependency in mod.dependencyIds)
            {
                TextBlock textBox = new TextBlock();
                textBox.Text = dependency;
                ModDetailsDependencies.Children.Add(textBox);
            }
        }

        private void ListViewMods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;

            Mod mod = listView.SelectedItem as Mod;
            ShowDetails(mod);
        }
    }
}
