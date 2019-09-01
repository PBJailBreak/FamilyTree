using Core.Contracts.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService service;
        private readonly IBackgroundJobClient backgroundJobs;

        public PeopleController(IPeopleService service, IBackgroundJobClient backgroundJobs)
        {
            this.service = service;
            this.backgroundJobs = backgroundJobs;
        }

        [HttpPost]
        public ActionResult AddPerson([FromBody] Core.DTOs.Person person)
        {
            this.backgroundJobs.Enqueue(() => this.service.UpsertAsync(person));
            return Ok();
        }

        [Route("{personId}/children")]
        [HttpPost]
        public IActionResult AddChild([FromRoute] int personId, [FromBody] Core.DTOs.Person person)
        {
            this.backgroundJobs.Enqueue(() => this.service.UpsertChildAsync(personId, person));

            return Ok();
        }
    }
}