namespace MessageSending.Controllers
{
    using Amazon.SimpleEmailV2;
    using Amazon.SimpleEmailV2.Model;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AmazonMessage = Amazon.SimpleEmailV2.Model.Message;

    [ApiController]
    [Route("/")]
    public class Controller : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAmazonSimpleEmailServiceV2 _emailService;
        private readonly string emailAddress;

        public Controller(
            ILogger<Controller> logger,
            IAmazonSimpleEmailServiceV2 emailService,
            IMessageSendingConfiguration messageSendingConfiguration)
        {
            _logger = logger;
            _emailService = emailService;
            emailAddress = messageSendingConfiguration.EmailAddress;
        }

        public async Task<IActionResult> Post([FromBody] MessageSending.Message message)
        {
            _logger.LogInformation(message.ToString());
            await SendMessage(message.From, message.Subject, message.Body);
            return Ok();
        }

        private async Task<SendEmailResponse> SendMessage(
            string from,
            string subject,
            string body)
        {
            var sendRequest = new SendEmailRequest
            {
                FromEmailAddress = emailAddress,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { emailAddress }
                },
                Content = new EmailContent
                {
                    Simple = new AmazonMessage
                    {
                        Subject = new Content { Data = subject },
                        Body = new Body
                        {
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = $"{from}{Environment.NewLine}{body}"
                            }
                        }
                    }
                },
            };
            return await _emailService.SendEmailAsync(sendRequest);
        }
    }
}
