using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using ToDoWebApi.DataAccess;
using Xunit;

namespace ToDoWebApi.Test.ToDoControllerTests
{
    [Collection(InfraFixture.INFRA_FIXTURE_KEY)]
    public class SetCompletedTest
    {
        private readonly HttpClient _httpClient;
        private readonly ToDoDbContext _dbContext;

        public SetCompletedTest(InfraFixture infraFixture)
        {
            _httpClient = infraFixture.InfraInstance.HttpClient;
            var scope = infraFixture.InfraInstance.ServiceProvider.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
        }

        [Fact]
        public async Task WhenToDoDoesNotExist__ResponseShouldBeNotFound()
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"todos/{Guid.NewGuid()}/completed");
            using HttpResponseMessage responseMessage = await _httpClient.SendAsync(httpRequestMessage);

            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
        }

        [Fact]
        public async Task WhenToDoIsIncomplete__ResponseShouldBeOk_and_ToDoStatusShouldBeCompleted()
        {
            // Arrange
            var toDo = new ToDoItemModel("some to-do");
            EntityEntry<ToDoItemModel> toDoEntity = await _dbContext.AddAsync(toDo);
            await _dbContext.SaveChangesAsync();

            // Act
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"todos/{toDo.Id}/completed");
            using HttpResponseMessage responseMessage = await _httpClient.SendAsync(httpRequestMessage);

            // Assert
            await toDoEntity.ReloadAsync();
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.True(toDo.Completed);
        }

        [Fact]
        public async Task WhenToDoIsCompleted__ResponseShouldBeOk_and_ToDoCompletedOnShouldBeModified()
        {
            // Arrange
            var toDo = new ToDoItemModel("some to-do");
            toDo.SetCompleted();
            EntityEntry<ToDoItemModel> toDoEntity = await _dbContext.AddAsync(toDo);
            await _dbContext.SaveChangesAsync();
            DateTime toDoInitialCompletedOn = toDo.CompletedOn ?? default;

            // Act
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"todos/{toDo.Id}/completed");
            using HttpResponseMessage responseMessage = await _httpClient.SendAsync(httpRequestMessage);

            // Assert
            await toDoEntity.ReloadAsync();
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.True(toDo.Completed);
            Assert.True(toDo.CompletedOn > toDoInitialCompletedOn);
        }
    }
}