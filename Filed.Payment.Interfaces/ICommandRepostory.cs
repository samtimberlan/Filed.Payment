using System.Threading.Tasks;

namespace Filed.Payment.Interfaces
{
    public interface ICommandRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        Task<int> SaveAsync();
        void Update(T entity);
    }
}
