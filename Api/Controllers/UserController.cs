using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //Create User
        [HttpPost("addUser")]
        public async Task<IActionResult> Create(User user)
        {
            var result = await _userRepository.CreateAsync(user);
            if (result)
            {
                return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
            }
            else
            {
                return BadRequest();
            }
        }

        //Get All
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetUsersAll()
        {
            var users = await _userRepository.GetAllAsync();
            if (!users.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(users);
            }
        }

        //GetUserById
        [HttpGet("getUserById/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        //Update
        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var result = await _userRepository.UpdateAsync(user);
            if(result)
            {
                return Ok();
            }else
            {
                return NotFound();
            }
        }

        //Delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
