using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WebApiProxy.Models
{
	public interface IAuthorizationTokenProvider
	{
		Token CreateToken(User user);
		ClaimsPrincipal ValidateToken(string token);
	}
}