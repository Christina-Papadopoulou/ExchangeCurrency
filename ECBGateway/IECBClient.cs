using System.Collections.Generic;
using System.Threading.Tasks;
using ECBGateway.Model;

namespace ECBGateway
{
    public interface IECBClient
    {
        Task<IEnumerable<ECBRate>> GetLatestRatesAsync();
    }
}
