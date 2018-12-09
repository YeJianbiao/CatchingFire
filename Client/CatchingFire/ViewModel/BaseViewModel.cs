using System.Collections.ObjectModel;

namespace CatchingFire.ViewModel
{
    public class BaseViewModel<T> : NotificationObject where T : class, new()
    {

        public ObservableCollection<T> Collection { get; set; }

        public ObservableCollection<PageModel> Pages { get; set; }

        public virtual void Add(T t)
        {
            Collection = new ObservableCollection<T> {t};
        }

        public virtual void Delete()
        {

        }

        public virtual void Save()
        {

        }

        public virtual void Search()
        {

        }
    }

    public class PageModel : NotificationObject
    {
        public int Index { get; set; }

        public bool IsSelected { get; set; }
    }
}
