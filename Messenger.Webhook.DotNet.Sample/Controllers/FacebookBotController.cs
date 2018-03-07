using Messenger.Webhook.DotNet.Sample.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Messenger.Webhook.DotNet.Sample.Controllers
{
    [RoutePrefix("api/fb")]
    public class FacebookBotController : ApiController
    {
        private const string MODE_QS_KEY = "hub.mode";
        private const string TOKEN_QS_KEY = "hub.verify_token";
        private const string CHALLENGE_QS_KEY = "hub.challenge";
        
        private const string VERIFY_TOKEN = "98ab9ed9-8f7b-408f-80a9-44cad8e75ba8";
        
        [Route("webhook")]
        public HttpResponseMessage Get()
        {
            // Get query string parameters
            string mode = GetQueryStringParameter(MODE_QS_KEY);
            string token = GetQueryStringParameter(TOKEN_QS_KEY);
            string challenge = GetQueryStringParameter(CHALLENGE_QS_KEY);

            // Check that mode and token are correct
            if (mode == "subscribe" && token == VERIFY_TOKEN)
            {
                // Respond with the challenge token from the request
                HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.OK);
                resp.Content = new StringContent(challenge, Encoding.UTF8, "text/plain");

                return resp;
            }

            // Respond with '403 Forbidden' as fallback behavior
            // (e.g.: mode is not 'subscribe', token does not match, etc...)
            return Request.CreateResponse(HttpStatusCode.Forbidden);
        }

        [Route("webhook")]
        public async Task<HttpResponseMessage> Post(dynamic body)
        {
            // Checks this is an event from a page subscription
            if (body.@object == "page")
            {
                // Iterates over each entry - there may be multiple if batched
                foreach (var entry in body.entry)
                {
                    // Gets the message. entry.messaging is an array, but 
                    // will only ever contain one message, so we get index 0
                    dynamic messagingEntry = entry.messaging[0];
                    string senderId = messagingEntry.sender.id;
                    string messageText = messagingEntry.message.text;

                    FacebookApi fbApi = new FacebookApi();
                    await fbApi.SendMessage(senderId, $"Message received: {messageText}");
                }

                // Returns a '200 OK' response to all requests
                return Request.CreateResponse(HttpStatusCode.OK, "EVENT_RECEIVED");
            }
            else
            {
                // Returns a '404 Not Found' if event is not from a page subscription
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        
        private string GetQueryStringParameter(string parameterName)
        {
            IEnumerable<KeyValuePair<string, string>> queryString = Request.GetQueryNameValuePairs();
            return queryString.Any(qs => qs.Key == parameterName)
                ? queryString.Single(qs => qs.Key == parameterName).Value
                : null;
        }
    }
}