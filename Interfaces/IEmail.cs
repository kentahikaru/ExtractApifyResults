using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ExtractApifyResults.Contracts;
using MimeKit;

namespace ExtractApifyResults.Interfaces
{
    public interface IEmail
    {
        Task<MimeMessage> MakeMessage(List<ApifyTaskResult> memStreamList);
        Task Send(List<ApifyTaskResult> memStream);
    }
}