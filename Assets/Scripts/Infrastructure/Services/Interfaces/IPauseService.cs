namespace Infrastructure.Services.Interfaces
{
    public interface IPauseService
    {
        void Pause();
        void Resume();
        void Reset();
        void SetPausedState(bool isPaused);
    }
}