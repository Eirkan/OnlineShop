using Azure.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Product.Contracts.ApiRoutes;
using Product.Contracts.Product.GetById;
using Product.Contracts.Product.Insert;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Product.Tests.WebApi.Tests
{
    [TestCaseOrderer("PriorityOrderer", "Product.Tests")]
    public class ApiProductTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public ApiProductTests(WebApplicationFactory<Startup> factory)
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
        public async Task Get_Product_ById()
        {
            // Arrange        
            await Task.CompletedTask;
            var client = _factory.CreateClient();
            var request = new GetByIdRequest(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"));

            // Act
            var response =
                Should.CompleteIn(async () =>
                {
                    return await client.PostAsync(ApiRoutes.Product.GetById, GetHttpContentForJson(request));
                },
                    TimeSpan.FromSeconds(30)
                );


            // Assert
            response.ShouldNotBeNull();
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }


        [Theory, TestPriority(1)]
        [InlineData("samsungA20", "samsung A20", 3.50, 5)]
        public async Task Insert_product_test(string Name, string description, decimal price, int availableStock)
        {
            // Arrange
            await using var application = new WebApplicationFactory<Startup>();
            using var client = application.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

            InsertRequest request = new InsertRequest(Name, description, price, availableStock);

            // Act
            var response = await client.PostAsync(ApiRoutes.Product.Insert, GetHttpContentForJson(request));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }


        [Theory, TestPriority(2)]
        [InlineData("samsungA20", "samsung A20", 3.50, 5)]
        public async Task Insert_and_get_product_test(string Name, string description, decimal price, int availableStock)
        {
            // Arrange
            await Task.CompletedTask;
            var client = _factory.CreateClient();
            var request = new InsertRequest(Name, description, price, availableStock);


            // Act
            var responseInsert =
                Should.CompleteIn(async () =>
                {
                    return await client.PostAsync(ApiRoutes.Product.Insert, GetHttpContentForJson(request));
                }, TimeSpan.FromSeconds(5));
            var resultInsert = Deserialize<InsertResponse>(responseInsert);


            var getByIdRequest = new GetByIdRequest(resultInsert.Id);
            var responseGetById =
                Should.CompleteIn(async () =>
                {
                    return await client.PostAsync(ApiRoutes.Product.GetById, GetHttpContentForJson(getByIdRequest));
                }, TimeSpan.FromSeconds(5));
            var resultGetById = Deserialize<GetByIdResponse>(responseGetById);


            // Assert
            responseInsert.StatusCode.ShouldBe(HttpStatusCode.OK);
            responseGetById.StatusCode.ShouldBe(HttpStatusCode.OK);

            resultInsert.Id.ShouldNotBe(Guid.Empty);
            resultGetById.Id.ShouldNotBe(Guid.Empty);
        }
    }
}