using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Api.Dto.Request;
using RestWithASPNET.Api.Model;
using RestWithASPNET.Api.Repository;

namespace RestWithASPNET.Api.Business.Implementations
{
    public class PersonBusiness : IPersonBusiness
    {
        private readonly IPersonRepository _repository;

        public PersonBusiness(IPersonRepository repository)
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
                var persons = _repository.FindAll();

                return persons;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public PageSearchRequest<Person> FindWithPagedSearch(string? name, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection)) && !sortDirection.Equals("desc") ? "asc" : "desc";
            var size = (page < 1) ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            string query = @"SELECT * FROM rest_asp_net_db.dbo.persons as p";


            if (!string.IsNullOrWhiteSpace(name)) query = query + $" WHERE p.FirstName LIKE '%{name}%'";

            query += $" ORDER BY p.FirstName {sort} OFFSET {offset} ROWS FETCH NEXT {size} ROWS ONLY";

            string countQuery = @"SELECT count(*) FROM rest_asp_net_db.dbo.persons as p WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(name)) countQuery = countQuery + $" AND p.FirstName LIKE '%{name}%'";

            var persons = _repository.FindWithPagedSearch(query);
            int total = _repository.GetCount(countQuery);

            return new PageSearchRequest<Person>
            {
                CurrentPage = page,
                List = persons,
                PageSize = size,
                SortDirection = sort,
                TotalResults = total
            };
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

        public List<Person> FindByName([FromQuery] string firstName, [FromQuery] string lastName)
        {
            return _repository.FindByName(firstName, lastName);
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

        public Person Disable(int id)
        {
            var personEntity = _repository.Disable(id);

            return personEntity;
        }
    }
}