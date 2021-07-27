namespace MessageSending
{
    using Microsoft.Extensions.Configuration;
    using System;

    public class VerifyServiceConfiguration : IVerifyServiceConfiguration
    {
        public VerifyServiceConfiguration(IConfiguration configuration)
        {
            SecretKey = Environment.GetEnvironmentVariable("verifyServiceSecretKey");
        }

        public string SecretKey { private set; get; }
    }
}
