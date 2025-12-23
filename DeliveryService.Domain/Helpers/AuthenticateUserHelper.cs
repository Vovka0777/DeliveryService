using DeliveryService.Domain.Models;
using System.Security.Claims;


namespace DeliveryService.Domain.Helpers
{

    public class AuthenticateUserHelper
    {
        public static ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim> 
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(ClaimTypes.Name, user.Login!),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),

        new Claim("AvatarPath", user.PathImage),
        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
    };

            return new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimTypes.Name, 
                ClaimsIdentity.DefaultRoleClaimType
            );
        }
    }
}