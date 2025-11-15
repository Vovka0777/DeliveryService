using DeliveryService.Domain.Models;
using System.Collections.Generic;
using System.Security.Claims;


namespace DeliveryService.Domain.Helpers
{

    public class AuthenticateUserHelper
    {
        public static ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!), 
                new Claim(ClaimTypes.Name, user.Login!), 
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("ProfileImage", (user.ProfileImg ?? 0).ToString())
            };

            return new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimTypes.Name,    
                ClaimTypes.Role     
            );
        }
    }
}