using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ToDoWebApi.Test.ToDoControllerTests
{
    [Collection(InfraFixture.INFRA_FIXTURE_KEY)]
    public class PostToDoTest
    {
        private readonly HttpClient _httpClient;

        public PostToDoTest(InfraFixture infraFixture)
        {
            _httpClient = infraFixture.InfraInstance.HttpClient;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task WhenToDoNameIsEmpty__ResponseShouldBeBadRequest(string toDoName)
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "todos");
            httpRequestMessage.Content = new StringContent($"\"{toDoName}\"", Encoding.Default, MediaTypeNames.Application.Json);
            using HttpResponseMessage responseMessage = await _httpClient.SendAsync(httpRequestMessage);

            Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }

        [Fact]
        public async Task WhenToDoNameIsValid__ResponseShouldBeCreated_and_IdShouldNotBeEmpty()
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "todos");
            httpRequestMessage.Content = new StringContent("\"some to-do\"", Encoding.Default, MediaTypeNames.Application.Json);
            using HttpResponseMessage responseMessage = await _httpClient.SendAsync(httpRequestMessage);
            string toDoId = await responseMessage.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
            Assert.NotEmpty(toDoId);
        }
    }
}