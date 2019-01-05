namespace ServiceOrchestrator.Core
{
    public interface IServiceCoordinator
    {
        void Raise<T>(object[] data) where T : IServiceCoordinator;
    }
}
