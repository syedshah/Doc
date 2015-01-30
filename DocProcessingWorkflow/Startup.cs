using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DocProcessingWorkflow.Startup))]

namespace DocProcessingWorkflow
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      ConfigureAuth(app);
    }
  }
}
