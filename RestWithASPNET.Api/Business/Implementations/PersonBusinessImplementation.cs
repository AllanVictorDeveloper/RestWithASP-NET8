using RestWithASPNET.Api.Model;
using RestWithASPNET.Api.Repository;

namespace RestWithASPNET.Api.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly IRepository<Person> _repository;

        public PersonBusinessImplementation(IRepository<Person> repository)
        {
            _repository = repository;
        }

        public Person Create(Person person)
        {
            try
            {
                _repository.Create(person);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return person;
        }

        public void Delete(int id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Person> FindAll()
        {
            try
            {
                return _repository.FindAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Person FindById(int id)
        {
            try
            {
                return _repository.FindById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Person Update(Person person)
        {
            try
            {
                _repository.Update(person);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return person;
        }
    }
}