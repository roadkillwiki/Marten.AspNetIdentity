using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Shouldly;
using Xunit.Abstractions;

namespace MartenAspNetIdentity.Tests.Integration
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
		private Fixture _fixture;

		public MartenRoleStoreTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
			_fixture = new Fixture();

			var store = DocumentStoreManager.GetMartenDocumentStore(typeof(MartenRoleStoreTests));
			_roleStore = new MartenRoleStore<IdentityRole>(store, new NullLogger<MartenRoleStore<IdentityRole>>());
		}

		public async Task<List<IdentityRole>> Add5Roles()
		{
			var rolesList = new List<IdentityRole>();
			for (int i = 1; i <= 5; i++)
			{
				var identityRole = new IdentityRole($"Role number {i}");
				identityRole.NormalizedName = identityRole.Name;
				identityRole.Id = identityRole.Id;
				await _roleStore.CreateAsync(identityRole, CancellationToken.None);

				rolesList.Add(identityRole);
			}

			return rolesList;
		}

		[Fact]
		public async Task CreateAsync()
		{
			// given
			var identityRole = new IdentityRole("root administrator sa");
			identityRole.NormalizedName = identityRole.Name;

			// when
			IdentityResult result = await _roleStore.CreateAsync(identityRole, CancellationToken.None);

			// then
			result.ShouldBe(IdentityResult.Success);

			IdentityRole role = await _roleStore.FindByNameAsync(identityRole.Name, CancellationToken.None);
			role.ShouldNotBeNull();
		}

		[Fact]
		public async Task UpdateAsync()
		{
			// given
			List<IdentityRole> rolesList = await Add5Roles();

			IdentityRole updatedRole = rolesList.First();
			updatedRole.Name = "A new name";

			// when
			IdentityResult result = await _roleStore.UpdateAsync(updatedRole, CancellationToken.None);

			// then
			result.ShouldBe(IdentityResult.Success);

			IdentityRole role = await _roleStore.FindByIdAsync(updatedRole.Id, CancellationToken.None);
			role.ShouldNotBeNull();
			role.Name.ShouldBe(updatedRole.Name);
		}

		[Fact]
		public async Task FindByNameAsync()
		{
			// given
			string roleName = "super root admin";
			var expectedRole = new IdentityRole(roleName);

			// when
			IdentityRole actualRole = await _roleStore.FindByNameAsync(roleName, CancellationToken.None);

			// then
			actualRole.ShouldNotBeNull();
			actualRole.Name.ShouldBe(expectedRole.Name);
		}

		[Fact]
		public async Task FindByIdAsync()
		{
			// given
			string id = "super root admin";
			var expectedRole = new IdentityRole(id);

			// when
			IdentityRole actualRole = await _roleStore.FindByIdAsync(id, CancellationToken.None);

			// then
			actualRole.ShouldNotBeNull();
			actualRole.Id.ShouldBe(expectedRole.Id);
		}
	}
}