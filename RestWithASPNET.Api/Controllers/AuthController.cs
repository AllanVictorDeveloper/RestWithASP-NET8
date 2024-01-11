using Asp.Versioning;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Api.Business;
using RestWithASPNET.Api.Dto.Request;

namespace RestWithASPNET.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;
        private ILogger<AuthController> _logger;

        public AuthController(ILoginBusiness loginBusiness, ILogger<AuthController> logger)
        {
            _loginBusiness = loginBusiness;
            _logger = logger;
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult Signin([FromBody] UserRequest request)
        {
            try
            {
                if (request is null)
                    return BadRequest("Invalid client request");

                var token = _loginBusiness.Login(request);

                if (token is null)
                    return Unauthorized();

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, "Erro no sistema, tente novamente.");
            }
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                if (tokenRequest is null)
                    return BadRequest("Invalid client request");

                var token = _loginBusiness.RefreshToken(tokenRequest);

                if (token is null)
                    return BadRequest("Invalid client request"); ;

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, "Erro no sistema, tente novamente.");
            }
        }

        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")]
        public IActionResult Revoke()
        {
            try
            {
                var username = User.Identity.Name;

                var result = _loginBusiness.RevokeToken(username);

                if (!result)
                    return BadRequest("Invalid client request"); ;

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, "Erro no sistema, tente novamente.");
            }
        }
    }
}