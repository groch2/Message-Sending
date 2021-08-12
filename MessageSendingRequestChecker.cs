namespace MessageSending
{
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class MessageSendingRequestChecker : IMessageSendingRequestChecker
    {
        private readonly HttpClient _recaptchaApiClient;
        private readonly string _checkerServiceSecretKey;
        private readonly ILogger _logger;

        public MessageSendingRequestChecker(
            HttpClient recaptchaApiClient,
            ICheckerServiceConfiguration checkerServiceConfiguration,
            ILogger<MessageSendingRequestChecker> logger)
        {
            recaptchaApiClient.BaseAddress =
                new System.Uri("https://www.google.com/recaptcha/api/");
            _recaptchaApiClient = recaptchaApiClient;
            _checkerServiceSecretKey = checkerServiceConfiguration.SecretKey;
            _logger = logger;
        }

        public async Task<RecaptchaVerifyResponse> CheckMessageSendingRequest(
            string token,
            string remoteIPAddress)
        {
            var content = new FormUrlEncodedContent(new[] {
                KeyValuePair.Create("secret", _checkerServiceSecretKey),
                KeyValuePair.Create("response", token),
                KeyValuePair.Create("remoteip", remoteIPAddress)
            });
            const string contentTypeHeaderKey = "Content-Type";
            content.Headers.Remove(contentTypeHeaderKey);
            content.Headers.Add(contentTypeHeaderKey, "application/x-www-form-urlencoded");
            using var httpResponse =
                await _recaptchaApiClient
                    .PostAsync("siteverify", content);
            var recaptchaVerifyResponse =
                await httpResponse.Content
                    .ReadFromJsonAsync<RecaptchaVerifyResponse>();
            if (!recaptchaVerifyResponse.Success)
            {
                var response = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogError(response);
            }
            return recaptchaVerifyResponse;
        }
    }
}