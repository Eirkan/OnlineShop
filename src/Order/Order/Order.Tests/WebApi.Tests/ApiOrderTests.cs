using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Order.Contracts.ApiRoutes;
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
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange        
            await Task.CompletedTask;
            var client = _factory.CreateClient();


            // Act
            var response =
                Should.CompleteIn(async () =>
                {
                    return await client.GetAsync("/weatherforecast");
                },
                    TimeSpan.FromSeconds(5)
                );


            // Assert
            response.ShouldNotBeNull();
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }


        //[Theory, TestPriority(1)]
        //[InlineData("erkan", "uygun", "euygun@gmail.com", "password")]
        //public async Task Register_user_test(string firstName, string lastName, string email, string password)
        //{
        //    // Arrange
        //    await using var application = new WebApplicationFactory<Startup>();
        //    using var client = application.CreateClient();
        //    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

        //    GetOrdersRequest request = new GetOrdersRequest(firstName, lastName, email, password);

        //    // Act
        //    var response = await client.PostAsync(ApiRoutes.Authentication.Register, GetHttpContentForJson(request));

        //    // Assert
        //    response.StatusCode.ShouldBe(HttpStatusCode.OK);
        //}


        //[Theory, TestPriority(2)]
        //[InlineData("erkan", "uygun", "erkan@gmail.com", "password")]
        //public async Task Register_and_login_user_test(string firstName, string lastName, string email, string password)
        //{
        //    // Arrange
        //    await Task.CompletedTask;
        //    var client = _factory.CreateClient();
        //    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

        //    var request = new GetOrdersRequest(firstName, lastName, email, password);
        //    var loginRequest = new LoginRequest(email, password);

        //    // Act
        //    var responseRegister =
        //        Should.CompleteIn(async () =>
        //        {
        //            return await client.PostAsync(ApiRoutes.Authentication.Register, GetHttpContentForJson(request));
        //        }, TimeSpan.FromSeconds(5));
        //    var resultRegister = Deserialize<GetOrdersResponse>(responseRegister);

        //    var responseLogin =
        //        Should.CompleteIn(async () =>
        //        {
        //            return await client.PostAsync(ApiRoutes.Authentication.Login, GetHttpContentForJson(loginRequest));
        //        }, TimeSpan.FromSeconds(5));
        //    var resultlogin = Deserialize<LoginResponse>(responseLogin);


        //    // Assert
        //    responseRegister.StatusCode.ShouldBe(HttpStatusCode.OK);
        //    responseLogin.StatusCode.ShouldBe(HttpStatusCode.OK);

        //    resultRegister.Token.ShouldNotBeNull();
        //    resultlogin.Token.ShouldNotBeNull();
        //}
    }
}