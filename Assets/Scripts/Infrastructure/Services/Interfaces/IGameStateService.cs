using Data.Enums;

namespace Infrastructure.Services.Interfaces
{
    public interface IGameStateService
    {
        void ChangeState(GameState newState);
        void ChangeMainMenuSubState(MainMenuSubState newSubState);
        public void ChangePauseSubState(PauseSubState newSubState);
    }
}