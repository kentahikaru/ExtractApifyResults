using System.IO;
using System.Threading.Tasks;
using ExtractApifyResults.Contracts;

namespace ExtractApifyResults.Interfaces
{
    public interface IAskApifyApi
    {
        Task<LastTaskRunContract> GetLastRunningTask(string task);
        Task<MemoryStream> GetTaskResult(string task, string format);
    }
}