using RestWithASPNET.Api.Dto.Request;
using RestWithASPNET.Api.Dto.Response;

namespace RestWithASPNET.Api.Business
{
    public interface ILoginBusiness
    {
        TokenResponse Login(UserRequest user);
        TokenResponse RefreshToken(TokenRequest token);

        bool RevokeToken(string userName);
    }
}