using Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyTreesController : ControllerBase
    {
        private readonly IFamilyTreeService familyTreeService;
        private readonly IMemoryCache cache;

        public FamilyTreesController(IFamilyTreeService familyTreeService, IMemoryCache cache)
        {
            this.familyTreeService = familyTreeService;
            this.cache = cache;
        }

        [HttpGet]
        [Route("{personId}")]
        public async Task<IActionResult> GetFamilyTree([FromRoute] int personId)
        {
            var familyTreeFromCache = GetFamilyTreeFromCache(personId);

            if (familyTreeFromCache != null)
            {
                return Ok(familyTreeFromCache);
            }

            var familyTree = await this.familyTreeService.GetAsync(personId);
            this.PutFamilyTreeIntoCache(personId, familyTree);

            return Ok(familyTree);
        }

        private Core.DTOs.Person GetFamilyTreeFromCache(int personId)
        {
            return (Core.DTOs.Person) this.cache.Get(personId);
        }

        private void PutFamilyTreeIntoCache(int personId, Core.DTOs.Person familyTree)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            this.cache.Set(personId, familyTree, cacheEntryOptions);
        }
    }
}