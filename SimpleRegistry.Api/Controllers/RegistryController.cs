using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SimpleRegistry.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistryController: ControllerBase
    {
        private List<Registry> _registry { get; set; }
        public RegistryController(List<Registry> registry)
        {
            _registry = registry;
        }

        [HttpGet("{userId:int}")]
        public IActionResult GetRegistryByUser([FromRoute] int userId)
        {
            var registry = _registry.FirstOrDefault(u => u.UserId == userId);

            if (registry == null)
            {
                return NotFound();
            }

            return Ok(registry);
        }
    }
}
