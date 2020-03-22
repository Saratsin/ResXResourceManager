using ResXManager.VSMac.ViewModels.Abstract;
using System.Linq;

namespace ResXManager.VSMac.ViewModels
{
    public class ResxRowViewModel : BaseItemsViewModel<string>
    {
        public ResxRowViewModel()
        {
        }

        private string _key;
        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public string GetValue(int columnIndex)
        {
            if  (columnIndex == 0)
            {
                return Key;
            }

            return this.ElementAtOrDefault(columnIndex);
        }
    }
}