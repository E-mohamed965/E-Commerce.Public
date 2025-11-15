using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace presentation.Controllers
{
    public class AuthenticationController(IServiceManager _serviceManager) : ApiBaseController
    {
        //Login 
        [HttpPost("login")] // POST : baseUrl/api/authentication/login
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginRequest)
        {
            // Implementation for user login
            var userDto= await _serviceManager.AuthenticationService.LoginAsync(loginRequest);
            return Ok(userDto);
        }

        //Register
        [HttpPost("register")] // POST : baseUrl/api/authentication/register
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerRequest)
        {
            // Implementation for user registration
            var userDto= await _serviceManager.AuthenticationService.RegisterAsync(registerRequest);
            return Ok(userDto);
        }

        //Check Email
        [HttpGet("CheckEmail")]// GET : baseUrl/api/authentication/CheckEmail
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await _serviceManager.AuthenticationService.CheckEmailAsync(email);
            return Ok(result);
        }
        //Get Current User
        [Authorize]
        [HttpGet("CurrentUser")] // GET : baseUrl/api/authentication/CurrentUser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user= await _serviceManager.AuthenticationService.GetCurrentUserAsync(email!);
            return Ok(user);
        }
        //Get Current User Address
        [Authorize]
        [HttpGet("Address")] // GET : baseUrl/api/authentication/Address
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address= await _serviceManager.AuthenticationService.GetCurrentUserAddressAsync(email!);
            return Ok(address);
        }
        //Update Current User Address
        [Authorize]
        [HttpPut("Address")] // PUT : baseUrl/api/authentication/Address
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto addressDto)
        {

            var email = User.FindFirstValue(ClaimTypes.Email);
            var updatedAddress = await _serviceManager.AuthenticationService.UpdateCurrentUserAddressAsync(email!, addressDto);
            return Ok(updatedAddress);
        }
    }
}
