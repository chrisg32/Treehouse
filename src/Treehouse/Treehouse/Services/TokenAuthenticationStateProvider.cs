using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using TreeHouse.Database;

namespace TreeHouse.Services
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;

        public TokenAuthenticationStateProvider(IJSRuntime jsRuntime, IConfiguration configuration)
        {
            _jsRuntime = jsRuntime;
            _configuration = configuration;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetTokenAsync();
            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task<bool> TryLogin(string userName, string password)
        {
            var isValid = await CheckCredentials(userName, password);
            if (!isValid)
            {
                await DeleteToken();
                return false;
            }

            var tokenInfo = CreateToken(userName);
            await SaveToken(tokenInfo);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            return true;
        }

        private async Task DeleteToken()
        {
            await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authToken");
            await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authTokenExpiry");
        }

        private async Task SaveToken((string token, DateTimeOffset expires) tokenInfo)
        {
            var (token, expires) = tokenInfo;
            await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authToken", token);
            await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authTokenExpiry", expires);
        }

        private (string token, DateTimeOffset expires) CreateToken(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenExpiration = int.Parse(_configuration["Jwt:TokenExpiration"]);
            var expires = DateTimeOffset.Now.AddMinutes(tokenExpiration);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires.LocalDateTime,
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenString, expires);
        }

        private async Task<bool> CheckCredentials(string userName, string password)
        {
            await using var db = new TreeHouseContext(_configuration["db"]);
            var hashedPass = HashPass(password);
            return (await db.Users.ToListAsync()).Any(u => u.FirstName.Equals(userName, StringComparison.InvariantCultureIgnoreCase) && u.Password.Equals(hashedPass));
        }

        public static string HashPass(string text)
        {
            var clearBytes = Encoding.Default.GetBytes(text);
            var hashedBytes = SHA1.Create().ComputeHash(clearBytes);
            return Encoding.Unicode.GetString(hashedBytes);
        }
        private async Task<string> GetTokenAsync()
        {
            var expiry = await _jsRuntime.InvokeAsync<object>("localStorage.getItem", "authTokenExpiry");
            if (expiry != null)
            {
                if (DateTime.Parse(expiry.ToString()) > DateTime.Now)
                {
                    return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
                }
            }
            return null;
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.SelectMany(ClaimFromKvp);
        }

        private static IEnumerable<Claim> ClaimFromKvp(KeyValuePair<string, object> kvp)
        {
            if (kvp.Value is JsonElement e && e.ValueKind == JsonValueKind.Array)
            {
                foreach (var arrayValue in e.EnumerateArray())
                {
                    yield return new Claim(kvp.Key, arrayValue.GetString());
                }
            }
            else yield return new Claim(kvp.Key, kvp.Value.ToString());
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task Logout()
        {
            await DeleteToken();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
