
using Microsoft.IdentityModel.Tokens;
using SkinTime.DAL.Entities;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkinTime.Helpers
{
    public class TokenUtilities : ITokenUtilities
    {
        private readonly string Issuer;
        private readonly string Key;

        public TokenUtilities(IConfiguration config)
        {
            Issuer = config.GetValue<string>("JWT:Issuer")!;
            Key = config.GetValue<string>("JWT:Key")!;
        }

        public string CreateJwtFromDictionary(Dictionary<string, string> data)
        {   
            var claims = new ClaimsIdentity();
            
            foreach (var pair in data)
            {
                claims.AddClaim(new Claim(pair.Key, pair.Value));    
            }

            var TokenHandler = new JwtSecurityTokenHandler();

            // Token Descriptor is used in order to make user Identity.
            SecurityTokenDescriptor TokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Issuer = Issuer,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = TokenHandler.CreateToken(TokenDescriptor);

            return TokenHandler.WriteToken(token);
        }

        public string CreateBase64RefreshToken(string id)
        {
            // The refresh token will has the following pattern {User id}|{createdTime}|{ExpiredTime}
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{id}|{DateTime.UtcNow.ToString()}|{DateTime.UtcNow.AddMinutes(10).ToString()}"));
        }

        public string? ValidateBase64RefreshToken(string token)
        {
            // Spliting the token into multiple part
            try
            {
                string[] tokenPart = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split('|');
                
                // Check for token malformation.
                if (tokenPart.Length != 3)
                {
                    return null;
                }

                // Check for the expiration time.
                if ( DateTime.Parse(tokenPart[2]) < DateTime.UtcNow)
                {
                    return null;
                }

                return tokenPart[0]; // Return the user id inside the token.
                }
                catch (FormatException)
                {
                    return null;
                }

            
        }

        public bool ValidateJwt(string token)
        {
            var TokenHandler = new JwtSecurityTokenHandler();

            var validatior = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
                ClockSkew = TimeSpan.Zero,
            };

            try
            {
                TokenHandler.ValidateToken(token, validatior, out var validatedToken);

                var Token = (JwtSecurityToken) validatedToken;
                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                // Actions when token is expired
                return false;
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                // Actions when token does not match the signature
                return false;
            }
            catch (SecurityTokenInvalidIssuerException)
            {
                // Actions when token does not match the issuer.
                return false;
            }
        }

        public Dictionary<string, string> GetDataDictionaryFromJwt(string token)
        {
            Dictionary<string, string> tokenData = new Dictionary<string, string>();

            var TokenHandler = new JwtSecurityTokenHandler();

            if (TokenHandler.CanReadToken(token))
            {
                JwtSecurityToken objectifiedToken = TokenHandler.ReadJwtToken(token);

                foreach (var claim in objectifiedToken.Claims)
                {
                    tokenData.Add(claim.Type, claim.Value);
                }
            }
            else
            {
                tokenData.Add("error", "Can not read token information");
            }

            return tokenData;
        }
    }
}
