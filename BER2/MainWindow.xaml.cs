using BER2.UI.GameWindow.Locations;
using BER2.UI.GameWindow.NPCs;
using Microsoft.Win32;
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

namespace BER2
{
    
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();

            GameManager.Instance.OnUIUpdateEvent += UIUpdate;

            if(!TryLoadGameFromPreferences())
                ShowOpenGameDialog();
        }

        private void SetMainContent(IEnumerable<IMainContent> contents)
        {
            MainContent.Children.Clear();
            foreach (IMainContent content in contents)
            {
                if (content == null)
                    continue;

                UIElement uiElement = content.getVisual();

                if (uiElement is Image)
                    //((Image)uiElement).Height = 150;
                    ((Image)uiElement).MaxHeight = 400;

                MainContent.Children.Add(uiElement);
            }
        }

        private void SetOptions(IEnumerable<Option> options)
        {
            Options.Children.Clear();

            foreach (Option option in options)
            {
                Button button = new Button();

                button.Content = option.Text.Text();

                button.Click += delegate { option.Commands.execute(); };

                Options.Children.Add(button);
            }

        }

        public void ShowOpenGameDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "BER Manifest Files (*.ber)|*.ber";
            if (openFileDialog.ShowDialog() == true)
                OpenGame(openFileDialog.FileName);
        }

        private bool TryLoadGameFromPreferences()
        {
            return GameManager.Instance.TryLoadGameFromPreferences();
        }

        private void OpenGame(string path)
        {
            GameManager.Instance.OpenGame(path);
        }

        public void UIUpdate(GameData gameData,Preferences preferences)
        {
            SetLocations(GameManager.Instance.CurrentReachableLocations);
            SetMainContent(GameManager.Instance.MainContent);
            SetOptions(GameManager.Instance.CurrentOptions);

            NPCs.Children.Clear();
            foreach (NPC npc in GameManager.Instance.GameData.NpcsPresent)
            {
                NPCButton npcButton = new NPCButton(npc);
                npcButton.Width = 200;
                NPCs.Children.Add(npcButton);
            }

            Statusbar.StatusTime.Content = gameData.WorldData.DateTime.ToString("F", preferences.CultureInfo);
        }

        private void SetLocations(IEnumerable<LocationConnection> locationConnections)
        {
            Locations.Children.Clear();

            foreach (LocationConnection locationConnection in locationConnections)
            {
                LocationButton locationButton = new LocationButton(locationConnection);
                Locations.Children.Add(locationButton);
            }
        }

        private void menuOpenGame_Click(object sender, RoutedEventArgs e)
        {
            ShowOpenGameDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GameManager.Instance.PreferencesSave();
        }

        private void menuSaveSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "BER Savegame Files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
                GameManager.Instance.SaveSavegame(openFileDialog.FileName);
            
        }

        private void menuSaveSaveQuick_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.SaveSavegame();
        }

        private void menuLoadSaveQuick_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Instance.LoadSavegame();
        }

        

        private void menuLoadSave_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "BER Savegame Files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
                GameManager.Instance.LoadSavegame(openFileDialog.FileName);
        }
    }
}
