using JetBrains.Annotations;
using ResXManager.Infrastructure;
using ResXManager.Model;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Text.RegularExpressions;


namespace ResXManager.VSMac
{
    [Export]
    [Export(typeof(ISourceFilesProvider))]
    internal class SourceFilesProvider : ISourceFilesProvider, IFileFilter
    {
        [CanBeNull]
        private Regex _fileExclusionFilter;
        [CanBeNull]
        public string SolutionFolder { get; set; }
        [CanBeNull]
        public string ExclusionFilter { get; set; }

        public IList<ProjectFile> SourceFiles
        {
            get
            {
                var folder = SolutionFolder;
                if (string.IsNullOrEmpty(folder))
                    return Array.Empty<ProjectFile>();

                _fileExclusionFilter = ExclusionFilter.TryCreateRegex();

                return new DirectoryInfo(folder).GetAllSourceFiles(this);
            }
        }

        public void Invalidate()
        {
        }

        public bool IsSourceFile(ProjectFile file)
        {
            return false;
        }

        public bool IncludeFile(ProjectFile file)
        {
            return _fileExclusionFilter?.IsMatch(file.RelativeFilePath) != true;
        }
    }
}
