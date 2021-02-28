using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleRegistry.Api.Controllers
{

    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RegistryController : ControllerBase
    {
        private List<Registry> _registry { get; set; }
        private IUserClient _userClient { get; set; }
        public RegistryController(List<Registry> registry, IUserClient userClient)
        {
            _registry = registry;
            _userClient = userClient;
        }

        [HttpPut("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] Register request)
        {
            try
            {
                // Verify if the buyer user id is valid
                await _userClient.GetUserDetailsAsync(request.BuyerUserId);

                // Get the registry 
                var registry = _registry.FirstOrDefault(r => r.Id == request.RegistryId);

                if (registry == null)
                {
                    return NotFound();
                }

                registry.BuyerUserId = request.BuyerUserId;

                return Ok();
            }
            catch (ApiException ex)
            {
                switch ((HttpStatusCode)ex.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound("User not found.");
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Registry>> GetRegistryByUser([FromRoute] int userId)
        {
            try
            {
                // Verify if the user id is valid
                await _userClient.GetUserDetailsAsync(userId);

                var registry = _registry.FirstOrDefault(u => u.UserId == userId);

                if (registry == null)
                {
                    return NotFound();
                }

                return Ok(registry);
            }
            catch (ApiException ex)
            {
                switch ((HttpStatusCode)ex.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound("User not found.");
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
