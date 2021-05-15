using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExtractApifyResults.Contracts;
using ExtractApifyResults.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExtractApifyResults.Services
{
    public class EMail : IEmail
    {
        ILogger<EMail> _logger;
       private readonly IOptions<ExtractApifyResultsConfiguration> _earConfig;
        private readonly IOptions<EmailSecretsConfiguration> _emailSecrets;
        private readonly IConfiguration _config;
        private readonly ITransport _transport;

        public EMail(ILogger<EMail> logger, IConfiguration config,  IOptions<ExtractApifyResultsConfiguration> earConfig,
            IOptions<EmailSecretsConfiguration> emailSecrets, ITransport transport)
        {
            _logger = logger;
            _config = config;
            _earConfig = earConfig;
            _emailSecrets = emailSecrets;
            _transport = transport;
        }

        public async Task Send(List<ApifyTaskResult> apifyTaskResults)
        {
            try
            {
            var message = await MakeMessage(apifyTaskResults);

                using(var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailSecrets.Value.SmtpServer, _emailSecrets.Value.Port, true);
                    await client.AuthenticateAsync(_emailSecrets.Value.UserName, _emailSecrets.Value.Password);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error happened");
            }
        }

        public async Task<MimeMessage> MakeMessage(List<ApifyTaskResult> apifyTaskResults)
        {
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress (_emailSecrets.Value.FromName, _emailSecrets.Value.FromAddress));
            message.To.Add (new MailboxAddress (_emailSecrets.Value.ToName, _emailSecrets.Value.ToAddress));
            message.Subject = "Apify Results";

            var body = new TextPart ("plain") {
                Text = @"Apify Results"};

            var multipart = new Multipart ("mixed");
            multipart.Add (body);

            foreach(ApifyTaskResult atr in apifyTaskResults)
            {
                MimePart attachment;
                switch(atr.MimeType)
                {
                    case MimeTypeEnum.html:
                    attachment = new MimePart ("text", "html");
                    await FillAttachment(attachment, atr);
                    multipart.Add(attachment);
                    break;

                    case MimeTypeEnum.xlsx:
                    attachment = new MimePart ("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    await FillAttachment(attachment, atr);
                    multipart.Add(attachment);
                    break;
                }

            }

            message.Body = multipart;

            return message;
        }

        private async Task FillAttachment(MimePart attachment, ApifyTaskResult atr)
        {
                attachment.Content = new MimeContent (atr.TaskResult, ContentEncoding.Default);
                attachment.ContentDisposition = new ContentDisposition (ContentDisposition.Attachment);
                attachment.ContentTransferEncoding = ContentEncoding.Base64;
                attachment.FileName = atr.Name;
        }
    }
}