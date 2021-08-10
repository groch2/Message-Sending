﻿namespace MessageSending.Controllers
{
    using Amazon.SimpleEmailV2;
    using Amazon.SimpleEmailV2.Model;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AmazonMessage = Amazon.SimpleEmailV2.Model.Message;

    [ApiController]
    [Route("/")]
    public class Controller : ControllerBase
    {
        private readonly IAmazonSimpleEmailServiceV2 _emailService;
        private readonly string emailAddress;
        private readonly IMessageSendingRequestVerifiyer _messageSendingRequestVerifiyer;
        private readonly IActionContextAccessor _actionContextAccessor;

        public Controller(
            IAmazonSimpleEmailServiceV2 emailService,
            IMessageSendingConfiguration messageSendingConfiguration,
            IMessageSendingRequestVerifiyer messageSendingRequestVerifiyer,
            IActionContextAccessor actionContextAccessor)
        {
            _emailService = emailService;
            emailAddress = messageSendingConfiguration.EmailAddress;
            _messageSendingRequestVerifiyer = messageSendingRequestVerifiyer;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<IActionResult> Post([FromBody] MessageSendingRequest messageSendingRequest)
        {
            var ipAddress = _actionContextAccessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            var recaptchaVerifyResponse =
                (await _messageSendingRequestVerifiyer
                    .VerifiyMessageSendingRequest(
                        messageSendingRequest.RecaptchaToken,
                        ipAddress));
            if (!recaptchaVerifyResponse.Success || recaptchaVerifyResponse.Score == 0)
            {
                return BadRequest();
            }
            var message = messageSendingRequest.Message;
            await SendMessage(
                message.From,
                message.Subject,
                message.Body);
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
                                Data = $"message from: {from}{Environment.NewLine}{body}"
                            }
                        }
                    }
                },
            };
            return await _emailService.SendEmailAsync(sendRequest);
        }
    }
}
