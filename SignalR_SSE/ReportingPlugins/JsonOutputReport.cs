using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace SignalR_SSE.ReportingPlugins
{
    public class JsonOutputReport : IReportingAction
    {
        public string PluginName => "Json";

        public Task<HttpResponseMessage> RunReportAsync(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            JsonSerializer ser = new JsonSerializer();

            var resp =
                new JsonReport()
                {
                    message = "you got me",
                    p = int.Parse(parameters.First(x => x.Key == "p").Value),
                    p2 = int.Parse(parameters.First(x => x.Key == "p2").Value)
                };

            string jsonresp = JsonConvert.SerializeObject(resp);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(jsonresp, System.Text.Encoding.UTF8, "application/json");
            return Task.FromResult(result);
        }
    }

    public class JsonReport
    {
        public string message { get; set; }
        public int p { get; set; }
        public int p2 { get; set; }
    }
}