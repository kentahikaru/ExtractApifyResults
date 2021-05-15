using System.Collections.Generic;
using System.IO;
using MimeKit;

namespace ExtractApifyResults.Interfaces
{
    public interface IEmail
    {
        MimeMessage MakeMessage(List<MemoryStream> memStreamList);
        void Send(List<MemoryStream> memStream);
    }
}