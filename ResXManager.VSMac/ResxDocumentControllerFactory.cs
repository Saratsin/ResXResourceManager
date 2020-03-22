using MonoDevelop.Ide.Gui.Documents;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ResXManager.VSMac
{
    [ExportDocumentControllerFactory(FileExtension = ".resx")]
    public class ResxDocumentControllerFactory : FileDocumentControllerFactory
    {
        private static readonly Regex ResxFileRegex = new Regex(@"^(.*?)\.((([A-Za-z][A-Za-z](\-[A-Za-z][A-Za-z])?)\.)?)resx$");

        private readonly Dictionary<string, ResxDocumentController> _resxDocumentControllers = new Dictionary<string, ResxDocumentController>();

        public override Task<DocumentController> CreateController(FileDescriptor modelDescriptor, DocumentControllerDescription controllerDescription)
        {
            var resxGroupKey = GetResxGroupKey(modelDescriptor);
            if (!_resxDocumentControllers.TryGetValue(resxGroupKey, out var resxDocumentController))
            {
                resxDocumentController = new ResxDocumentController(resxGroupKey, controller => _resxDocumentControllers.Remove(controller.ResxGroupKey));
                _resxDocumentControllers[resxGroupKey] = resxDocumentController;
            }
            
            return Task.FromResult<DocumentController>(resxDocumentController);
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
