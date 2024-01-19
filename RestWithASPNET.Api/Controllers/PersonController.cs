using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Api.Business;
using RestWithASPNET.Api.Model;

namespace RestWithASPNET.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonBusiness _personBusiness;

        public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        [ProducesResponseType((200), Type = typeof(List<Person>))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult Get([FromQuery] string? name, string sortDirection, int pageSize, int page)
        {

            var persons = _personBusiness.FindWithPagedSearch(name, sortDirection, pageSize, page);
            return Ok(persons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(Person))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult Get(int id)
        {
            var person = _personBusiness.FindById(id);

            if (person is null)
                return NotFound();

            return Ok(person);
        }

        [HttpGet("findPersonByName")]
        [ProducesResponseType((200), Type = typeof(Person))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult findPersonByName(string firstName, string lastName)
        {
            var person = _personBusiness.FindByName(firstName, lastName);

            if (person is null || person.Count == 0)
                return NotFound("Nenhuma pessoa encontrada com esses parametros.");

            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType((200), Type = typeof(Person))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult Post([FromBody] Person person)
        {
            if (person is null)
                return BadRequest();

            return Ok(_personBusiness.Create(person));
        }

        [HttpPut]
        [ProducesResponseType((200), Type = typeof(Person))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult Put([FromBody] Person person)
        {
            if (person is null)
                return BadRequest();

            return Ok(_personBusiness.Update(person));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType((200), Type = typeof(Person))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult Patch(int id)
        {
            var person = _personBusiness.Disable(id);

            return Ok(person);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        public IActionResult Delete(int id)
        {
            _personBusiness.Delete(id);

            return NoContent();
        }
    }
}