namespace MessageSending
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class MessageSendingRequestVerifiyer : IMessageSendingRequestVerifiyer
    {
        private readonly HttpClient _recaptchaApiClient;
        private readonly string _verifyServiceSecretKey;

        public MessageSendingRequestVerifiyer(
            IHttpClientFactory httpClientFactory,
            IVerifyServiceConfiguration verifyServiceConfiguration)
        {
            _recaptchaApiClient = httpClientFactory.CreateClient(Constants.RecaptchaApiClient);
            _verifyServiceSecretKey = verifyServiceConfiguration.SecretKey;
        }

        public async Task<bool> VerifiyMessageSendingRequest(string token, string remoteIPAddress)
        {
            var content = new FormUrlEncodedContent(new[] {
                KeyValuePair.Create("secret", _verifyServiceSecretKey),
                KeyValuePair.Create("response", token),
                KeyValuePair.Create("Remoteip", remoteIPAddress)
            });
            const string contentTypeHeaderKey = "Content-Type";
            content.Headers.Remove(contentTypeHeaderKey);
            content.Headers.Add(contentTypeHeaderKey, "application/x-www-form-urlencoded");
            var httpResponse =
                await _recaptchaApiClient
                    .PostAsync("siteverify", content);
#if DEBUG
            var response = await httpResponse.Content.ReadAsStringAsync();
            System.Console.WriteLine(response);
#endif
            var recaptchaVerifyResponse =
                await httpResponse.Content
                    .ReadFromJsonAsync<RecaptchaVerifyResponse>();
            return recaptchaVerifyResponse.Success;
        }

        class RecaptchaVerifyRequest
        {
            public string Secret { get; set; }
            public string Response { get; set; }
            public string Remoteip { get; set; }
        }

        class RecaptchaVerifyResponse
        {
            public bool Success { get; set; }
            public string Challenge_ts { get; set; }
            public string Hostname { get; set; }
        }
    }
}