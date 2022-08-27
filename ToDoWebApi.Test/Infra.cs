using System;
using System.Net.Http;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoWebApi.DataAccess;

namespace ToDoWebApi.Test
{
    public class Infra : IDisposable
    {
        public HttpClient HttpClient { get; }
        public IServiceProvider ServiceProvider { get; }

        private IHost TestServerHost { get; }
        private readonly TestcontainersContainer _dbTestContainer;

        public Infra()
        {
            // Configure Host
            IHostBuilder hostBuilder = Program.CreateHostBuilder(null!);
            hostBuilder.UseEnvironment("test")
                       .ConfigureWebHost(builder => { builder.UseTestServer(); });

            TestServerHost = hostBuilder.Build();
            ServiceProvider = TestServerHost.Services;

            // Create external dependencies and run
            _dbTestContainer = BuildSqlServerTestContainer(ServiceProvider);
            Task dbRunTask = _dbTestContainer.StartAsync();
            Task.WaitAll(dbRunTask);

            // Run Test Server
            TestServerHost.Start();

            HttpClient = TestServerHost.GetTestClient();
        }

        private TestcontainersContainer BuildSqlServerTestContainer(IServiceProvider serviceProvider)
        {
            var toDoDbContext = serviceProvider.GetRequiredService<ToDoDbContext>();
            string connectionString = toDoDbContext.Database.GetConnectionString();
            var dbConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            int portNumber = int.Parse(dbConnectionStringBuilder.DataSource.Split(",")[1]);
            ITestcontainersBuilder<MsSqlTestcontainer> testContainersBuilder
                = new TestcontainersBuilder<MsSqlTestcontainer>()
                   .WithDatabase(new MsSqlTestcontainerConfiguration
                                 {
                                     Password = dbConnectionStringBuilder.Password,
                                     Port = portNumber
                                 });

            MsSqlTestcontainer dbTestContainer = testContainersBuilder.Build();
            return dbTestContainer;
        }


        public void Dispose()
        {
            TestServerHost.Dispose();
            _dbTestContainer.DisposeAsync();
        }
    }
}