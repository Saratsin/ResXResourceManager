using JetBrains.Annotations;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Documents;
using ResXManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;

namespace ResXManager.VSMac
{
    public class ResxDocumentController : DocumentController
	{
        private readonly Action<ResxDocumentController> _onClosedAction;

        private List<List<string>> _data;

        protected override Task OnLoad(bool reloading)
        {
            var resourceManager = Extension.Instance.Container.GetExportedValue<ResourceManager>();
            resourceManager.Reload();

            var entries = resourceManager.TableEntries;
            var cultures = resourceManager.Cultures;

            _data = entries.Select(entry =>
            {
                return cultures.Select(culture =>
                {
                    return entry.Values[culture.Culture?.Name];

                }).Prepend(entry.Key).ToList();
            })
            .Prepend(cultures.Select(culture => culture.Culture?.DisplayName ?? "Neutral").Prepend("Key").ToList())
            .ToList();

            return base.OnLoad(reloading);
        }

        public ResxDocumentController([NotNull] string resxGroupKey, [NotNull] Action<ResxDocumentController> onClosedAction)
        {
            ResxGroupKey = resxGroupKey;
            DocumentTitle = resxGroupKey;
            _onClosedAction = onClosedAction;
        }

        public string ResxGroupKey { get; }

        protected override Control OnGetViewControl(DocumentViewContent view)
        {
            var mainBox = new VBox();
            var toolBox = new HBox()
            {
                BackgroundColor = new Color(1, 0, 0),
                HeightRequest = 50
            };
            mainBox.PackStart(toolBox);

            var dataEnumerator = _data.GetEnumerator();
            dataEnumerator.MoveNext();
            var fieldsData = dataEnumerator.Current;
            var fields = fieldsData.Select(fieldData => new DataField<string>()).ToArray();
            var source = new ListStore(fields);
            while (dataEnumerator.MoveNext())
            {
                var rowData = dataEnumerator.Current;
                var rowIndex = source.AddRow();
                var columnIndex = 0;
                foreach(var field in fields)
                {
                    source.SetValue(rowIndex, field, rowData[columnIndex++]);
                }
            }

            var listView = new ListView(source) { SelectionMode = SelectionMode.None,  ExpandHorizontal = false };

            for (var i = 0; i < fieldsData.Count; ++i)
            {
                var cellView = new TextCellView(fields[i]) { Editable = true};
                var listViewColumn = new ListViewColumn(fieldsData[i], cellView) { CanResize = true };
                listView.Columns.Add(listViewColumn);
            }

            mainBox.PackStart(listView, true, true);
            return new XwtControl(mainBox);
        }

        protected override Task OnInitialize(ModelDescriptor modelDescriptor, Properties status)
        {
            return base.OnInitialize(modelDescriptor, status);
        }

        protected override void OnClosed()
        {
            _onClosedAction.Invoke(this);
            base.OnClosed();
        }
    }
}