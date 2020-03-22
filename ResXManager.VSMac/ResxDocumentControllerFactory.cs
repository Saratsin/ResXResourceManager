using Microsoft.CodeAnalysis;
using MonoDevelop.Ide.Gui.Documents;
using ResXManager.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ResXManager.VSMac
{
    [ExportDocumentControllerFactory(FileExtension = ".resx")]
    public class ResxDocumentControllerFactory : FileDocumentControllerFactory
    {
        private const string ResxFileRegexPattern = @"^({0})\.((([A-Za-z][A-Za-z](\-[A-Za-z][A-Za-z])?)\.)?)resx$";

        private static readonly Regex ResxFileRegex = new Regex(string.Format(ResxFileRegexPattern, ".*?"));

        private readonly Dictionary<string, ResxDocumentController> _resxDocumentControllers = new Dictionary<string, ResxDocumentController>();

        public override Task<DocumentController> CreateController(FileDescriptor fileDescriptor, DocumentControllerDescription controllerDescription)
        {
            var resxGroupKey = GetResxGroupKey(fileDescriptor);
            if (!_resxDocumentControllers.TryGetValue(resxGroupKey, out var resxDocumentController))
            {
                var projectName = fileDescriptor.Owner.Name;
                var sourceFiles = ProduceSourceFiles(fileDescriptor.FilePath, projectName).ToList();
                var sourceFilesProvider = Extension.Instance.Container.GetExportedValue<ISourceFilesProvider>();
                foreach (var sourceFile in sourceFiles)
                {
                    sourceFilesProvider.SourceFiles.Add(sourceFile);
                }

                resxDocumentController = new ResxDocumentController(resxGroupKey, controller =>
                {
                    _resxDocumentControllers.Remove(controller.ResxGroupKey);
                    foreach (var sourceFile in sourceFiles)
                    {
                        sourceFilesProvider.SourceFiles.Remove(sourceFile);
                    }
                });
                _resxDocumentControllers[resxGroupKey] = resxDocumentController;
            }

            return Task.FromResult<DocumentController>(resxDocumentController);
        }

        private IEnumerable<ProjectFile> ProduceSourceFiles(string openedFilePath, string projectName)
        {
            var openedFileName = Path.GetFileName(openedFilePath);
            var openedFileMatch = ResxFileRegex.Match(openedFileName);
            var sourceFilesKey = openedFileMatch.Groups[1].Value;

            var sourceFilesDirectory = Path.GetDirectoryName(openedFilePath);
            var sourceFilePaths = Directory.GetFiles(sourceFilesDirectory);
            var sourceFileRegex = new Regex(string.Format(ResxFileRegexPattern, sourceFilesKey));

            foreach (var sourceFilePath in sourceFilePaths)
            {
                var sourceFileName = Path.GetFileName(sourceFilePath);
                var isSourceFile = sourceFileRegex.IsMatch(sourceFileName);
                if (isSourceFile)
                {
                    yield return new ProjectFile(sourceFilePath, sourceFilesDirectory, projectName, null);
                }
            }
        }

        protected override IEnumerable<DocumentControllerDescription> GetSupportedControllers(FileDescriptor modelDescriptor)
        {
            var resxGroupKey = GetResxGroupKey(modelDescriptor);
            if (!_resxDocumentControllers.ContainsKey(resxGroupKey))
            {
                yield return new DocumentControllerDescription("Resx Manager", true, DocumentControllerRole.Source);
            }
        }

        private string GetResxGroupKey(FileDescriptor fileDescriptor)
        {
            var filePath = fileDescriptor.FilePath;
            var fileMatch = ResxFileRegex.Match(Path.GetFileName(filePath));
            var resxGroupKey = fileMatch.Groups[1].Value;
            return resxGroupKey;
        }
    }
}
