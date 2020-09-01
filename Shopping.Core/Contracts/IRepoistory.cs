using Shopping.Core.Models;
using System.Linq;

namespace Shopping.Core.Contracts
{
    public interface IRepoistory<T> where T : BaseEntity
    {
        IQueryable<T> Collection();
        void commit();
        void Delete(string Id);
        T Find(string Id);
        void Insert(T t);
        void Update(T t);
    }
}