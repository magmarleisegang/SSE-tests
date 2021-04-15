using SignalR_SSE.SSE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SignalR_SSE.Controllers
{
    public class ValuesController : ApiController
    {
        private int? MyConnectionId;


        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            //Request.GetClientCertificate().
            // TODO: authorize user (out of the post scope)
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            MyConnectionId = id;
            response.Content =
                new PushStreamContent((Action<Stream, HttpContent, TransportContext>)OnStreamAvailable, "text/event-stream");
            response.Headers.Add("Connection", "keep-alive");
            ConnectionTracker.Instance.AddConnection(id);

            return response;
        }
        private void OnStreamAvailable(Stream stream, HttpContent content, TransportContext context)
        {
            ManualResetEvent wait = new ManualResetEvent(false);
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 8000, true))
            {
                var unansweredHeartbeats = 0;
                var unanswereMessages = 0;
                writer.NewLine = "\n";
                WriteEvent(writer, "helo", "");

                NotificationService.Instance.OnNotify += (object sender, EventArgs e) =>
                {
                    if (((NotifyEventArgs)e).Id.HasValue && ((NotifyEventArgs)e).Id != MyConnectionId)
                        return;
                    try
                    {
                        var message = ((NotifyEventArgs)e).Message; // get JSON 
                        WriteEvent(writer, "hit", message);// mark event as our custom event "log"
                        unanswereMessages = 0;
                    }
                    catch (HttpException ex)
                    {
                        if (unanswereMessages < 3)
                        {
                            unanswereMessages++;
                        }
                        else
                        {
                            // if client terminate connection then we'll know about it on Flush. 
                            // we'll get a HttpException: 'The remote host closed the connection. The error code is 0x800704CD.'
                            wait.Set();
                            ConnectionTracker.Instance.RemoveConnection(MyConnectionId);
                        }
                    }
                };

                bool waiting = true;
                // wait a Heartbeat timeout, after that send an empty msg to check connection
                while (waiting)
                {
                    if (!wait.WaitOne(3000))
                    {
                        try
                        {
                            WriteEvent(writer, "heartbeat", "");
                        }
                        catch (HttpException ex)
                        {
                            if (unansweredHeartbeats < 3)
                            {
                                unansweredHeartbeats++;
                            }
                            else
                            {
                                //client disconnected
                                wait.Set();
                                waiting = false;
                                ConnectionTracker.Instance.RemoveConnection(MyConnectionId);

                            }
                        }
                    }
                }
            }
        }
        private static void WriteEvent(TextWriter writer, string eventType, string data)
        {
            if (!string.IsNullOrEmpty(eventType))
            {
                writer.WriteLine("event:" + eventType);
            }
            writer.WriteLine("data:" + data ?? "");
            writer.WriteLine();
            writer.Flush(); // StreamWriter.Flush calls Flush on underlying Stream
        }

    }
}
