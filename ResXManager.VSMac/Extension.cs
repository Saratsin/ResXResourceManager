using ResXManager.Infrastructure;
using ResXManager.Model;
using System;
using System.ComponentModel.Composition.Hosting;

namespace ResXManager.VSMac
{
    public class Extension
    {
        private static readonly Lazy<Extension> InstanceLazy = new Lazy<Extension>(() => new Extension(), true);

        private Extension()
        {
        }

        public static Extension Instance => InstanceLazy.Value;

        public CompositionContainer Container { get; private set; }

        public void Initialize()
        {
            var compositionCatalog = new AggregateCatalog();
            compositionCatalog.Catalogs.Add(new AssemblyCatalog(GetType().Assembly));
            compositionCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ITranslator).Assembly));
            compositionCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ISourceFilesProvider).Assembly));
            Container = new CompositionContainer(compositionCatalog, true);
        }
    }
}
