using ApiRestExpress.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiRestExpress
{
    [Route("api/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        protected readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(AppUser appUser)
        {
            AppUser user = _context.AppUsers.SingleOrDefault(user => user.Login == appUser.Login);

            if (user == null)
            {
                return BadRequest("Invalid Credentials");
            }

            //TODO 2.Check encrypted password
            string storedEncyptedPassword = user.Password.Split('$')[1];
            string b64salt = user.Password.Split('$')[0];
            byte[] salt = Convert.FromBase64String(b64salt);
            string inputEncryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: appUser.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            if (storedEncyptedPassword != inputEncryptedPassword)
            {
                return BadRequest("Invalid Credentials");
            }

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Login", user.Login)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: signIn);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(AppUser appUser)
        {
            AppUser? user = _context.AppUsers.SingleOrDefault(user => user.Login == appUser.Login);

            if (user != null)
            {
                return BadRequest("Login already used");
            }

            byte[] salt = new byte[256 / 8]; // Generate a 256-bit salt using a secure PRNG
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string b64salt = Convert.ToBase64String(salt);
            string encryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: appUser.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            appUser.Password = b64salt + "$" + encryptedPassword;
            _context.AppUsers.Add(appUser);
            _context.SaveChanges();

            return Ok();
        }

    }
}
