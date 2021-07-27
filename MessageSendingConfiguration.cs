namespace MessageSending
{
    using Microsoft.Extensions.Configuration;

    public class MessageSendingConfiguration : IMessageSendingConfiguration
    {
        public MessageSendingConfiguration(IConfiguration configuration)
        {
            EmailAddress = configuration["emailAddress"];
        }

        public string EmailAddress { private set; get; }
    }
}
