using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleRegistry.Api.Controllers
{
    
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RegistryController: ControllerBase
    {
        private List<Registry> _registry { get; set; }
        private IUserClient _userClient { get; set; }
        public RegistryController(List<Registry> registry, IUserClient userClient)
        {
            _registry = registry;
            _userClient = userClient;
        }

        
        [HttpGet("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Registry>> GetRegistryByUser([FromRoute] int userId)
        {
            // Verify if the buyer user id is valid
            var buyer = await _userClient.GetUserDetailsAsync(userId);
            if (buyer == null)
            {
                return NotFound("User not found.");
            }

            var registry = _registry.FirstOrDefault(u => u.UserId == userId);

            if (registry == null)
            {
                return NotFound();
            }

            return Ok(registry);
        }

        [HttpPut("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Register([FromBody] Register request)
        {
            // Verify if the buyer user id is valid
            var buyer = await _userClient.GetUserDetailsAsync(request.BuyerUserId);
            if (buyer == null)
            {
                return NotFound("Buyer not found.");
            }
            // Get the registry 
            var registry = _registry.FirstOrDefault(r => r.Id == request.RegistryId);

            if (registry == null)
            {
                return NotFound();
            }

            registry.BuyerUserId = request.BuyerUserId;

            return Ok();
        }
    }
}
