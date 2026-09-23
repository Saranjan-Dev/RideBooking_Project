using Microsoft.AspNetCore.Mvc;
using RideBooking.API.Services;
using RideBooking.API.DTOs;

namespace RideBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result)
            {
                return Conflict(new
                {
                    message = "Email already exists."
                });
            }
            return Ok(new
            {
                message = "Registration successful."
            });

        }
    }
}
