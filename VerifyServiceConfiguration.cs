namespace MessageSending
{
    using Amazon.SecretsManager;
    using Amazon.SecretsManager.Model;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class VerifyServiceConfiguration : IVerifyServiceConfiguration
    {
        private readonly IAmazonSecretsManager _amazonSecretsManagerClient;

        public VerifyServiceConfiguration(
            IConfiguration configuration,
            IAmazonSecretsManager amazonSecretsManagerClient,
            IEnvironmentConfiguration environmentConfiguration)
        {
            _amazonSecretsManagerClient = amazonSecretsManagerClient;
            SecretKey =
                environmentConfiguration.IsProduction ?
                GetRecaptchaSecretKey().Result :
                configuration["verifyServiceSecretKey"];
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

            return
                JsonConvert.DeserializeAnonymousType(
                    secretValueResponse.SecretString, new { RecaptchaSecretKey = "" })
                .RecaptchaSecretKey;
        }
    }
}
