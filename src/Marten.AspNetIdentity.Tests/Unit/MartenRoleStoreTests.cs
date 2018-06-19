using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Marten.AspNetIdentity.Tests.Unit
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
		private readonly Fixture _fixture;

		public MartenRoleStoreTests(ITestOutputHelper testOutputHelper)
		{
			_fixture = new Fixture();
			_testOutputHelper = testOutputHelper;
			_documentStore = new Mock<IDocumentStore>();
			_roleStore = new MartenRoleStore<IdentityRole>(_documentStore.Object, new NullLogger<MartenRoleStore<IdentityRole>>());
		}

		[Fact]
		public async Task GetRoleIdAsync()
		{
			// given
			var identityRole = _fixture.Create<IdentityRole>();

			// when
			string id = await _roleStore.GetRoleIdAsync(identityRole, CancellationToken.None);

			// then
			id.ShouldBe(identityRole.Id);
		}

		[Fact]
		public async Task GetRoleNameAsync()
		{
			// given
			var identityRole = _fixture.Create<IdentityRole>();

			// when
			string roleName = await _roleStore.GetRoleNameAsync(identityRole, CancellationToken.None);

			// then
			roleName.ShouldBe(identityRole.Name);
		}

		[Fact]
		public async Task SetRoleNameAsync()
		{
			// given
			var identityRole = _fixture.Create<IdentityRole>();
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
			var identityRole = _fixture.Create<IdentityRole>();

			// when
			string normalizedName = await _roleStore.GetNormalizedRoleNameAsync(identityRole, CancellationToken.None);

			// then
			normalizedName.ShouldBe(identityRole.NormalizedName);
		}

		[Fact]
		public async Task SetNormalizedRoleNameAsync()
		{
			// given
			var identityRole = _fixture.Create<IdentityRole>();
			string expectedNormalizedName = "CHIEFBINCOLLECTOR";

			// when
			await _roleStore.SetNormalizedRoleNameAsync(identityRole, expectedNormalizedName, CancellationToken.None);

			// then
			identityRole.NormalizedName.ShouldBe(expectedNormalizedName);
		}
	}
}