using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ToDoWebApi.DataAccess;
using Xunit;

namespace ToDoWebApi.Test.ToDoControllerTests
{
    [Collection(InfraFixture.INFRA_FIXTURE_KEY)]
    public class GetToDoTest
    {
        private readonly HttpClient _httpClient;
        private readonly ToDoDbContext _dbContext;

        public GetToDoTest(InfraFixture infraFixture)
        {
            _httpClient = infraFixture.InfraInstance.HttpClient;
            var scope = infraFixture.InfraInstance.ServiceProvider.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
        }

        [Fact]
        public async Task WhenToDoDoesNotExist__ResponseShouldBeNotFound()
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"todos/{Guid.NewGuid()}/");
            using HttpResponseMessage responseMessage = await _httpClient.SendAsync(httpRequestMessage);

            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
        }

        [Fact]
        public async Task WhenToDoIsExist__ResponseShouldBeOk_and_ContentShouldBeNotEmpty()
        {
            // Arrange
            var seedToDo = new ToDoItemModel("some to-do");
            await _dbContext.AddAsync(seedToDo);
            await _dbContext.SaveChangesAsync();

            // Act
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"todos/{seedToDo.Id}/");
            using HttpResponseMessage responseMessage = await _httpClient.SendAsync(httpRequestMessage);

            // Assert
            string content = await responseMessage.Content.ReadAsStringAsync();
            var toDo = JsonConvert.DeserializeObject<ToDoItemModel>(content);
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.NotNull(toDo);
        }
    }
}