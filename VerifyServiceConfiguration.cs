namespace MessageSending
{
    using Microsoft.Extensions.Configuration;

    public class VerifyServiceConfiguration : IVerifyServiceConfiguration
    {
        public VerifyServiceConfiguration(IConfiguration configuration)
        {
            SecretKey = configuration["verifyServiceSecretKey"];
        }

        public string SecretKey { private set; get; }
    }
}
