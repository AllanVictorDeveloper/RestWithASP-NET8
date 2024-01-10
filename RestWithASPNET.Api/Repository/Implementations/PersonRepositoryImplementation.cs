using RestWithASPNET.Api.Model;
using RestWithASPNET.Api.Model.Context;

namespace RestWithASPNET.Api.Repository.Implementations
{
    public class PersonRepositoryImplementation : IPersonRepository, IRepository<Person>
    {
        private readonly Context _context;

        public PersonRepositoryImplementation(Context context)
        {
            _context = context;
        }

        public Person Create(Person person)
        {
            try
            {
                _context.Add(person);
                _context.SaveChanges();
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
                var person = FindById(id);
                _context.Persons.Remove(person);
                _context.SaveChanges();
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
                return _context.Persons.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Person? FindById(int id)
        {
            try
            {
                var person = _context.Persons.SingleOrDefault(x => x.Id.Equals(id));

                return person;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Person? Update(Person person)
        {
            try
            {
                if (!Exists(person.Id))
                    return null;

                _context.Persons.Update(person);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return person;
        }

        public bool Exists(int id)
        {
            var person = _context.Persons.SingleOrDefault(x => x.Id.Equals(id));

            if (person is null)
            {
                return false;
            }

            return true;
        }
    }
}