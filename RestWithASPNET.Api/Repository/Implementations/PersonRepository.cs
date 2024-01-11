using RestWithASPNET.Api.Model;
using RestWithASPNET.Api.Model.Context;
using RestWithASPNET.Api.Repository.Generic;

namespace RestWithASPNET.Api.Repository.Implementations
{
    public class PersonRepository : GenericRespository<Person>, IPersonRepository
    {
        public PersonRepository(Context context) : base(context)
        {
        }

        public Person Disable(int id)
        {
            if (!_context.Persons.Any(p => p.Id.Equals(id))) return null;

            var user = _context.Persons.SingleOrDefault(p => p.Id.Equals(id));

            if (user is not null)
            {
                user.Enabled = false;

                try
                {
                    _context.Entry(user).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
                return user;
        }
    }
}