using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Marten.AspNetIdentity.Tests.Unit
{
	public class MartenUserStoreTests
	{
		private ITestOutputHelper _testOutputHelper;
		private MartenUserStore<TestUser> _userStore;
		private Mock<IDocumentStore> _documentStore;
		private readonly Fixture _fixture;

		public MartenUserStoreTests(ITestOutputHelper testOutputHelper)
		{
			_fixture = new Fixture();
			_fixture.Register<TestUser>(() => new TestUser());

			_testOutputHelper = testOutputHelper;
			_documentStore = new Mock<IDocumentStore>();
			_userStore = new MartenUserStore<TestUser>(_documentStore.Object, new NullLogger<MartenUserStore<TestUser>>());
		}

		[Theory]
		[CustomAutoData]
		public async Task GetRoleIdAsync(TestUser testUser)
		{
			// given

			// when
			string id = await _userStore.GetUserIdAsync(testUser, CancellationToken.None);

			// then
			id.ShouldBe(testUser.Id);
		}

		[Theory]
		[CustomAutoData]
		public async Task SetRoleNameAsync(TestUser testUser)
		{
			// given
			string expectedUsername = "new user name";

			// when
			await _userStore.SetUserNameAsync(testUser, expectedUsername, CancellationToken.None);

			// then
			testUser.UserName.ShouldBe(expectedUsername);
		}

		[Theory]
		[CustomAutoData]
		public async Task GetNormalizedUserNameAsync(TestUser testUser)
		{
			// given

			// when
			string normalizedUserName = await _userStore.GetNormalizedUserNameAsync(testUser, CancellationToken.None);

			// then
			normalizedUserName.ShouldBe(testUser.NormalizedUserName);
		}

		[Theory]
		[CustomAutoData]
		public async Task SetNormalizedUserNameAsync(TestUser testUser)
		{
			// given
			string normalizedUsername = "NORMALIZEDNAME";

			// when
			await _userStore.SetNormalizedUserNameAsync(testUser, normalizedUsername, CancellationToken.None);

			// then
			testUser.NormalizedUserName.ShouldBe(normalizedUsername);
		}

		[Theory]
		[CustomAutoData]
		public async Task SetPasswordHashAsync(TestUser testUser)
		{
			// given
			string passwordHash = "Password Hash";

			// when
			await _userStore.SetPasswordHashAsync(testUser, passwordHash, CancellationToken.None);

			// then
			testUser.PasswordHash.ShouldBe(passwordHash);
		}

		[Theory]
		[CustomAutoData]
		public async Task GetPasswordHashAsync(TestUser testUser)
		{
			// given

			// when
			string passwordHash = await _userStore.GetPasswordHashAsync(testUser, CancellationToken.None);

			// then
			passwordHash.ShouldBe(testUser.PasswordHash);
		}

		[Theory]
		[CustomAutoData]
		public async Task HasPasswordAsync(TestUser testUser)
		{
			// given

			// when
			bool hasPasswordHash = await _userStore.HasPasswordAsync(testUser, CancellationToken.None);

			// then
			hasPasswordHash.ShouldBeTrue();
		}

		[Theory]
		[CustomAutoData]
		public async Task SetEmailAsync(TestUser testUser)
		{
			// given
			string email = "billgates@microsoft.com";

			// when
			await _userStore.SetEmailAsync(testUser, email, CancellationToken.None);

			// then
			testUser.Email.ShouldBe(email);
		}

		[Theory]
		[CustomAutoData]
		public async Task GetEmailAsync(TestUser testUser)
		{
			// given

			// when
			string email = await _userStore.GetEmailAsync(testUser, CancellationToken.None);

			// then
			email.ShouldBe(testUser.Email);
		}

		[Theory]
		[CustomAutoData]
		public async Task GetEmailConfirmedAsync(TestUser testUser)
		{
			// given
			testUser.EmailConfirmed = true;

			// when
			bool emailConfirmed = await _userStore.GetEmailConfirmedAsync(testUser, CancellationToken.None);

			// then
			emailConfirmed.ShouldBe(testUser.EmailConfirmed);
		}

		[Theory]
		[CustomAutoData]
		public async Task SetEmailConfirmedAsync(TestUser testUser)
		{
			// given
			bool confirmed = true;

			// when
			await _userStore.SetEmailConfirmedAsync(testUser, confirmed, CancellationToken.None);

			// then
			testUser.EmailConfirmed.ShouldBe(confirmed);
		}

		[Theory]
		[CustomAutoData]
		public async Task GetNormalizedEmailAsync(TestUser testUser)
		{
			// given

			// when
			string normalizedEmail = await _userStore.GetNormalizedEmailAsync(testUser, CancellationToken.None);

			// then
			normalizedEmail.ShouldBe(testUser.NormalizedEmail);
		}

		[Theory]
		[CustomAutoData]
		public async Task SetNormalizedEmailAsync(TestUser testUser)
		{
			// given
			string normalizedEmail = "NORMALIZEDNAME";

			// when
			await _userStore.SetNormalizedEmailAsync(testUser, normalizedEmail, CancellationToken.None);

			// then
			testUser.NormalizedEmail.ShouldBe(normalizedEmail);
		}

		[Theory]
		[CustomAutoData]
		public async Task SetPhoneNumberAsync(TestUser testUser)
		{
			// given
			string phoneNumber = "0800505050";

			// when
			await _userStore.SetPhoneNumberAsync(testUser, phoneNumber, CancellationToken.None);

			// then
			testUser.PhoneNumber.ShouldBe(phoneNumber);
		}

		[Theory]
		[CustomAutoData]
		public async Task GetPhoneNumberAsync(TestUser testUser)
		{
			// given

			// when
			string phoneNumber = await _userStore.GetPhoneNumberAsync(testUser, CancellationToken.None);

			// then
			phoneNumber.ShouldBe(testUser.PhoneNumber);
		}

		[Theory]
		[CustomAutoData]
		public async Task GetPhoneNumberConfirmedAsync(TestUser testUser)
		{
			// given
			testUser.PhoneNumberConfirmed = true;

			// when
			bool emailConfirmed = await _userStore.GetPhoneNumberConfirmedAsync(testUser, CancellationToken.None);

			// then
			emailConfirmed.ShouldBe(testUser.PhoneNumberConfirmed);
		}

		[Theory]
		[CustomAutoData]
		public async Task SetPhoneNumberConfirmedAsync(TestUser testUser)
		{
			// given
			bool confirmed = true;

			// when
			await _userStore.SetPhoneNumberConfirmedAsync(testUser, confirmed, CancellationToken.None);

			// then
			testUser.PhoneNumberConfirmed.ShouldBe(confirmed);
		}

		// TODO: two factor auth
	}
}