using LiveTelemetryWebUi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiveTelemetryWebUi.Interfaces
{
    public interface IApiService
    {
        Task<IEnumerable<LiveSession>> GetSessions();
    }
}
