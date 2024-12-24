using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserManagementController : ControllerBase
	{
		private static List<User> users = new List<User>
		{
			new User(1, "John"),
			new User(2, "Jane"),
			new User(3, "Doe")
		};

		[HttpGet]
		[Route("GetUser")]
		public ActionResult<List<User>> GetUser()
		{
			try
			{
				return Ok(users);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet]
		[Route("GetUserById/{id}")]
		public ActionResult<User> GetUserById(int id)
		{
			try
			{
				var user = users.FirstOrDefault(x => x.Id == id);
				if (user == null)
				{
					return NotFound("User not found");
				}
				return Ok(user);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPost]
		[Route("AddUser")]
		public IActionResult AddUser([FromBody] User newUser)
		{
			try
			{
				if (newUser == null || string.IsNullOrEmpty(newUser.Name))
				{
					return BadRequest("Invalid user data.");
				}

				if (users.Any(x => x.Id == newUser.Id))
				{
					return BadRequest("User already exists");
				}

				users.Add(newUser);
				return Ok("User added");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPost]
		[Route("UpdateUser")]
		public IActionResult UpdateUser(int userId, [FromBody] User updatedUser)
		{
			try
			{
				if (updatedUser == null || string.IsNullOrEmpty(updatedUser.Name))
				{
					return BadRequest("Invalid user data.");
				}

				var user = users.FirstOrDefault(x => x.Id == userId);
				if (user == null)
				{
					return NotFound("User not found");
				}

				user.Name = updatedUser.Name;
				return Ok("User updated");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpDelete]
		[Route("DeleteUser")]
		public IActionResult DeleteUser(int userId)
		{
			try
			{
				var user = users.FirstOrDefault(x => x.Id == userId);
				if (user == null)
				{
					return NotFound("User not found");
				}

				users.Remove(user);
				return Ok("User deleted");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
