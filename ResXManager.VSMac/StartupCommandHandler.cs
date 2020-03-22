using MonoDevelop.Components.Commands;
using ResXManager.Model;
using System;
using System.ComponentModel.Composition.Hosting;

namespace ResXManager.VSMac
{
    public class StartupCommandHandler : CommandHandler
    {
        public StartupCommandHandler()
        {
        }

        //protected override void Run()
        //{
        //    base.Run();

        //    var assembly = GetType().Assembly;
        //    var folder = Path.GetDirectoryName(assembly.Location);
            
        //    _compositionCatalog = new AggregateCatalog();
        //    _compositionContainer = new CompositionContainer(_compositionCatalog, true);
        //    _compositionCatalog.Catalogs.Add(new AssemblyCatalog(assembly));
        //    _compositionCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ResXManager.Infrastructure.ITracer).Assembly));
        //    _compositionCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ResXManager.Model.GlobalExtensions).Assembly));

        //    _sourceFilesProvider = _compositionContainer.GetExportedValue<SourceFilesProvider>();
        //    ResourceManager = _compositionContainer.GetExportedValue<ResourceManager>();
        //    ResourceManager.BeginEditing += ResourceManager_BeginEditing;

        //    Configuration = _compositionContainer.GetExportedValue<Configuration>();
        //}

        //[NotNull]
        //public ResourceManager ResourceManager { get; }

    }
}
