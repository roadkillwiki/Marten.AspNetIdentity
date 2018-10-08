using System.Collections.Generic;

namespace Marten.AspNetIdentity
{
	public interface IClaimsUser
	{
		IList<byte[]> Claims { get; set; }
	}
}