﻿using RestWithASPNET.Api.Model;

namespace RestWithASPNET.Api.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
       Person Disable(int id);
    }
}