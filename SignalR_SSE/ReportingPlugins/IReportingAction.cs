using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SignalR_SSE.ReportingPlugins
{
    public interface IReportingAction
    {
        string PluginName { get; }
        Task<HttpResponseMessage> RunReportAsync(IEnumerable<KeyValuePair<string, string>> parameters);
    }
}
