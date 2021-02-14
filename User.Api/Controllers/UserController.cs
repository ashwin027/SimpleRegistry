using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {
        private List<User> _users { get; set; }
        public UserController(List<User> users)
        {
            _users = users;
        }

        [HttpGet("{userId:int}")]
        public IActionResult GetUserDetails([FromRoute] int userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
