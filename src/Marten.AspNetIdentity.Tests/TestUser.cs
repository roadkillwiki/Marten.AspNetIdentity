using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Marten.AspNetIdentity.Tests
{
	public class TestUser : IdentityUser, IClaimsUser
	{
		private List<byte[]> _claims = new List<byte[]>();

		// This is for the Json.net serializer, it can't serialize ILists
		public List<byte[]> Claims => _claims;

		// Hide the interface's version from Json.Net or it gets confused
		IList<byte[]> IClaimsUser.Claims
		{
			get
			{
				return _claims;
			}
			set
			{
				_claims = new List<byte[]>(value);
			}
		}

		public TestUser()
		{
			Id = Guid.NewGuid().ToString();
		}
	}
}