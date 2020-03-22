using JetBrains.Annotations;
using ResXManager.Model;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ResXManager.VSMac
{
    [Export(typeof(ISourceFilesProvider))]
    public class VSMacSourceFilesProvider : ISourceFilesProvider
    {
        [NotNull, ItemNotNull]
        public IList<ProjectFile> SourceFiles { get; } = new List<ProjectFile>();

        [CanBeNull]
        public string SolutionFolder { get; }

        public void Invalidate()
        {
        }
    }
}
