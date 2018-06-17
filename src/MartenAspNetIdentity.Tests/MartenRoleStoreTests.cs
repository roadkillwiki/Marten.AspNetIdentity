using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Shouldly;
using Xunit.Abstractions;

namespace MartenAspNetIdentity.Tests
{
	public class MartenRoleStoreTests
	{
		private MartenRoleStore<IdentityRole> _roleStore;
		// docker run -d -p 5432:5432 --name aspnetidentity-postgres -e POSTGRES_USER=aspnetidentity -e POSTGRES_PASSWORD=aspnetidentity postgres

		public MartenRoleStoreTests(ITestOutputHelper testOutput)
		{
			testOutput.WriteLine("test");
			var store = DocumentStoreManager.GetMartenDocumentStore(typeof(MartenRoleStoreTests));
			_roleStore = new MartenRoleStore<IdentityRole>(store, new NullLogger<MartenRoleStore<IdentityRole>>());
		}

		[Fact]
		public async Task CreateAsync()
		{
			// given
			var identityRole = new IdentityRole("chief bin collector");
			identityRole.NormalizedName = identityRole.Name;

			// when
			IdentityResult result = await _roleStore.CreateAsync(identityRole, CancellationToken.None);

			// then
			result.ShouldBe(IdentityResult.Success);

			IdentityRole role = await _roleStore.FindByNameAsync(identityRole.Name, CancellationToken.None);
			role.ShouldNotBeNull();
		}
	}
}