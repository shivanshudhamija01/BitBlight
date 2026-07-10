namespace Infrastructure.Services.Interfaces
{
    public interface IPauseRegistry
    {
        void Register(IPausable pausable);
        void Unregister(IPausable pausable);
    }
}