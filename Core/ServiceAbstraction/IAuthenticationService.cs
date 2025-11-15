using Shared.DataTransferObjects.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        //Login
        //Take Email and Password
        //Return Token , Email, DisplayName
        Task<UserDto> LoginAsync(LoginDto loginDto);


        //Register
        //Take Email, Password, UserName, DisplayName,PhoneNumber
        //Return Token , Email, DisplayName
        Task<UserDto> RegisterAsync(RegisterDto registerDto);


        // Check Email
        //Take Email
        //Return Bool
        Task<bool> CheckEmailAsync(string email);

        // Get Current User
        //Take Email
        //Return UserDto
        Task<UserDto> GetCurrentUserAsync(string email);

        // Get Current User Address
        //Take Email
        //Return AddressDto
        Task<AddressDto> GetCurrentUserAddressAsync(string email);

        // Update Current User Address
        //Take Email, AddressDto
        //Return AddressDto
        Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto);
    }
}
