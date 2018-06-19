using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Marten.AspNetIdentity.Tests.Integration
{
	public class MartenUserStoreTests
	{
		//
		// These tests require a postgres server, the Docker command is:
		//
		// $> docker run -d -p 5432:5432 --name aspnetidentity-postgres -e POSTGRES_USER=aspnetidentity -e POSTGRES_PASSWORD=aspnetidentity postgres
		//

		private readonly ITestOutputHelper _testOutputHelper;
		private MartenUserStore<IdentityUser> _userStore;
		private Fixture _fixture;

		public MartenUserStoreTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
			_fixture = new Fixture();

			var store = DocumentStoreManager.GetMartenDocumentStore(typeof(MartenUserStoreTests));
			_userStore = new MartenUserStore<IdentityUser>(store, new NullLogger<MartenUserStore<IdentityUser>>());
		}

		public async Task<List<IdentityUser>> AddFiveUsers()
		{
			var usersList = new List<IdentityUser>();
			for (int i = 1; i <= 5; i++)
			{
				var identityUser = new IdentityUser($"User {i}");
				identityUser.NormalizedUserName = identityUser.UserName;
				identityUser.Id = identityUser.Id;
				await _userStore.CreateAsync(identityUser, CancellationToken.None);

				usersList.Add(identityUser);
			}

			return usersList;
		}

		[Fact]
		public async Task CreateAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();

			// when
			IdentityResult result = await _userStore.CreateAsync(identityUser, CancellationToken.None);

			// then
			result.ShouldBe(IdentityResult.Success);

			IdentityUser role = await _userStore.FindByNameAsync(identityUser.NormalizedUserName, CancellationToken.None);
			role.ShouldNotBeNull();
		}

		[Fact]
		public async Task UpdateAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			await _userStore.CreateAsync(identityUser, CancellationToken.None);
			identityUser.UserName = "new name";

			// when
			IdentityResult result = await _userStore.UpdateAsync(identityUser, CancellationToken.None);

			// then
			result.ShouldBe(IdentityResult.Success);

			IdentityUser actualUserName = await _userStore.FindByIdAsync(identityUser.Id, CancellationToken.None);
			actualUserName.ShouldNotBeNull();
			actualUserName.UserName.ShouldBe(identityUser.UserName);
		}

		[Fact]
		public async Task FindByNameAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			await _userStore.CreateAsync(identityUser, CancellationToken.None);

			// when
			IdentityUser actualUser = await _userStore.FindByNameAsync(identityUser.NormalizedUserName, CancellationToken.None);

			// then
			actualUser.ShouldNotBeNull();
			actualUser.Id.ShouldBe(identityUser.Id);
			actualUser.Email.ShouldBe(identityUser.Email);
			actualUser.NormalizedUserName.ShouldBe(identityUser.NormalizedUserName);
		}

		[Fact]
		public async Task FindByEmailAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			var result = await _userStore.CreateAsync(identityUser, CancellationToken.None);

			// when
			IdentityUser actualUser = await _userStore.FindByEmailAsync(identityUser.NormalizedEmail, CancellationToken.None);

			// then
			actualUser.ShouldNotBeNull();
			actualUser.Id.ShouldBe(identityUser.Id);
			actualUser.Email.ShouldBe(identityUser.Email);
			actualUser.NormalizedEmail.ShouldBe(identityUser.NormalizedEmail);
		}

		[Fact]
		public async Task FindByIdAsync()
		{
			// given
			var identityUser = _fixture.Create<IdentityUser>();
			await _userStore.CreateAsync(identityUser, CancellationToken.None);

			// when
			IdentityUser actualUser = await _userStore.FindByIdAsync(identityUser.Id, CancellationToken.None);

			// then
			actualUser.ShouldNotBeNull();
			actualUser.Id.ShouldBe(identityUser.Id);
		}
	}
}