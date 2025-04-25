using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperNotesBackend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SuperNotesBackend.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Login with just username
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                return BadRequest("Username is required");
            }

            // Create the claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim("UserId", request.Username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            // Sign in the user
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Also set the session (as backup)
            HttpContext.Session.SetString("User", request.Username);

            return Ok(new { message = "Logged in successfully", username = request.Username });
        }

        // Logout user
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            // Clear session
            HttpContext.Session.Clear();
            
            return Ok("Logged out successfully.");
        }

        // Check if user is authenticated
        [HttpGet("check")]
        [Authorize]
        public IActionResult CheckAuth()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Not authenticated");
            }
            return Ok(new { username = username });
        }
    }
    
    public class LoginRequest
    {
        public string Username { get; set; }
    }
}
