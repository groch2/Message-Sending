using Microsoft.Extensions.Configuration;
namespace MessageSending
{
    public class MessageSendingConfiguration : IMessageSendingConfiguration
    {
        public MessageSendingConfiguration(IConfiguration configuration)
        {
            EmailAddress = configuration["emailAddress"];
        }

        public string EmailAddress { private set; get; }
    }
}
