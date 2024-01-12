using RestWithASPNET.Api.Dto.Request;
using RestWithASPNET.Api.Model;

namespace RestWithASPNET.Api.Business
{
    public interface IPersonBusiness
    {
        Person Create(Person person);

        Person Update(Person person);

        List<Person> FindAll();

        PageSearchRequest<Person> FindWithPagedSearch(string? name, string sortDirection, int pageSize, int page);

        Person FindById(int id);

        List<Person> FindByName(string firstName, string lastName);

        void Delete(int id);

        Person Disable(int id);
    }
}