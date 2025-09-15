using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Infrastructure.Services;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace CustomerOnboarding.Tests
{
    public class BankServiceTests
    {
        private static IHttpClientFactory CreateHttpClientFactory(HttpResponseMessage response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var client = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://fake-alat-url.com/")
            };

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(f => f.CreateClient("alat")).Returns(client);

            return factoryMock.Object;
        }

        [Fact]
        public async Task GetBanksAsync_ReturnsSuccess_WhenApiResponseIsValid()
        {
            // Arrange
            var expectedResponse = new BankApiResponse
            {
                HasError = false,
                Result = new List<BankDto>
                {
                    new BankDto { Name = "Test Bank", Code = "123" }
                },
                TimeGenerated = DateTime.UtcNow
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedResponse)
            };

            var factory = CreateHttpClientFactory(httpResponse);
            var service = new BankService(factory);

            // Act  
            var result = await service.GetBanksAsync();

            // Assert
            Assert.False(result.HasError);
            Assert.Single(result.Result);
            Assert.Equal("Test Bank", result.Result.First().Name);
        }

        [Fact]
        public async Task GetBanksAsync_ReturnsError_WhenHttpFails()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var factory = CreateHttpClientFactory(httpResponse);
            var service = new BankService(factory);

            // Act
            var result = await service.GetBanksAsync();

            // Assert
            Assert.True(result.HasError);
            Assert.Contains("Failed to fetch banks", result.ErrorMessage);
        }

        [Fact]
        public async Task GetBanksAsync_ReturnsError_WhenApiResponseIsNull()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null") // invalid response
            };

            var factory = CreateHttpClientFactory(httpResponse);
            var service = new BankService(factory);

            // Act
            var result = await service.GetBanksAsync();

            // Assert
            Assert.True(result.HasError);
            Assert.Equal("Invalid response from bank API.", result.ErrorMessage);
        }
    }
}
