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
	public class MartenUserStoreTests
	{
		//
		// These tests require a postgres server, the Docker command is:
		//
		// $> docker run -d -p 5432:5432 --name aspnetidentity-postgres -e POSTGRES_USER=aspnetidentity -e POSTGRES_PASSWORD=aspnetidentity postgres
		//

		private readonly ITestOutputHelper _testOutputHelper;
		private MartenUserStore<IdentityUser> _userStore;
		private Mock<IDocumentStore> _documentStore;

		public MartenUserStoreTests(ITestOutputHelper testOutputHelper)
		{
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
	}
}