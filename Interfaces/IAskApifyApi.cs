using System.IO;
using System.Threading.Tasks;
using ExtractApifyResults.Contracts;
using ExtractApifyResults.Contracts.LastTaskRunContract;
using ExtractApifyResults.Contracts.TaskObject;

namespace ExtractApifyResults.Interfaces
{
    public interface IAskApifyApi
    {
        Task<LastTaskRunContract> GetLastRunningTask(string task);
        Task<ApifyTaskResult> GetApifyTaskResult(string task, MimeTypeEnum format);
        Task<MemoryStream> GetTaskResult(string task, MimeTypeEnum format);
    }
}