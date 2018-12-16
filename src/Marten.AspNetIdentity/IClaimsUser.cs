using System.Collections.Generic;

namespace Marten.AspNetIdentity
{
	public interface IClaimsUser
	{
		IList<string> RoleClaims { get; set; }
	}
}