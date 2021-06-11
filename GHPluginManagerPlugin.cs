using Rhino.PlugIns;

namespace GHPluginManager
{
 
  public class GHPluginManagerPlugin : Rhino.PlugIns.PlugIn
  {
    public GHPluginManagerPlugin()
    {
      Instance = this;
    }

    
    public static GHPluginManagerPlugin Instance
    {
      get;
      private set;
    }

    protected override LoadReturnCode OnLoad(ref string errorMessage)
    {
      return LoadReturnCode.Success;
    }
  }
}
