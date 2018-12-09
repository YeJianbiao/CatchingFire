
using System.Collections.ObjectModel;
using CatchingFire.ViewModel;
using GalaSoft.MvvmLight.CommandWpf;

namespace CatchingFire.Area.TechnologySummarize.ViewModel
{
    public class ListBoxPagingViewModel : BaseViewModel<object>
    {

        public override void Add(object t)
        {
            Collection = new ObservableCollection<object>();
            for (int i = 0; i < 100; i++)
            {
                Collection.Add(i);
            }
        }

        private int _pageCount;

        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                _pageCount = value;
                RaisePropertyChanged("PageCount");
                Pages.Clear();
                for (int i = 0; i < _pageCount; i++)
                {
                    var pageModel = new PageModel
                    {
                        Index = i + 1,
                        IsSelected = i == 0
                    };
                    Pages.Add(pageModel);
                }
            }
        }

        public int CurrentPage { get; set; } = 0;

        
        
    }

}
