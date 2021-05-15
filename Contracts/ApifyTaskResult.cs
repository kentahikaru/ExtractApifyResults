using System.IO;

namespace ExtractApifyResults.Contracts
{
    public class ApifyTaskResult
    {
        public string Name {get;set;}
        public MimeTypeEnum MimeType {get;set;}
        public MemoryStream TaskResult {get;set;}
    }
}