using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Messenger.Webhook.DotNet.Sample.Services
{
    /// <summary>
    /// https://developers.facebook.com/docs/messenger-platform/reference/send-api/
    /// </summary>
    public class FacebookApi
    {
        private const string BASE_API_URL = "https://graph.facebook.com/v2.6/me/messages";

        private string AccessToken { get; set; }

        public FacebookApi()
        {
            LoadConfiguration();
        }

        public async Task<bool> SendMessage(string recipientId, string text)
        {
            string url = $"{BASE_API_URL}?access_token={AccessToken}";
            bool isSuccessful = false;

            using(HttpClient http = new HttpClient())
            {
                object requestBody = new
                {
                    messaging_type = "RESPONSE",
                    recipient = new
                    {
                        id = recipientId
                    },
                    message = new
                    {
                        text = text
                    }
                };

                HttpResponseMessage apiResponse = await http.PostAsJsonAsync(url, requestBody);
                isSuccessful = apiResponse.IsSuccessStatusCode;
            }

            return isSuccessful;
        }

        private void LoadConfiguration()
        {
            AccessToken = ConfigurationManager.AppSettings["FbPageAccessToken"];
        }
    }
}