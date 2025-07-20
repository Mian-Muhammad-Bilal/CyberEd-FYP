using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
namespace GroqApiLibrary
{
    public interface IGroqApiClient
    {
        Task<JObject> CreateChatCompletionAsync(JObject request);
    }
}