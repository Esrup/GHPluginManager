using Rhino;
using Rhino.Commands;
using RhinoWindows;

namespace GHPluginManager.Commands
{
  public class GHPluginManagerSemiModalDialogCommand : Command
  {
    public override string EnglishName => "GHPluginManagerSemiModalDialog";

    protected override Result RunCommand(RhinoDoc doc, RunMode mode)
    {
      var dialog = new Views.GHPluginManagerDialog();
      dialog.ShowSemiModal(RhinoApp.MainWindowHandle());

      return Result.Success;
    }
  }
}