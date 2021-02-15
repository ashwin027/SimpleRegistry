using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace SimpleRegistry.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistryController: ControllerBase
    {
        private const string UserAppId = "UserApp";
        private List<Registry> _registry { get; set; }
        private DaprClient _daprClient { get; set; }
        public RegistryController(List<Registry> registry, DaprClient daprClient)
        {
            _registry = registry;
            _daprClient = daprClient;
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetRegistryByUser([FromRoute] int userId, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _daprClient.InvokeMethodAsync<User>(HttpMethod.Get, UserAppId, $"/api/user/{userId}", cancellationToken);
                var registry = _registry.FirstOrDefault(u => u.UserId == userId);

                if (registry == null)
                {
                    return NotFound("Registry not found.");
                }

                return Ok(registry);
            }
            catch (InvocationException ex)
            {
                switch (ex.Response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return NotFound("User not found.");
                    default:
                        return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Register request)
        {
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
