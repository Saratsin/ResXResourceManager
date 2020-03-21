using MonoDevelop.Components;
using MonoDevelop.Ide.Gui.Documents;
using System.Threading;
using System.Threading.Tasks;

namespace ResXManager.VSMac
{
    public class ResxDocumentController : FileDocumentController
	{
        protected override Control OnGetViewControl(DocumentViewContent view)
        {
            return new XwtControl(new Xwt.TextEntry
            {
                Text = "Extension development is possible, йопта)",
                BackgroundColor = Xwt.Drawing.Color.FromBytes(0, 255,255),
                VerticalPlacement = Xwt.WidgetPlacement.Center,
                HorizontalPlacement = Xwt.WidgetPlacement.Center
            });
        }
	}
}