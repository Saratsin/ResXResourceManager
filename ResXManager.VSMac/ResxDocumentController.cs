﻿using JetBrains.Annotations;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Documents;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;

namespace ResXManager.VSMac
{
    public class ResxDocumentController : DocumentController
	{
        private readonly Action<ResxDocumentController> _onClosedAction;

        private string[] _collection =
        {
            "Часом буває так, що хочеш, почути",
            "Речі, яких ніколи б, не знати",
            "І тягне за руку, тебе в то, місце",
            "Де думаєш, краще б, очей не мати",
            "Ти дивишся в кухні, на кран, і на воду",
            "А правда, нізвідки, не виходить",
            "І дивляться, в очі, тобі знайомі",
            "І їхні, очі, тобі говорять",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Часом буває, так, що в магазині",
            "Ти хочеш, крикнути, Ну в чому я винна",
            "Тебе окидають, розуміючим, оком",
            "Що дуже болить, хоча й, ненароком",
            "І ти ростеш, старієш, вмираєш",
            "А тої, правди, так і не знаєш",
            "Боїшся її, і від неї, втікаєш",
            "І в стінах, своїх ти одна, засинаєш",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Часом буває так, що хочеш, почути",
            "Речі, яких ніколи б, не знати",
            "І тягне за руку, тебе в то, місце",
            "Де думаєш, краще б, очей не мати",
            "Ти дивишся в кухні, на кран, і на воду",
            "А правда, нізвідки, не виходить",
            "І дивляться, в очі, тобі знайомі",
            "І їхні, очі, тобі говорять",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Часом буває, так, що в магазині",
            "Ти хочеш, крикнути, Ну в чому я винна",
            "Тебе окидають, розуміючим, оком",
            "Що дуже болить, хоча й, ненароком",
            "І ти ростеш, старієш, вмираєш",
            "А тої, правди, так і не знаєш",
            "Боїшся її, і від неї, втікаєш",
            "І в стінах, своїх ти одна, засинаєш",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Часом буває так, що хочеш, почути",
            "Речі, яких ніколи б, не знати",
            "І тягне за руку, тебе в то, місце",
            "Де думаєш, краще б, очей не мати",
            "Ти дивишся в кухні, на кран, і на воду",
            "А правда, нізвідки, не виходить",
            "І дивляться, в очі, тобі знайомі",
            "І їхні, очі, тобі говорять",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Часом буває, так, що в магазині",
            "Ти хочеш, крикнути, Ну в чому я винна",
            "Тебе окидають, розуміючим, оком",
            "Що дуже болить, хоча й, ненароком",
            "І ти ростеш, старієш, вмираєш",
            "А тої, правди, так і не знаєш",
            "Боїшся її, і від неї, втікаєш",
            "І в стінах, своїх ти одна, засинаєш",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Часом буває так, що хочеш, почути",
            "Речі, яких ніколи б, не знати",
            "І тягне за руку, тебе в то, місце",
            "Де думаєш, краще б, очей не мати",
            "Ти дивишся в кухні, на кран, і на воду",
            "А правда, нізвідки, не виходить",
            "І дивляться, в очі, тобі знайомі",
            "І їхні, очі, тобі говорять",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Часом буває, так, що в магазині",
            "Ти хочеш, крикнути, Ну в чому я винна",
            "Тебе окидають, розуміючим, оком",
            "Що дуже болить, хоча й, ненароком",
            "І ти ростеш, старієш, вмираєш",
            "А тої, правди, так і не знаєш",
            "Боїшся її, і від неї, втікаєш",
            "І в стінах, своїх ти одна, засинаєш",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема",
            "Спи, собі, сама",
            "Коли, біля тебе, мене нема"
        };

        protected override Task OnLoad(bool reloading)
        {
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

            var firstField = new DataField<string>();
            var secondField = new DataField<string>();
            var thirdField = new DataField<string>();

            var source = new ListStore(firstField, secondField, thirdField);
            foreach(var rowString in _collection)
            {
                var index = source.AddRow();
                var rowItems = rowString.Split(',');
                source.SetValue(index, firstField, rowItems[0]);
                source.SetValue(index, secondField, rowItems[1]);
                source.SetValue(index, thirdField, rowItems[2]);
            }

            var listView = new ListView(source)
            {
                SelectionMode = SelectionMode.None,
                Columns =
                {
                    new ListViewColumn("Ein", new TextCellView(firstField) { Editable = true })
                    {
                        CanResize = true
                    },
                    new ListViewColumn("Zwei", new TextCellView(secondField) { Editable = true })
                    {
                        CanResize = true
                    },
                    new ListViewColumn("Drei", new TextCellView(thirdField) { Editable = true })
                    {
                        CanResize = true
                    }
                }
            };

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