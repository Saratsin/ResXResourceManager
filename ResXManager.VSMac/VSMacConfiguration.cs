using JetBrains.Annotations;
using ResXManager.Infrastructure;
using ResXManager.Model;
using System.ComponentModel.Composition;

namespace ResXManager.VSMac
{
    [Export(typeof(IConfiguration))]
    public class VSMacConfiguration : Configuration
    {
        [ImportingConstructor]
        public VSMacConfiguration([NotNull] ITracer tracer) : base(tracer)
        {
        }

        public override bool IsScopeSupported => false;

        public override ConfigurationScope Scope => ConfigurationScope.Global;
    }
}
