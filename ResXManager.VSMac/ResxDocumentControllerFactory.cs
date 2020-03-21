using MonoDevelop.Ide.Gui.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResXManager.VSMac
{
    [ExportDocumentControllerFactory(FileExtension = ".resx")]
    public class ResxDocumentControllerFactory : FileDocumentControllerFactory
    {
        public ResxDocumentControllerFactory()
        {
        }

        public override async Task<DocumentController> CreateController(FileDescriptor modelDescriptor, DocumentControllerDescription controllerDescription)
        {
            return new ResxDocumentController();
        }

        protected override IEnumerable<DocumentControllerDescription> GetSupportedControllers(FileDescriptor modelDescriptor)
        {
            yield return new DocumentControllerDescription("Resx Manager", true, DocumentControllerRole.Source);
        }
    }
}
