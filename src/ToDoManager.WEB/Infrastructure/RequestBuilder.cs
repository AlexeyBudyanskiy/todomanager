using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;
using ToDoManager.WEB.DataAccess.Entities;
using ToDoManager.WEB.Infrastructure.Exceptions;
using ToDoManager.WEB.Infrastructure.TransientFaultHandling;

namespace ToDoManager.WEB.Infrastructure
{
    public class RequestBuilder
    {
        public static HttpResponseMessage MakeGetRequest(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var responseMessage = client.GetAsync(url).Result;

            return responseMessage;           
        }

        public static async Task<HttpResponseMessage> MakeGetRequestAsync(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var responseMessage = await client.GetAsync(url);

            return responseMessage;
        }

        public static List<Assignment> GetAssignments(string url)
        {
            var result = new List<Assignment>();
            var httpRetryPolicy = RetryPolicyFactory.MakeExponentialHttpRetryPolicy();
            
            httpRetryPolicy.ExecuteAction(() =>
            {
                var responseMessage = MakeGetRequest(url);

                if (responseMessage.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<List<Assignment>>(
                        responseMessage.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    throw new TransientFaultException("Unsuccesfull request", responseMessage.StatusCode);
                }
            });

            return result;
        }

        public static async Task<List<Assignment>> GetAssignmentsAsync(string url)
        {
            var result = new List<Assignment>();
            var httpRetryPolicy = RetryPolicyFactory.MakeExponentialHttpRetryPolicy();

            await httpRetryPolicy.ExecuteAsync(async () =>
            {
                var responseMessage = await MakeGetRequestAsync(url);

                if (responseMessage.IsSuccessStatusCode)
                {
                   result = JsonConvert.DeserializeObject<List<Assignment>>(
                        await responseMessage.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new TransientFaultException("Unsuccesfull request", responseMessage.StatusCode);
                }
            });

            return result;
        }

        public static List<Assignment> GetAssignmentsPolly(string url)
        {
            var result = new List<Assignment>();
            var policy = Policy.HandleResult(HttpStatusCode.BadRequest).Retry(3);

            policy.Execute(() =>
            {
                var responseMessage = MakeGetRequest(url);

                if (responseMessage.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<List<Assignment>>(
                        responseMessage.Content.ReadAsStringAsync().Result);
                }

                return responseMessage.StatusCode;
            });

            return result;
        }
    }
}
