using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Extensions;

namespace ResXManager.VSMac
{
    [StartupHandlerExtension]
    public class StartupCommandHandler : CommandHandler
    {
        public StartupCommandHandler()
        {
        }

        protected override void Run()
        {
            base.Run();

            Extension.Instance.Initialize();
        }
    }
}