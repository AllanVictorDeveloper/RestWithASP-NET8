using RestWithASPNET.Api.Model.Base;

namespace RestWithASPNET.Api.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T obj);

        T Update(T obj);

        List<T> FindAll();

        T FindById(int id);

        void Delete(int id);

        bool Exists(int id);

        List<T> FindWithPagedSearch(string query);

        int GetCount(string query);
    }
}