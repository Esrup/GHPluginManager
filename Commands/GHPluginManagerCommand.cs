using System.Runtime.InteropServices;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.UI;
using GHPluginManager.Views;

namespace GHPluginManager.Commands
{
  [Guid("5876514f-0bbb-47a9-add7-59dc07f1fd63")]
  public class GHPluginManagerHost : RhinoWindows.Controls.WpfElementHost
  {
    public GHPluginManagerHost(uint docSn) 
      : base(new GHPluginManagerHostPanel(docSn), null)
    {
    }
  }

  public class GHPluginManagerCommand : Command
  {
    public GHPluginManagerCommand()
    {
      Instance = this;
      Panels.RegisterPanel(GHPluginManagerPlugin.Instance, typeof(GHPluginManagerHost), "SampleWpfPanel", System.Drawing.SystemIcons.WinLogo);
    }

    public static GHPluginManagerCommand Instance
    {
      get; private set;
    }

    public override string EnglishName => "GHPluginManagerPanel";

    protected override Result RunCommand(RhinoDoc doc, RunMode mode)
    {
      var panel_id = typeof(GHPluginManagerHost).GUID;
      var panel_visible = Panels.IsPanelVisible(panel_id);

      var prompt = (panel_visible)
        ? "Sample panel is visible. New value"
        : "Sample Manager panel is hidden. New value";

      var go = new GetOption();
      go.SetCommandPrompt(prompt);
      var hide_index = go.AddOption("Hide");
      var show_index = go.AddOption("Show");
      var toggle_index = go.AddOption("Toggle");
      go.Get();

      if (go.CommandResult() != Result.Success)
        return go.CommandResult();

      var option = go.Option();
      if (null == option)
        return Result.Failure;

      var index = option.Index;
      if (index == hide_index)
      {
        if (panel_visible)
          Panels.ClosePanel(panel_id);
      }
      else if (index == show_index)
      {
        if (!panel_visible)
          Panels.OpenPanel(panel_id);
      }
      else if (index == toggle_index)
      {
        if (panel_visible)
          Panels.ClosePanel(panel_id);
        else
          Panels.OpenPanel(panel_id);
      }
      return Result.Success;
    }
  }
}
