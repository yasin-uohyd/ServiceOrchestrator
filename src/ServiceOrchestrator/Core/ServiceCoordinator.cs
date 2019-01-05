namespace ServiceOrchestrator.Core
{
    public class ServiceCoordinator : IServiceCoordinator
    {
        public void Raise<T>(object[] data) where T : IServiceCoordinator
        {

        }
    }
}
