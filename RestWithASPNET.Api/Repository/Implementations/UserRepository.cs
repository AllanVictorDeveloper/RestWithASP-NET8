using RestWithASPNET.Api.Model;
using RestWithASPNET.Api.Model.Context;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNET.Api.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public User? ValidateCredentials(User user)
        {
            var pass = ComputeHash(user.Password, SHA256.Create());

            return _context.Users.FirstOrDefault(u => u.UserName.Equals(user.UserName) && (u.Password.Equals(pass)));
        }

        public User? ValidateCredentials(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName.Equals(userName));
        }

        public User? RefreshUserInfo(User user)
        {
            if (!_context.Users.Any(p => p.Id.Equals(user.Id)))
                return null;

            var result = _context.Users.SingleOrDefault(x => x.Id.Equals(user.Id));

            if (result is not null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return result;
        }

        public bool RevokeToken(string userName)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName.Equals(userName));

            if (user is null) return false;

            user.RefreshToken = null;
            _context.SaveChanges();

            return true;
        }

        private string ComputeHash(string? password, HashAlgorithm hashAlgorithm)
        {
            byte[] hashedBytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));

            var builder = new StringBuilder();

            foreach (var item in hashedBytes)
            {
                builder.Append(item.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}