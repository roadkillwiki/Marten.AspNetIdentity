using AutoFixture;
using AutoFixture.Xunit2;

namespace Marten.AspNetIdentity.Tests
{
	public class CustomAutoDataAttribute : AutoDataAttribute
	{
		//public CustomAutoDataAttribute() : base(() =>
		//{
		//	// 🤢
		//	var fixture = new Fixture();
		//	fixture.Register<TestUser>(() => new TestUser());
		//	var user = fixture.Create<TestUser>();

		//	return fixture;
		//})
		//{ }
	}
}