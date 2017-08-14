using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WebApi.ViewModels;
using Data.Infrastructure.UnitOfWork;
using Model;
using Newtonsoft.Json;
using Service.Services;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Auth
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionService _permissionService;
        private readonly UserManager<AppUser> _userManager;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork, IPermissionService permissionService, UserManager<AppUser> userManager)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            _unitOfWork = unitOfWork;
            _permissionService = permissionService;
            _userManager = userManager;
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
           
            var claims = new[]
         {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id),

                 //Custom claim
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.fullName),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.avatar),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.email),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.username),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.roles),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.permissions)
             };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id)
        {
            var requestUser = _unitOfWork.GetRepository<AppUser>().Find(id);
            var roles = _userManager.GetRolesAsync(requestUser);
            var permissions = _permissionService.GetByUserId(requestUser.Id);

            string fullName = string.IsNullOrEmpty(requestUser.FullName) ? "" : requestUser.FullName;
            string avatar = string.IsNullOrEmpty(requestUser.Avatar) ? "" : requestUser.Avatar;
            string email = string.IsNullOrEmpty(requestUser.Email) ? "" : requestUser.Email;

            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                    new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, id),
                    new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, Helpers.Constants.Strings.JwtClaims.ApiAccess),

                    //Custom claim
                    new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.fullName, fullName),
                    new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.avatar, avatar),
                    new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.email, email),
                    new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.username, requestUser.UserName),
                    new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.roles, JsonConvert.SerializeObject(roles)),
                    new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.permissions, JsonConvert.SerializeObject(permissions))

            });
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}
