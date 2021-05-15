using System;
using System.Collections.Generic;
using System.IO;
using ExtractApifyResults;
using ExtractApifyResults.Interfaces;
using ExtractApifyResults.Services;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MimeKit.Utils;
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

        public MimeMessage MakeMessage(List<MemoryStream> memStreamList)
        {
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress (_emailSecrets.Value.FromName, _emailSecrets.Value.FromAddress));
            message.To.Add (new MailboxAddress (_emailSecrets.Value.ToName, _emailSecrets.Value.ToAddress));
            message.Subject = "Apify Results";

            // create our message text, just like before (except don't set it as the message.Body)
            var body = new TextPart ("plain") {
                Text = @"Apify Results"};

            // create an image attachment for the file located at path
            var attachment = new MimePart ("text", "html") {
                Content = new MimeContent (memStreamList[0], ContentEncoding.Default),
                ContentDisposition = new ContentDisposition (ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "HtmlResults"
            };

             var attachment2 = new MimePart ("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                Content = new MimeContent (memStreamList[1], ContentEncoding.Default),
                ContentDisposition = new ContentDisposition (ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "Xlsx results"
            };

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart ("mixed");
            multipart.Add (body);
            multipart.Add (attachment);
            multipart.Add (attachment2);

            // now set the multipart/mixed as the message body
            message.Body = multipart;








            // var builder = new BodyBuilder();
            // builder.TextBody = @"Apify Results";

            // builder.HtmlBody = string.Format(@"<br>Apify Results<br>");

            // builder.LinkedResources.Add("Html Results", memStreamList[0]);
            // builder.LinkedResources[0].ContentId = MimeUtils.GenerateMessageId();
            // builder.Attachments.Add("Html Results", memStreamList[0]);

            // builder.LinkedResources.Add("Xlsx Results", memStreamList[1]);
            // builder.LinkedResources[1].ContentId = MimeUtils.GenerateMessageId();
            // builder.Attachments.Add("Xlsx Results", memStreamList[1]);

            // message.Body = builder.ToMessageBody();
            return message;

        }

        public void Send(List<MemoryStream> memStream)
        {
            try
            {
            var message = MakeMessage(memStream);

                using(var client = new SmtpClient())
                {
                    client.Connect(_emailSecrets.Value.SmtpServer, _emailSecrets.Value.Port, true);
                    client.Authenticate(_emailSecrets.Value.UserName, _emailSecrets.Value.Password);

                    client.Send(message);
                    client.Disconnect(true);

                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error happened");
            }
        }
    }
}