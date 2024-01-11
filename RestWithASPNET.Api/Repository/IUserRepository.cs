using RestWithASPNET.Api.Model;

namespace RestWithASPNET.Api.Repository
{
    public interface IUserRepository
    {
        User? ValidateCredentials(User request);

        User? ValidateCredentials(string userName);

        bool RevokeToken(string userName);

        User? RefreshUserInfo(User user);
    }
}