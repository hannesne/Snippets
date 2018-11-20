using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Echo
{
    public static class Echo
    {
        [FunctionName("Echo")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("Executing Echo Function");

            // ReadAsAsync is not atomic on streams, so we only want to do this once.
            dynamic data = await req.Content.ReadAsAsync<object>();

            string expectedMessage = await ExtractParameter(req, "message", data);
            string expectedResult = await ExtractParameter(req, "result", data);

            if (!Enum.TryParse(expectedResult, out HttpStatusCode httpResult))
                httpResult = HttpStatusCode.OK;

            return expectedMessage == null ? 
                req.CreateResponse(httpResult) : 
                req.CreateResponse(httpResult,expectedMessage);
            
        }

        private static async Task<string> ExtractParameter(HttpRequestMessage req, string parameterName, dynamic data)
        {
            string parameter = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, parameterName, StringComparison.OrdinalIgnoreCase) == 0)
                .Value;

            if (parameter != null)
                return parameter;
            
            if (data != null)
            {
                parameter = data[parameterName];
            }

            return parameter;
        }
    }
}
