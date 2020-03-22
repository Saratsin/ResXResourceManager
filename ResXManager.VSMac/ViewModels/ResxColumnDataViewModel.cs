using ResXManager.VSMac.Enums;
using ResXManager.VSMac.ViewModels.Abstract;

namespace ResXManager.VSMac.ViewModels
{
    public class ResxColumnDataViewModel : BaseViewModel
    {
        public ResxColumnDataViewModel(string locale, ColumnType type)
        {
            Locale = locale;
            Type = type;
        }

        public string Locale { get; }

        public ColumnType Type { get; }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
    }
}