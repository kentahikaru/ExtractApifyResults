using System.IO;
using System.Threading.Tasks;

namespace ExtractApifyResults.Interfaces
{
    public interface ITransport
    {
        Task<string> GetDataString(string url);
        Task<MemoryStream> GetDataStream(string url);
    }
}