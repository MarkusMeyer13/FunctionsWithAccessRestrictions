using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyFunctionProj
{
    public static class HttpExample
    {
        [FunctionName("HttpExample")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var remoteAddress = req.HttpContext.Connection.RemoteIpAddress;

            JObject outputData = new JObject();
            try
            {
                try
                {
                    outputData["AddressFamily"] = remoteAddress.AddressFamily.ToString();
                    outputData["IsIPv6SiteLocal"] = remoteAddress.IsIPv6SiteLocal;
                    outputData["IsIPv4MappedToIPv6"] = remoteAddress.IsIPv4MappedToIPv6;
                }
                catch (Exception e)
                {
                    outputData["Exception1"] = e.Message;
                }
                try
                {
                    byte[] ipV4Bytes = remoteAddress.MapToIPv4().GetAddressBytes();
                    var ipV4 = $"{ipV4Bytes[0]}.{ipV4Bytes[1]}.{ipV4Bytes[2]}.{ipV4Bytes[3]}";
                    outputData["ipV4"] = ipV4;
                }
                catch (Exception e)
                {
                    outputData["Exception2"] = e.Message;
                }
            }
            catch (Exception e)
            {
                outputData["Exception3"] = e.Message;
            }
            return new OkObjectResult(outputData.ToString());
        }
    }
}
