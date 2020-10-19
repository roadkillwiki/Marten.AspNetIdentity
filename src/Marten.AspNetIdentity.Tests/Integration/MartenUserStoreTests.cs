using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Marten.AspNetIdentity.Tests.Integration
{
	public class MartenUserStoreTests : IClassFixture<PostgresSqlFixture>
	{
		private readonly ITestOutputHelper _testOutputHelper;
		private MartenUserStore<TestUser> _userStore;

		public MartenUserStoreTests(ITestOutputHelper testOutputHelper, PostgresSqlFixture postgresSqlFixture)
		{
			_testOutputHelper = testOutputHelper;

			var store = DocumentStoreManager.GetMartenDocumentStore(typeof(MartenUserStoreTests), postgresSqlFixture.ConnectionString);
			_userStore = new MartenUserStore<TestUser>(store, new NullLogger<MartenUserStore<TestUser>>());
			_userStore.Wipe();
		}

		public async Task<List<TestUser>> AddFiveUsers()
		{
			var usersList = new List<TestUser>();
			for (int i = 1; i <= 5; i++)
			{
				var testUser = new TestUser() { UserName = $"User {i}" };
				testUser.NormalizedUserName = testUser.UserName;
				testUser.Id = testUser.Id;
				await _userStore.CreateAsync(testUser, CancellationToken.None);

				usersList.Add(testUser);
			}

			return usersList;
		}

		[Theory]
		[AutoData]
		public async Task CreateAsync(TestUser testUser)
		{
			// given

			// when
			IdentityResult result = await _userStore.CreateAsync(testUser, CancellationToken.None);

			// then
			result.ShouldBe(IdentityResult.Success);

			TestUser user = await _userStore.FindByNameAsync(testUser.NormalizedUserName, CancellationToken.None);
			user.ShouldNotBeNull();
		}

		[Theory]
		[AutoData]
		public async Task UpdateAsync(TestUser testUser)
		{
			// given
			await _userStore.CreateAsync(testUser, CancellationToken.None);
			testUser.UserName = "new name";

			// when
			IdentityResult result = await _userStore.UpdateAsync(testUser, CancellationToken.None);

			// then
			result.ShouldBe(IdentityResult.Success);

			TestUser actualUserName = await _userStore.FindByIdAsync(testUser.Id, CancellationToken.None);
			actualUserName.ShouldNotBeNull();
			actualUserName.UserName.ShouldBe(testUser.UserName);
		}

		[Theory]
		[AutoData]
		public async Task FindByNameAsync(TestUser testUser)
		{
			// given
			await _userStore.CreateAsync(testUser, CancellationToken.None);

			// when
			TestUser actualUser = await _userStore.FindByNameAsync(testUser.NormalizedUserName, CancellationToken.None);

			// then
			actualUser.ShouldNotBeNull();
			actualUser.Id.ShouldBe(testUser.Id);
			actualUser.Email.ShouldBe(testUser.Email);
			actualUser.NormalizedUserName.ShouldBe(testUser.NormalizedUserName);
		}

		[Theory]
		[AutoData]
		public async Task FindByEmailAsync(TestUser testUser)
		{
			// given
			var result = await _userStore.CreateAsync(testUser, CancellationToken.None);

			// when
			TestUser actualUser = await _userStore.FindByEmailAsync(testUser.NormalizedEmail, CancellationToken.None);

			// then
			actualUser.ShouldNotBeNull();
			actualUser.Id.ShouldBe(testUser.Id);
			actualUser.Email.ShouldBe(testUser.Email);
			actualUser.NormalizedEmail.ShouldBe(testUser.NormalizedEmail);
		}

		[Theory]
		[AutoData]
		public async Task FindByIdAsync(TestUser testUser)
		{
			// given
			await _userStore.CreateAsync(testUser, CancellationToken.None);

			// when
			TestUser actualUser = await _userStore.FindByIdAsync(testUser.Id, CancellationToken.None);

			// then
			actualUser.ShouldNotBeNull();
			actualUser.Id.ShouldBe(testUser.Id);
		}

		[Fact]
		public async Task Users_IQueryable()
		{
			// given
			List<TestUser> testUsers = await AddFiveUsers();
			TestUser testUser = testUsers.First();

			// when
			TestUser actualUser = _userStore.Users.First(x => x.Id == testUser.Id);

			// then
			actualUser.ShouldNotBeNull();
			actualUser.Id.ShouldBe(testUser.Id);
			actualUser.Email.ShouldBe(testUser.Email);
			actualUser.NormalizedEmail.ShouldBe(testUser.NormalizedEmail);
		}

		[Theory]
		[AutoData]
		public async Task AddClaimsAsync(TestUser testUser)
		{
			// given
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, "CanRead"),
				new Claim(ClaimTypes.Role, "CanWrite"),
				new Claim(ClaimTypes.Surname, "This shouldn't be stored")
			};

			// when
			await _userStore.AddClaimsAsync(testUser, claims, CancellationToken.None);

			// then
			TestUser user = await _userStore.FindByNameAsync(testUser.NormalizedUserName, CancellationToken.None);
			user.RoleClaims.ShouldNotBeNull();
			user.RoleClaims.Count.ShouldBe(2);
		}

		[Theory]
		[AutoData]
		public async Task ReplaceClaimAsync(TestUser testUser)
		{
			// given
			var oldClaim = new Claim(ClaimTypes.Role, "PowerfulAdmin");
			var newClaim = new Claim(ClaimTypes.Role, "LonelyGrunt");

			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, "CanRead"),
				new Claim(ClaimTypes.Role, "CanWrite"),
				oldClaim
			};

			await _userStore.AddClaimsAsync(testUser, claims, CancellationToken.None);

			// when
			await _userStore.ReplaceClaimAsync(testUser, oldClaim, newClaim, CancellationToken.None);

			// then
			TestUser actualUser = await _userStore.FindByNameAsync(testUser.NormalizedUserName, CancellationToken.None);
			actualUser.RoleClaims.ShouldNotBeNull();
			actualUser.RoleClaims.Count.ShouldBe(3);

			var userClaims = actualUser.RoleClaims;
			userClaims.ShouldContain("LonelyGrunt");
			userClaims.ShouldNotContain("PowerfulAdmin");
		}

		[Theory]
		[AutoData]
		public async Task RemoveClaimsAsync(TestUser testUser)
		{
			// given
			var canWriteClaim = new Claim(ClaimTypes.Role, "CanWrite");
			var canReadClaim = new Claim(ClaimTypes.Role, "CanRead");
			var isAdminClaim = new Claim(ClaimTypes.Role, "IsAdmin");

			var claims = new List<Claim>()
			{
				canReadClaim,
				canWriteClaim,
				isAdminClaim
			};

			var claimsToRemove = new List<Claim>()
			{
				canWriteClaim,
				isAdminClaim
			};

			await _userStore.AddClaimsAsync(testUser, claims, CancellationToken.None);

			// when
			await _userStore.RemoveClaimsAsync(testUser, claimsToRemove, CancellationToken.None);

			// then
			TestUser actualUser = await _userStore.FindByNameAsync(testUser.NormalizedUserName, CancellationToken.None);
			actualUser.RoleClaims.ShouldNotBeNull();
			actualUser.RoleClaims.Count.ShouldBe(1);

			var userClaims = actualUser.RoleClaims;
			userClaims.ShouldNotContain(isAdminClaim.Value);
			userClaims.ShouldNotContain(canWriteClaim.Value);
		}

		[Theory]
		[AutoData]
		public async Task GetUsersForClaimAsync(List<TestUser> testUsers)
		{
			// given
			var canWriteClaim = new Claim(ClaimTypes.Role, "CanWrite");
			var canReadClaim = new Claim(ClaimTypes.Role, "CanRead");
			var isAdminClaim = new Claim(ClaimTypes.Role, "IsAdmin");

			var readWriteAndAdminClaims = new List<Claim>()
			{
				canReadClaim,
				canWriteClaim,
				isAdminClaim
			};

			var readClaims = new List<Claim>()
			{
				canReadClaim,
			};

			foreach (TestUser user in testUsers)
			{
				await _userStore.CreateAsync(user, CancellationToken.None);
			}

			await _userStore.AddClaimsAsync(testUsers[0], readWriteAndAdminClaims, CancellationToken.None);
			await _userStore.AddClaimsAsync(testUsers[1], readWriteAndAdminClaims, CancellationToken.None);
			await _userStore.AddClaimsAsync(testUsers[2], readClaims, CancellationToken.None);

			// when
			IList<TestUser> usersForClaim = await _userStore.GetUsersForClaimAsync(isAdminClaim, CancellationToken.None);

			// then
			usersForClaim.ShouldNotBeNull();
			usersForClaim.Count.ShouldBe(2);

			IEnumerable<string> userClaims = usersForClaim.First().RoleClaims;
			userClaims.ShouldContain(canReadClaim.Value);
			userClaims.ShouldContain(canWriteClaim.Value);
			userClaims.ShouldContain(isAdminClaim.Value);
		}
	}
}