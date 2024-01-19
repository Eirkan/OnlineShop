using Customer.Contracts.ApiRoutes;
using Customer.Contracts.Customer.GetByEMail;
using Customer.Contracts.Customer.Insert;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Customer.Tests.WebApi.Tests
{
    [TestCaseOrderer("PriorityOrderer", "Customer.Tests")]
    public class ApiCustomerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public ApiCustomerTests(WebApplicationFactory<Startup> factory)
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
        public async Task Get_Customer_ByEMail()
        {
            // Arrange        
            await Task.CompletedTask;
            var client = _factory.CreateClient();
            var request = new GetByEMailRequest("something@email.com");

            // Act
            var response =
                Should.CompleteIn(async () =>
                {
                    return await client.PostAsync(ApiRoutes.Customer.GetByEMail, GetHttpContentForJson(request));
                },
                    TimeSpan.FromSeconds(30)
                );


            // Assert
            response.ShouldNotBeNull();
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }


        [Theory, TestPriority(1)]
        [InlineData("erkan", "uygun", "euygun@gmail.com")]
        public async Task Insert_Customer_test(string firstName, string lastName, string email)
        {
            // Arrange
            await using var application = new WebApplicationFactory<Startup>();
            using var client = application.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

            InsertRequest request = new InsertRequest(firstName, lastName, email);

            // Act
            var response = await client.PostAsync(ApiRoutes.Customer.Insert, GetHttpContentForJson(request));

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }


        [Theory, TestPriority(2)]
        [InlineData("erkan", "uygun", "erkan@gmail.com")]
        public async Task Insert_and_GetByEMail_Customer_test(string firstName, string lastName, string email)
        {
            // Arrange
            await Task.CompletedTask;
            var client = _factory.CreateClient();

            var request = new InsertRequest(firstName, lastName, email);
            var getByEMailRequest = new GetByEMailRequest(email);

            // Act
            var responseInsert =
                Should.CompleteIn(async () =>
                {
                    return await client.PostAsync(ApiRoutes.Customer.Insert, GetHttpContentForJson(request));
                }, TimeSpan.FromSeconds(5));
            var resultInsert = Deserialize<InsertResponse>(responseInsert);

            var responseGetByEMail =
                Should.CompleteIn(async () =>
                {
                    return await client.PostAsync(ApiRoutes.Customer.GetByEMail, GetHttpContentForJson(getByEMailRequest));
                }, TimeSpan.FromSeconds(5));
            var resultGetByEMail = Deserialize<GetByEMailResponse>(responseGetByEMail);


            // Assert
            responseInsert.StatusCode.ShouldBe(HttpStatusCode.OK);
            responseGetByEMail.StatusCode.ShouldBe(HttpStatusCode.OK);

            resultInsert.CustomerId.ShouldNotBe(Guid.Empty);
            resultGetByEMail.CustomerId.ShouldNotBe(Guid.Empty);
        }
    }
}