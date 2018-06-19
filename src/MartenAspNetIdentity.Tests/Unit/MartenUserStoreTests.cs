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
			var identityUser = _fixture.Create<IdentityUser>();

			// when
			string id = await _userStore.GetUserIdAsync(identityUser, CancellationToken.None);

			// then
			id.ShouldBe(identityUser.Id);
		}

		[Fact]
		public async Task SetRoleNameAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
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
			var identityUser = _fixture.Create<IdentityUser>();

			// when
			string normalizedUserName = await _userStore.GetNormalizedUserNameAsync(identityUser, CancellationToken.None);

			// then
			normalizedUserName.ShouldBe(identityUser.NormalizedUserName);
		}

		[Fact]
		public async Task SetNormalizedUserNameAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			string normalizedUsername = "NORMALIZEDNAME";

			// when
			await _userStore.SetNormalizedUserNameAsync(identityUser, normalizedUsername, CancellationToken.None);

			// then
			identityUser.NormalizedUserName.ShouldBe(normalizedUsername);
		}

		[Fact]
		public async Task SetPasswordHashAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			string passwordHash = "Password Hash";

			// when
			await _userStore.SetPasswordHashAsync(identityUser, passwordHash, CancellationToken.None);

			// then
			identityUser.PasswordHash.ShouldBe(passwordHash);
		}

		[Fact]
		public async Task GetPasswordHashAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();

			// when
			string passwordHash = await _userStore.GetPasswordHashAsync(identityUser, CancellationToken.None);

			// then
			passwordHash.ShouldBe(identityUser.PasswordHash);
		}

		[Fact]
		public async Task HasPasswordAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();

			// when
			bool hasPasswordHash = await _userStore.HasPasswordAsync(identityUser, CancellationToken.None);

			// then
			hasPasswordHash.ShouldBeTrue();
		}

		[Fact]
		public async Task SetEmailAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			string email = "billgates@microsoft.com";

			// when
			await _userStore.SetEmailAsync(identityUser, email, CancellationToken.None);

			// then
			identityUser.Email.ShouldBe(email);
		}

		[Fact]
		public async Task GetEmailAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();

			// when
			string email = await _userStore.GetEmailAsync(identityUser, CancellationToken.None);

			// then
			email.ShouldBe(identityUser.Email);
		}

		[Fact]
		public async Task GetEmailConfirmedAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			identityUser.EmailConfirmed = true;

			// when
			bool emailConfirmed = await _userStore.GetEmailConfirmedAsync(identityUser, CancellationToken.None);

			// then
			emailConfirmed.ShouldBe(identityUser.EmailConfirmed);
		}

		[Fact]
		public async Task SetEmailConfirmedAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			bool confirmed = true;

			// when
			await _userStore.SetEmailConfirmedAsync(identityUser, confirmed, CancellationToken.None);

			// then
			identityUser.EmailConfirmed.ShouldBe(confirmed);
		}

		[Fact]
		public async Task GetNormalizedEmailAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();

			// when
			string normalizedEmail = await _userStore.GetNormalizedEmailAsync(identityUser, CancellationToken.None);

			// then
			normalizedEmail.ShouldBe(identityUser.NormalizedEmail);
		}

		[Fact]
		public async Task SetNormalizedEmailAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			string normalizedEmail = "NORMALIZEDNAME";

			// when
			await _userStore.SetNormalizedEmailAsync(identityUser, normalizedEmail, CancellationToken.None);

			// then
			identityUser.NormalizedEmail.ShouldBe(normalizedEmail);
		}

		[Fact]
		public async Task SetPhoneNumberAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			string phoneNumber = "0800505050";

			// when
			await _userStore.SetPhoneNumberAsync(identityUser, phoneNumber, CancellationToken.None);

			// then
			identityUser.PhoneNumber.ShouldBe(phoneNumber);
		}

		[Fact]
		public async Task GetPhoneNumberAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();

			// when
			string phoneNumber = await _userStore.GetPhoneNumberAsync(identityUser, CancellationToken.None);

			// then
			phoneNumber.ShouldBe(identityUser.PhoneNumber);
		}

		[Fact]
		public async Task GetPhoneNumberConfirmedAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			identityUser.PhoneNumberConfirmed = true;

			// when
			bool emailConfirmed = await _userStore.GetPhoneNumberConfirmedAsync(identityUser, CancellationToken.None);

			// then
			emailConfirmed.ShouldBe(identityUser.PhoneNumberConfirmed);
		}

		[Fact]
		public async Task SetPhoneNumberConfirmedAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			bool confirmed = true;

			// when
			await _userStore.SetPhoneNumberConfirmedAsync(identityUser, confirmed, CancellationToken.None);

			// then
			identityUser.PhoneNumberConfirmed.ShouldBe(confirmed);
		}

		// TODO: two factor auth
	}
}