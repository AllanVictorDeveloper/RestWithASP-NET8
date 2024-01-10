using RestWithASPNET.Api.Model;

namespace RestWithASPNET.Api.Business
{
    public interface IPersonBusiness
    {
        Person Create(Person person);

        Person Update(Person person);

        List<Person> FindAll();

        Person FindById(int id);

        void Delete(int id);
    }
}