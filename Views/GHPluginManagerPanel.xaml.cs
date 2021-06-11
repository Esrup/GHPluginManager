using System.Windows.Controls;
using GHPluginManager.ViewModels;

namespace GHPluginManager.Views
{
  /// <summary>
  /// Interaction logic for GHPluginManagerPanel.xaml
  /// </summary>
  public partial class GHPluginManagerHostPanel
  {
    public GHPluginManagerHostPanel(uint documentSerialNumber)
    {
      DataContext = new GHPluginManagerHostPaneViewModel(documentSerialNumber);
      InitializeComponent();
    }

    private GHPluginManagerHostPaneViewModel ViewModel => DataContext as GHPluginManagerHostPaneViewModel;

    private void Button1_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      var vm = ViewModel;
      if (vm == null)
        return;

      for (var i = 0; i < 20; i++)
      {
        // Keep changing the same setting to make sure it only writes the file
        // one time when this loop is done
        vm.IncrementCounter();
        
        // Change a different settings each time which should reset the save
        // settings timer
        if (vm.UseMultipleCounters == true)
          vm.IncrementCounter(i);
      
        // The save timer fires every 500ms, this is here to make sure we go past
        // that a couple of times.  This is useful when testing the settings auto
        // writing function, it should only get called once when this loop is 
        // finished.
        System.Threading.Thread.Sleep(100);
      }

      vm.Message = $"Counter set to {vm.Counter}";
    }

    private void Button2_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      Rhino.RhinoApp.RunScript("_Line 0,0,0 5,5,0", false);
    }

    private void Button3_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      Rhino.RhinoApp.RunScript("_Circle", false);
    }


  }
}
