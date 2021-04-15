using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SignalR_SSE.ReportingPlugins
{
    public class HelpReport : IReportingAction
    {
        public string PluginName => "Help";

        public async Task<HttpResponseMessage> RunReportAsync(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent("You have been helped");
            return result;
        }
    }
}