
namespace Utility.Cache
{
    public interface ICache
    {

        bool Contains(string key);

        void Add(string key, object target, int time);

        T Get<T>(string key) where T : class;

        void Remove(string key);

        void Clear();

    }
}
