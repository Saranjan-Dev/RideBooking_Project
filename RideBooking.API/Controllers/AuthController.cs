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

            if (!result.Success)
            {
                if (result.Message == "Email already exists.")
                {
                    return Conflict(new
                    {
                        message = result.Message
                    });
                }

                return StatusCode(500, new
                {
                    message = result.Message
                });
            }
            return Ok(new
            {
                message = result.Message
            });

        }
    }
}
