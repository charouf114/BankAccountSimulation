using Application.Authentification.Commands.Authenticate;
using Domain.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using HttpMethod = System.Net.Http.HttpMethod;

namespace IntegrationTests
{
    internal static class HttpClientExtentions
    {
        public static async Task<HttpResponseMessage> CallHttpMethodAsync<T>(this HttpClient httpClient, HttpMethod httpMethod, string userToken, string? requestUri, T value)
        {
            var request = new HttpRequestMessage(httpMethod, requestUri);
            request.Content = JsonContent.Create(value);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

            return await httpClient.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> Authenticate(this HttpClient httpClient, AuthenticateCommand authenticateCommand)
        {
            return await httpClient.PostAsJsonAsync("api/auth/Authenticate", authenticateCommand);
        }

        public static async Task<HttpResponseMessage> DepositMoney(this HttpClient httpClient, string userToken, TransactionInput transactionInput)
        {
            return await httpClient.CallHttpMethodAsync(HttpMethod.Post, userToken, "api/card/Deposit", transactionInput);
        }

        public static async Task<HttpResponseMessage> WithDrawalMoney(this HttpClient httpClient, string userToken, TransactionInput transactionInput)
        {
            return await httpClient.CallHttpMethodAsync(HttpMethod.Post, userToken, "api/card/WithDrawal", transactionInput);
        }

        public static async Task<HttpResponseMessage> GetHistory(this HttpClient httpClient, string userToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/card/History");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
            return await httpClient.SendAsync(request);
        }
    }
}
