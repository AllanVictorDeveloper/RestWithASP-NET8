using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Api.Model.Base;
using RestWithASPNET.Api.Model.Context;

namespace RestWithASPNET.Api.Repository.Generic
{
    public class GenericRespository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly Context _context;

        private DbSet<T> dataset;

        public GenericRespository(Context context)
        {
            _context = context;
            dataset = _context.Set<T>();
        }

        public T Create(T obj)
        {
            try
            {
                dataset.Add(obj);
                _context.SaveChanges();
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public T Update(T obj)
        {
            var result = dataset.SingleOrDefault(x => x.Id == obj.Id);

            if (result is not null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(obj);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return null;
            }
        }

        public List<T> FindAll()
        {
            return dataset.ToList();
        }

        public T FindById(int id)
        {
            return dataset.SingleOrDefault(x => x.Id.Equals(id));
        }

        public void Delete(int id)
        {
            try
            {
                var result = dataset.SingleOrDefault(x => x.Id == id);

                if (result is not null)
                {
                    dataset.Remove(result);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Exists(int id)
        {
            return dataset.Any(p => p.Id == id);
        }
    }
}