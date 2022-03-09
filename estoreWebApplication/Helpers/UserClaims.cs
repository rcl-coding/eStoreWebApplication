using estoreWebApplication.Models;
using System.Security.Claims;

namespace estoreWebApplication.Helpers
{
    public static class UserClaims
    {
        public static UserViewModel GetUserClaims(ClaimsPrincipal user)
        {
            UserViewModel userVM = new UserViewModel();
            var claims = user.Claims;

            string stAddress = claims?.FirstOrDefault(x => x.Type.Equals("streetAddress", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
            string city = claims?.FirstOrDefault(x => x.Type.Equals("city", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
            string state = claims?.FirstOrDefault(x => x.Type.Equals("state", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
            string country = claims?.FirstOrDefault(x => x.Type.Equals("country", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;

            userVM.Name = claims?.FirstOrDefault(x => x.Type.Equals("name", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
            userVM.Email = claims?.FirstOrDefault(x => x.Type.Equals("emails", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
            userVM.Address = $"{stAddress} , {city}, {state}, {country}";

            return userVM;

        }
    }
}
