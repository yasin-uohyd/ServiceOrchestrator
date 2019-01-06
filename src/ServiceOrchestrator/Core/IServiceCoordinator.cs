using System.Threading.Tasks;
using ServiceOrchestrator.Events;

namespace ServiceOrchestrator.Core
{
    public interface IServiceCoordinator
    {
        Task Raise<T>(object data) where T : IServiceEvent, new();
    }
}
