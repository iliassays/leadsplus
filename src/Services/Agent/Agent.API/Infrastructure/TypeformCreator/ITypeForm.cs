using System.Threading.Tasks;

namespace Agent.TypeFormIntegration
{
    public interface ITypeForm
    {
        Task<string> GetTypeFormAsync(string url);
        Task<string> CreateTypeFormAsync(string typeFormContent);
    }
}

