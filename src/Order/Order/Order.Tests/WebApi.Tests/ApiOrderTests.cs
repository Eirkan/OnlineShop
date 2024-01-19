using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Order.Contracts.ApiRoutes;
using Order.Contracts.Order.GetOrdersByDateRange;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Order.Tests.WebApi.Tests
{
    [TestCaseOrderer("PriorityOrderer", "Order.Tests")]
    public class ApiOrderTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public ApiOrderTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        private static HttpContent GetHttpContentForJson(object request)
        {
            HttpContent httpContentLogin = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContentLogin.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpContentLogin;
        }


        private T Deserialize<T>(HttpResponseMessage responseMessage)
        {
            var responseContent = responseMessage.Content.ReadAsStringAsync()!;
            var result = JsonConvert.DeserializeObject<T>(responseContent.Result);

            return result!;
        }


        [Fact]
        public async Task Get_Orders_ByDateRange_Test()
        {
            // Arrange        
            await Task.CompletedTask;
            var client = _factory.CreateClient();
            var request = new GetOrdersByDateRangeRequest(StartDate: DateTime.Now, EndDate: DateTime.Now);

            // Act
            var response =
                Should.CompleteIn(async () =>
                {
                    return await client.PostAsync(ApiRoutes.Order.GetOrdersByDateRange, GetHttpContentForJson(request));
                },
                    TimeSpan.FromSeconds(15)
                );
            var result = Deserialize<List<GetOrdersByDateRangeResponse>>(response);

            // Assert
            response.ShouldNotBeNull();
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

    }
}