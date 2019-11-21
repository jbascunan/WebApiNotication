using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using WebApiProxy.Models;

namespace WebApiProxy.Security.Jwt
{
	public class JsonWebAuthorizationTokenProvider
	{
		private readonly SigningCredentials signingCredentials;
		private readonly string secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"]; //Base64Encoded String
		private readonly string issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"]; //The Issuer of the token
		private readonly string audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"]; //The targeted Audience
		private readonly string expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

		//Default constructor converts the Key into a base64 encoded string
		public JsonWebAuthorizationTokenProvider()
		{
			signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Convert.FromBase64String(secretKey)), SecurityAlgorithms.HmacSha256Signature);
		}

		public Token CreateToken(User user)
		{
			ClaimsIdentity claimsIdentity = GetClaimsIdentity(user);
			//claimsIdentity.AddClaims(claims);

			DateTime fechaHoraActual = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
			DateTime expirationTime = Convert.ToDateTime(fechaHoraActual.AddMinutes(Convert.ToInt32(expireTime)).ToString("dd-MM-yyyy HH:mm"));

			//var payLoad = new JwtPayload(issuerToken, audienceToken, claims, null, expirationTime); //Create a JWT Payload from claims         
			var handler = new JwtSecurityTokenHandler();

			var jwtSecurityToken = handler.CreateJwtSecurityToken(
				audience: audienceToken,
				issuer: issuerToken,
				subject: claimsIdentity,
				notBefore: fechaHoraActual,
				expires: expirationTime,
				signingCredentials: signingCredentials);

			var tokenString = handler.WriteToken(jwtSecurityToken); //Serialize the token

			var token = new Token(); //Instantiate a new Token (remember our poco?)
			token.token = tokenString; //Set the JWToken 
			token.expiration_date = expirationTime; //Set the Expiration

			return token; //Return the token
		}

		private ClaimsIdentity GetClaimsIdentity(User user)
		{
			// Here we can save some values to token.
			// For example we are storing here user id and email
			Claim[] claims = new[]
			{
				new Claim(ClaimTypes.Name, user.Username)
			};
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");

			// Adding roles code
			// Roles property is string collection but you can modify Select code if it it's not
			claimsIdentity.AddClaims(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
			return claimsIdentity;
		}
		
		public ClaimsPrincipal ValidateToken(string jwToken)
		{
			ClaimsPrincipal principal;
			SecurityToken token;
			var handler = new JwtSecurityTokenHandler();
			var validationParameters = ValidationParameters(); //The validation parameters used to validate the token

			try
			{
				//Try to validate token - will throw exception if anything is wrong, else return the ClaimsPrincipal
				principal = handler.ValidateToken(jwToken, validationParameters, out token);
			}
			catch (Exception)
			{
				principal = null;
			}
			return principal;
		}
		private TokenValidationParameters ValidationParameters()
		{
			//Expiration time is validated by default, but can be set explicitly
			var validationParameters = new TokenValidationParameters();
			var signKey = signingCredentials.Key;
			validationParameters.IssuerSigningKey = signKey; //Validate signingKey (the private key we used to sign the header with)
			validationParameters.ValidAudience = audienceToken; //Validate correct audience
			validationParameters.ValidIssuer = issuerToken; //Validate correct issuer
			return validationParameters;
		}

		private static string GenerateRandomSecretKey()
		{
			string secretKey = Guid.NewGuid().ToString();
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(secretKey);
			return Convert.ToBase64String(plainTextBytes);
		}
	}
}