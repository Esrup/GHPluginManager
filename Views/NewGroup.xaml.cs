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
    /// Interaction logic for NewGroup.xaml
    /// </summary>
    public partial class NewGroup : Page
    {
        public class Plugin
        {
            public string Name { get; set; }
            public string FullName { get; set; }
        }
        private List<List<Plugin>> Plugins { get; set; }

        public NewGroup()
        {
            InitializeComponent();
            Plugins = loadPluginNames();
            addPlugins();
        }

        private void addPlugins()
        {
            for (int i = 0; i < Plugins.Count; i++)
            {
                for (int ii = 0; ii < Plugins[i].Count; ii++)
                {
                    
                    PluginList.Items.Add(Plugins[i][ii]);
                }
            }

        }
        private List<List<Plugin>> loadPluginNames()
        {
            List<List<Plugin>> pluginName = new List<List<Plugin>>();
            List<string> pathList = new List<string>();


            //Add default folder
            pathList.Add(Folders.DefaultAssemblyFolder);

            //Add defaultRhino6 folder
            pathList.Add(Folders.DefaultAssemblyFolderVersion6);

            //Add Custom folders
            pathList.AddRange(Folders.CustomAssemblyFolders);

            for (int i = 0; i < pathList.Count; i++)
            {
                List<Plugin> plugins = new List<Plugin>();
                var files = System.IO.Directory.GetFiles(pathList[i], "*.*", System.IO.SearchOption.AllDirectories)
                    .Where(s => s.EndsWith(".gha") || s.EndsWith(".ghpy"));
                foreach (var item in files)
                {
                    Plugin plugin = new Plugin();
                    plugin.FullName = item;
                    var splitnames = item.Split('\\');
                    plugin.Name = splitnames[splitnames.Length - 1].Split('.')[0];
                    plugins.Add(plugin);
                }
                pluginName.Add(plugins);
            }
            

            return pluginName;
        }
        private void saveGroup(string[] list)
        {
            string groupName = GroupName.Text;
            string filePath = Folders.AppDataFolder + $"\\PluginManager\\{groupName}.txt";

            //Check if Group Name exist, if it does then add a unique number after
            if (File.Exists(filePath))
            {
                filePath = getNextFileName(filePath);
            }
            File.WriteAllLines(filePath, list);
        }


        private string getNextFileName(string fileName)
        {
            //https://stackoverflow.com/a/24563008
            string extension = System.IO.Path.GetExtension(fileName);

            int i = 0;
            while (File.Exists(fileName))
            {
                if (i == 0)
                    fileName = fileName.Replace(extension, "(" + ++i + ")" + extension);
                else
                    fileName = fileName.Replace("(" + i + ")" + extension, "(" + ++i + ")" + extension);
            }

            return fileName;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Groups());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedPlugins = new List<string>();
            foreach (var item in PluginList.SelectedItems)
            {
                Plugin pl = (Plugin)item;
                selectedPlugins.Add(pl.FullName);
            }
            saveGroup(selectedPlugins.ToArray());

            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Groups());
        }
    }
}
