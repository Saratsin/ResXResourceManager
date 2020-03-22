using ResXManager.VSMac.ViewModels.Abstract;
using System;
using System.Collections.ObjectModel;

namespace ResXManager.VSMac.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            List = new ResxListViewModel();
        }

        public ResxListViewModel List { get; }
    }
}
