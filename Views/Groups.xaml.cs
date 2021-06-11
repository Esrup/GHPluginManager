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
using Grasshopper;
using System.IO;

namespace GHPluginManager.Views
{
    /// <summary>
    /// Interaction logic for Groups.xaml
    /// </summary>
    public partial class Groups : Page
    {
        public Groups()
        {
            InitializeComponent();
            readGroups();
            putGroupsOnList();
        }

        class PluginGroup
        {
            public string GroupName { get; set; }
            public string[] PluginPath { get; set; }
            public string[] PluginName { get; set; }
        }
        List<PluginGroup> PluginGroups { get; set; }

        private void readGroups()
        {
            PluginGroups = new List<PluginGroup>();
            var pluginGroupspaths = Directory.GetFiles((Folders.AppDataFolder + $"PluginManager\\"), "*.txt", SearchOption.AllDirectories);
            for (int i = 0; i < pluginGroupspaths.Length; i++)
            {
                var plGroup = new PluginGroup();
                var splitPath = pluginGroupspaths[i].Split('\\');
                plGroup.GroupName = splitPath[splitPath.Length - 1].Split('.')[0];
                if (plGroup.GroupName == "PluginManager")
                    continue;
                plGroup.PluginPath = File.ReadAllLines(pluginGroupspaths[i]);
                plGroup.PluginName = new string[plGroup.PluginPath.Length];


                for (int ii = 0; ii < plGroup.PluginPath.Length; ii++)
                {
                    var splitPluginPath = plGroup.PluginPath[ii].Split('\\');
                    plGroup.PluginName[ii] = splitPluginPath[splitPluginPath.Length - 1].Split('.')[0];
                }

                PluginGroups.Add(plGroup);
            }

        }
        private void putGroupsOnList()
        {

            for (int i = 0; i < PluginGroups.Count; i++)
            {

                itemListGroups.Items.Add(PluginGroups[i]);
            }
        }

        private void AddGroupClick(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new NewGroup());
        }

        private void unloadPlugins(PluginGroup pluginList)
        {
            for (int i = 0; i < pluginList.PluginName.Length; i++)
            {
                var temp = System.IO.Path.GetDirectoryName(pluginList.PluginPath[i]);
                var temp2 = System.IO.Path.GetDirectoryName(pluginList.PluginPath[i]) + pluginList.PluginName[i] + ".no6";
                File.WriteAllText(System.IO.Path.GetDirectoryName(pluginList.PluginPath[i]) + "\\" + pluginList.PluginName[i] + ".no6", "");
            }
        }
        private void loadAllPlugins()
        {
            List<string> pathList = new List<string>();

            //Add default folder
            pathList.Add(Folders.DefaultAssemblyFolder);

            //Add defaultRhino6 folder
            pathList.Add(Folders.DefaultAssemblyFolderVersion6);

            //Add Custom folders
            pathList.AddRange(Folders.CustomAssemblyFolders);

            try
            {
                for (int i = 0; i < pathList.Count; i++)
                {
                    foreach (var item in System.IO.Directory.GetFiles(pathList[i], "*.no6", System.IO.SearchOption.AllDirectories))
                    {
                        File.Delete(item);
                    }
                }
            }
            catch
            {


            }
        }

        private void SetasDefaultClick(object sender, RoutedEventArgs e)
        {
            string filePath = Folders.AppDataFolder + $"\\PluginManager\\PluginManager.txt";
            var defaultPlGroup = (PluginGroup)itemListGroups.SelectedItem;
            File.WriteAllText(filePath, defaultPlGroup.GroupName);
            loadAllPlugins();
            unloadPlugins(defaultPlGroup);
        }

        private void LoadNowClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
