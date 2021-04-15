using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;

namespace SignalR_SSE.ReportingPlugins
{
    public class MyCustomContentNegotiator : DefaultContentNegotiator
    {
        protected override MediaTypeFormatterMatch MatchRequestMediaType(HttpRequestMessage request, MediaTypeFormatter formatter)
        {
            return null;
        }
    }
}