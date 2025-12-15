using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Domain.Entity;
using UserManagement.Infrastructure;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IValidator<User> _validator;


        public UsersController(ILogger<UsersController> logger, IValidator<User> validator)
        {
            _logger = logger;
            _validator = validator;
        }


        [HttpGet]
        public IActionResult GetAll(string? firstName, string? lastName)
        {
            var users = FileStorage.ReadUsers(_logger);
            if (!string.IsNullOrEmpty(firstName)) users = users.Where(u => u.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrEmpty(lastName)) users = users.Where(u => u.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(users);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var users = FileStorage.ReadUsers(_logger);
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound(new { status = 404, message = "User not found." });
            return Ok(user);
        }


        [HttpPost]
        public IActionResult Create(User user)
        {
            var users = FileStorage.ReadUsers(_logger);
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            FileStorage.WriteUsers(users, _logger);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, User updatedUser)
        {
            var validationResult = _validator.Validate(updatedUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "Validation failed.",
                    errors = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }


            var users = FileStorage.ReadUsers(_logger);
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound(new { status = 404, message = "User not found." });


            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Age = updatedUser.Age;


            FileStorage.WriteUsers(users, _logger);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var users = FileStorage.ReadUsers(_logger);
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound(new { status = 404, message = "User not found." });


            users.Remove(user);
            FileStorage.WriteUsers(users, _logger);
            return NoContent();
        }

        [HttpGet("/error-demo")]
        public IActionResult ErrorDemo() => throw new Exception("Simulated unhandled exception");
    }
}