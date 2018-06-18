using System;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using Shouldly;
using Xunit.Abstractions;

namespace MartenAspNetIdentity.Tests.Unit
{
	public class MartenRoleStoreTests
	{
		//
		// These tests require a postgres server, the Docker command is:
		//
		// $> docker run -d -p 5432:5432 --name aspnetidentity-postgres -e POSTGRES_USER=aspnetidentity -e POSTGRES_PASSWORD=aspnetidentity postgres
		//

		private readonly ITestOutputHelper _testOutputHelper;
		private MartenRoleStore<IdentityRole> _roleStore;
		private Mock<IDocumentStore> _documentStore;

		public MartenRoleStoreTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
			_documentStore = new Mock<IDocumentStore>();
			_roleStore = new MartenRoleStore<IdentityRole>(_documentStore.Object, new NullLogger<MartenRoleStore<IdentityRole>>());
		}

		[Fact]
		public async Task GetRoleIdAsync()
		{
			// given
			var identityRole = new IdentityRole("chief bin collector");
			identityRole.Id = "my id";

			// when
			string id = await _roleStore.GetRoleIdAsync(identityRole, CancellationToken.None);

			// then
			id.ShouldBe(identityRole.Id);
		}

		[Fact]
		public async Task GetRoleNameAsync()
		{
			// given
			var identityRole = new IdentityRole("chief bin collector");

			// when
			string roleName = await _roleStore.GetRoleNameAsync(identityRole, CancellationToken.None);

			// then
			roleName.ShouldBe(identityRole.Name);
		}

		[Fact]
		public async Task SetRoleNameAsync()
		{
			// given
			var identityRole = new IdentityRole("chief bin collector");
			string expectedRoleName = "new role name";

			// when
			await _roleStore.SetRoleNameAsync(identityRole, expectedRoleName, CancellationToken.None);

			// then
			identityRole.Name.ShouldBe(expectedRoleName);
		}

		[Fact]
		public async Task GetNormalizedRoleNameAsync()
		{
			// given
			var identityRole = new IdentityRole("chief bin collector");
			identityRole.NormalizedName = "normalized name";

			// when
			string normalizedName = await _roleStore.GetNormalizedRoleNameAsync(identityRole, CancellationToken.None);

			// then
			normalizedName.ShouldBe(identityRole.NormalizedName);
		}

		[Fact]
		public async Task SetNormalizedRoleNameAsync()
		{
			// given
			var identityRole = new IdentityRole("chief bin collector");
			string expectedNormalizedName = "CHIEFBINCOLLECTOR";

			// when
			await _roleStore.SetNormalizedRoleNameAsync(identityRole, expectedNormalizedName, CancellationToken.None);

			// then
			identityRole.NormalizedName.ShouldBe(expectedNormalizedName);
		}
	}
}