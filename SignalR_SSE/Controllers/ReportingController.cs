using SignalR_SSE.ReportingPlugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SignalR_SSE.Controllers.Api
{
    public class ReportingController : ApiController
    {

        public ReportingController()
        {
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Get(string pluginName)
        {
            var parameters = Request.GetQueryNameValuePairs();
            if (!string.IsNullOrEmpty(pluginName))
            {
                if (ReportPluginsManager.PluginExists(pluginName))
                {
                    var result = await (ReportPluginsManager.GetPlugin(pluginName)).RunReportAsync(parameters);
                    //var response = Request.CreateResponse(HttpStatusCode.OK, result.Data, result.ContentType);
                    return result;
                }

            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"Invalid PluginName");
        }
    }
}
