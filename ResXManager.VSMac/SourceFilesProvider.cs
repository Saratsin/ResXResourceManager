using JetBrains.Annotations;
using MonoDevelop.Ide;
using ResXManager.Model;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using TomsToolbox.Composition;

namespace ResXManager.VSMac
{
    [Export(typeof(ISourceFilesProvider))]
    internal class SourceFilesProvider : ISourceFilesProvider
    {
        private readonly IExportProvider _exportProvider;
        private readonly Configuration _configuration;

        [ImportingConstructor]
        public SourceFilesProvider([NotNull] IExportProvider exportProvider, [NotNull] Configuration configuration)
        {
            _exportProvider = exportProvider;
            _configuration = configuration;
        }

        public IList<ProjectFile> SourceFiles => DteSourceFiles.ToList().AsReadOnly();

        [CanBeNull]
        public string SolutionFolder => "";// Solution.SolutionFolder;

        [NotNull, ItemNotNull]
        private IEnumerable<ProjectFile> DteSourceFiles
        {
            get
            {
                var fileFilter = new FileFilter(_configuration);
                return GetProjectFiles(fileFilter);
            }
        }

        public void Invalidate()
        {
            //Solution.Invalidate();
        }

        //[NotNull, ItemNotNull]
        private IEnumerable<ProjectFile> GetProjectFiles(IFileFilter fileFilter)
        {
            return Enumerable.Empty<ProjectFile>();
            //return Solution.GetProjectFiles(fileFilter);
        }

        //[NotNull]
        //private DteSolution Solution => _exportProvider.GetExportedValue<DteSolution>();
    }
}
