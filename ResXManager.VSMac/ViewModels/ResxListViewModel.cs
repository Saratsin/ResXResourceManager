using DocumentFormat.OpenXml.Wordprocessing;
using ResXManager.Model;
using ResXManager.VSMac.ViewModels.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xwt;

namespace ResXManager.VSMac.ViewModels
{
    public class ResxListViewModel : BaseItemsViewModel<ResxRowViewModel>, IListDataSource
    {
        public ResxListViewModel()
        {
        }

        public event EventHandler<ListRowEventArgs> RowInserted;
        public event EventHandler<ListRowEventArgs> RowDeleted;
        public event EventHandler<ListRowEventArgs> RowChanged;
        public event EventHandler<ListRowOrderEventArgs> RowsReordered;

        public IReadOnlyList<ResxColumnDataViewModel> LocalizationColumns { get; }

        public int RowCount => Count;

        public Type[] ColumnTypes => Enumerable.Repeat(typeof(string), ColumnCount).ToArray();

        private IEnumerable<ResxColumnDataViewModel> EnabledLocalizationColumns => LocalizationColumns.Where(column => column.IsEnabled);

        private int ColumnCount => EnabledLocalizationColumns.Count() + 1;

        public object GetValue(int row, int column)
        {
            var rowViewModel = this.ElementAtOrDefault(row);
            if (rowViewModel is null)
            {
                return null;
            }

            return rowViewModel.GetValue(column);
        }

        public void SetValue(int row, int column, object value)
        {
            //var entry = default(ResourceTableEntry);
            //entry.CodeReferences.Count();
            //entry.Comment.Count();
            //entry.CommentAnnotations.Count();
            //entry.Comments.Count();
            //entry.NeutralLanguage.
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            switch (e.PropertyName)
            {
                case nameof(Count):
                    RaisePropertyChanged(nameof(RowCount));
                    break;
            }
        }

        protected override void InsertItem(int index, ResxRowViewModel item)
        {
            base.InsertItem(index, item);
            var eventArgs = new ListRowEventArgs(index);
            RowInserted?.Invoke(this, eventArgs);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            var eventArgs = new ListRowEventArgs(index);
            RowDeleted?.Invoke(this, eventArgs);
        }
    }
}
