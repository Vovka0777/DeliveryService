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
        // 1. КРИТИЧЕСКОЕ ИСПРАВЛЕНИЕ: Добавлен уникальный ID
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        
        // 2. Основные клеймы
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(ClaimTypes.Name, user.Login!),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),

        // 3. Пользовательские клеймы
        new Claim("AvatarPath", user.PathImage)
    };

            return new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimTypes.Name, // 4. ИСПРАВЛЕНО: Теперь используется Login (ClaimTypes.Name)
                ClaimsIdentity.DefaultRoleClaimType
            );
        }
    }
}