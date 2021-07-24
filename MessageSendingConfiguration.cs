using Microsoft.Extensions.Configuration;
using System;

namespace MessageSending
{
    public class MessageSendingConfiguration : IMessageSendingConfiguration
    {
        public MessageSendingConfiguration(IConfiguration configuration)
        {
            EmailAddress = Environment.GetEnvironmentVariable("emailAddress");
        }

        public string EmailAddress { private set; get; }
    }
}
