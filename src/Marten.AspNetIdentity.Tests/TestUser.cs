using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Marten.AspNetIdentity.Tests
{
	public class TestUser : IdentityUser, IClaimsUser
	{
		public IList<string> RoleClaims { get; set; }

		public TestUser()
		{
			Id = Guid.NewGuid().ToString();
		}
	}
}