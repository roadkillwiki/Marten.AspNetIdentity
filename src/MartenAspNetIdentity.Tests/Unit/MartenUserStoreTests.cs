using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Marten;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using Shouldly;
using Xunit.Abstractions;

namespace MartenAspNetIdentity.Tests.Unit
{
	public class MartenUserStoreTests
	{
		//
		// These tests require a postgres server, the Docker command is:
		//
		// $> docker run -d -p 5432:5432 --name aspnetidentity-postgres -e POSTGRES_USER=aspnetidentity -e POSTGRES_PASSWORD=aspnetidentity postgres
		//

		private ITestOutputHelper _testOutputHelper;
		private MartenUserStore<IdentityUser> _userStore;
		private Mock<IDocumentStore> _documentStore;
		private readonly Fixture _fixture;

		public MartenUserStoreTests(ITestOutputHelper testOutputHelper)
		{
			_fixture = new Fixture();
			_testOutputHelper = testOutputHelper;
			_documentStore = new Mock<IDocumentStore>();
			_userStore = new MartenUserStore<IdentityUser>(_documentStore.Object, new NullLogger<MartenUserStore<IdentityUser>>());
		}

		[Fact]
		public async Task GetRoleIdAsync()
		{
			// given
			var identityUser = new IdentityUser("my root user");
			identityUser.Id = "my id";

			// when
			string id = await _userStore.GetUserIdAsync(identityUser, CancellationToken.None);

			// then
			id.ShouldBe(identityUser.Id);
		}

		[Fact]
		public async Task SetRoleNameAsync()
		{
			// given
			var identityUser = new IdentityUser("my root user");
			string expectedUsername = "new user name";

			// when
			await _userStore.SetUserNameAsync(identityUser, expectedUsername, CancellationToken.None);

			// then
			identityUser.UserName.ShouldBe(expectedUsername);
		}

		[Fact]
		public async Task GetNormalizedUserNameAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task SetNormalizedUserNameAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task FindByIdAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task FindByNameAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task SetPasswordHashAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task GetPasswordHashAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task HasPasswordAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task SetEmailAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task GetEmailAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task GetEmailConfirmedAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task SetEmailConfirmedAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task FindByEmailAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task GetNormalizedEmailAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task SetNormalizedEmailAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task SetPhoneNumberAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task GetPhoneNumberAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task GetPhoneNumberConfirmedAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		[Fact]
		public async Task SetPhoneNumberConfirmedAsync()
		{
			// given
			IdentityUser user = _fixture.Create<IdentityUser>();

			// when

			// then
		}

		// SetTwoFactorEnabledAsync
		// GetTwoFactorEnabledAsync
		//
	}
}