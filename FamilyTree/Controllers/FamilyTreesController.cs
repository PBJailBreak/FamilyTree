using Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyTreesController : ControllerBase
    {
        private readonly IFamilyTreeService familyTreeService;

        public FamilyTreesController(IFamilyTreeService familyTreeService)
        {
            this.familyTreeService = familyTreeService;
        }

        [HttpGet]
        [Route("{personId}")]
        public async Task<IActionResult> GetFamilyTree([FromRoute] int personId)
        {
            return Ok(await this.familyTreeService.GetAsync(personId));
        }
    }
}