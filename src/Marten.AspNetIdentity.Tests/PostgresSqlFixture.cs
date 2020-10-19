using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Databases;
using Xunit;

namespace Marten.AspNetIdentity.Tests
{
    public class PostgresSqlFixture : IAsyncLifetime
    {
        private readonly PostgreSqlTestcontainer _testContainer;
        
        public PostgresSqlFixture()
        {
            var testContainerBuilder = new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithCleanUp(true)
                .WithDatabase(new PostgreSqlTestcontainerConfiguration
                {
                    Database = "aspnetidentity",
                    Username = "aspnetidentity",
                    Password = "aspnetidentity"
                })
                .WithImage("clkao/postgres-plv8");

            _testContainer = testContainerBuilder.Build();
        }

        public string ConnectionString => _testContainer.ConnectionString;
        
        public async Task InitializeAsync()
        {
            await _testContainer.StartAsync();

            var result = await _testContainer.ExecAsync(new[]
            {
                "/bin/sh", "-c",
                "psql -U aspnetidentity -c \"CREATE EXTENSION plv8; SELECT extversion FROM pg_extensions WHERE extname = 'plv8';\""
            });
        }

        public async Task DisposeAsync()
        {
            await _testContainer.StopAsync();
        }
    }
}