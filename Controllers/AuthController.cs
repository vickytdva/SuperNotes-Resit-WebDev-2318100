using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperNotesBackend.Models;

namespace SuperNotesBackend.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Simulate user login (this can be extended to check with a database)
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "test" && request.Password == "password")  // Simulated check
            {
                // Create session
                HttpContext.Session.SetString("User", request.Username);
                return Ok("Logged in successfully.");
            }
            return Unauthorized("Invalid credentials.");
        }

        // Logout user and clear session
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  // Clear session data
            return Ok("Logged out successfully.");
        }
    }
    
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
