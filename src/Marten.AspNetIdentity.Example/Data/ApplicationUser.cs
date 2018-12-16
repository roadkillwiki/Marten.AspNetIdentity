using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace Marten.AspNetIdentity.Example.Data
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser, IClaimsUser
	{
		public IList<string> RoleClaims { get; set; }
	}
}