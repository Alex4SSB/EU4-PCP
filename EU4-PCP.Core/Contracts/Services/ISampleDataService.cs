using System.Collections.Generic;
using System.Threading.Tasks;

using EU4_PCP.Core.Models;

namespace EU4_PCP.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetGridDataAsync();
    }
}
