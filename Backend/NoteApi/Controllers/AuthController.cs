using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using NoteApi.Dto;
using NoteApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NoteApi.Controllers
{
    [ApiController]
    [Route("/api/v1")]
    public class AuthController(UserRepository userRepository) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult?> UserLogin([FromBody] UserRequest request)
        {
            try
            {
                var userRes = await userRepository.GetUser(request.Username, request.Password);

                if (userRes.Data == null)
                    return BadRequest(new
                    {
                        data = new { },
                        success = false,
                        message = "Invalid Credentials"
                    });

                return Ok(new
                {

                    data = userRes.Data,
                    token = userRes.Token,
                    message = "Success",
                    success = true
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    data = new { },
                    success = false,
                    message = "failed"
                }
            );
            }
        }
    }
}
