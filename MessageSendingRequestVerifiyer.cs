﻿namespace MessageSending
{
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class MessageSendingRequestVerifiyer : IMessageSendingRequestVerifiyer
    {
        private readonly HttpClient _recaptchaApiClient;
        private readonly string _verifyServiceSecretKey;
        private readonly ILogger _logger;

        public MessageSendingRequestVerifiyer(
            IHttpClientFactory httpClientFactory,
            IVerifyServiceConfiguration verifyServiceConfiguration,
            ILogger<MessageSendingRequestVerifiyer> logger)
        {
            _recaptchaApiClient = httpClientFactory.CreateClient(Constants.RecaptchaApiClient);
            _verifyServiceSecretKey = verifyServiceConfiguration.SecretKey;
            _logger = logger;
        }

        public async Task<RecaptchaVerifyResponse> VerifiyMessageSendingRequest(string token, string remoteIPAddress)
        {
            var content = new FormUrlEncodedContent(new[] {
                KeyValuePair.Create("secret", _verifyServiceSecretKey),
                KeyValuePair.Create("response", token),
                KeyValuePair.Create("remoteip", remoteIPAddress)
            });
            const string contentTypeHeaderKey = "Content-Type";
            content.Headers.Remove(contentTypeHeaderKey);
            content.Headers.Add(contentTypeHeaderKey, "application/x-www-form-urlencoded");
            var httpResponse =
                await _recaptchaApiClient
                    .PostAsync("siteverify", content);
            var recaptchaVerifyResponse =
                await httpResponse.Content
                    .ReadFromJsonAsync<RecaptchaVerifyResponse>();
            if (!recaptchaVerifyResponse.Success)
            {
                var response = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation(response);
            }
            return recaptchaVerifyResponse;
        }
    }
}