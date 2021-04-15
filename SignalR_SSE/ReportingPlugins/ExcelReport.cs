using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignalR_SSE.ReportingPlugins
{
    public class ExcelReport : IReportingAction
    {
        public string PluginName => "Excel";

        public Task<HttpResponseMessage> RunReportAsync(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var filestream = System.IO.File.OpenRead(@"C:\Users\magdalena.leisegang\Documents\Visual Studio 2017\Projects\SignalR_SSE\SignalR_SSE\testexcel.xlsx");
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(filestream);
            result.Content.Headers.ContentType =
        new MediaTypeHeaderValue("application/vnd.ms-excel");
            return Task.FromResult(result);
        }
    }
}