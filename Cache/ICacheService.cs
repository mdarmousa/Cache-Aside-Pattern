using System;
using System.Threading.Tasks;

namespace Cache_Aside_Pattern.Cache
{
    public interface ICacheService
    {

        void Add<TItem>(TItem item, string key);

        TItem Get<TItem>(string key) where TItem : class;
    }
}
