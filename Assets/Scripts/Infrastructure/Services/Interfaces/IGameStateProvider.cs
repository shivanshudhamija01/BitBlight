using Data.Enums;

namespace Infrastructure.Services.Interfaces
{
    public interface IGameStateProvider
    {
        GameState CurrentGameState { get; }
        MainMenuSubState CurrentMainMenuSubState { get; }

        PauseSubState CurrentPauseSubState { get; }
    }
}