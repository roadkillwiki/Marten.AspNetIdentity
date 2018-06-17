using System;
using System.Reflection;
using Marten;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MartenAspNetIdentity
{
	public static class Extensions
	{
		public static IdentityBuilder AddMartenStores<TUser, TRole>(this IdentityBuilder builder, string postgresConnectionString)
													where TUser : IdentityUser
													where TRole : IdentityRole
		{
			IDocumentStore documentStore = CreateDocumentStore(postgresConnectionString);

			builder = builder
						  .AddRoleStore<MartenRoleStore<TRole>>()
						  .AddUserStore<MartenUserStore<TUser>>();

			builder.Services.AddSingleton<IDocumentStore>(documentStore);

			return builder;
		}

		public static IdentityBuilder AddMartenStores<TUser, TRole>(this IdentityBuilder builder)
												where TUser : IdentityUser
												where TRole : IdentityRole
		{
			return builder.AddRoleStore<MartenRoleStore<TRole>>()
						  .AddUserStore<MartenUserStore<TUser>>();
		}

		internal static IDocumentStore CreateDocumentStore(string connectionString)
		{
			string databaseOwner = "aspnetidentity";
			var documentStore = DocumentStore.For(options =>
			{
				options.CreateDatabasesForTenants(c =>
				{
					c.MaintenanceDatabase(connectionString);
					c.ForTenant()
						.CheckAgainstPgDatabase()
						.WithOwner(databaseOwner)
						.WithEncoding("UTF-8")
						.ConnectionLimit(-1)
						.OnDatabaseCreated(_ =>
						{
							Console.WriteLine($"Postgres '{_.Database}' database created");
						});
				});

				options.Connection(connectionString);
				options.Schema.For<IdentityRole>().Index(x => x.Id);
			});

			return documentStore;
		}
	}
}