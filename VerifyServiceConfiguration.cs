namespace MessageSending
{
    using Amazon.SecretsManager;
    using Amazon.SecretsManager.Model;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class VerifyServiceConfiguration : IVerifyServiceConfiguration
    {
        private readonly IAmazonSecretsManager _amazonSecretsManagerClient;
        private readonly ILogger _logger;

        public VerifyServiceConfiguration(
            IConfiguration configuration,
            IAmazonSecretsManager amazonSecretsManagerClient,
            IEnvironmentConfiguration environmentConfiguration,
            ILogger<VerifyServiceConfiguration> logger)
        {
            _amazonSecretsManagerClient = amazonSecretsManagerClient;
            _logger = logger;
            SecretKey =
                environmentConfiguration.IsProduction ?
                GetRecaptchaSecretKey().Result :
                configuration["verifyServiceSecretKey"];
            logger.LogInformation(new { environmentConfiguration.IsProduction }.ToString());
        }

        public string SecretKey { private set; get; }

        async private Task<string> GetRecaptchaSecretKey()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = "MorganSite/RecaptchaSecretKey",
                VersionStage = "AWSCURRENT"
            };
            var secretValueResponse = await _amazonSecretsManagerClient.GetSecretValueAsync(request);
            _logger.LogInformation(new { secretValueRequestId = secretValueResponse.ResponseMetadata.RequestId }.ToString());
            var secretValueResponseMetadata = JsonConvert.SerializeObject(secretValueResponse.ResponseMetadata.Metadata);
            _logger.LogInformation(secretValueResponseMetadata);

            var recaptchaSecretKey = 
                JsonConvert.DeserializeAnonymousType(secretValueResponse.SecretString, new { RecaptchaSecretKey = "" }).RecaptchaSecretKey;
            return recaptchaSecretKey;
        }
    }
}
