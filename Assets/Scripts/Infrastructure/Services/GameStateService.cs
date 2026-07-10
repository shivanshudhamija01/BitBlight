using Data.Enums;
using Infrastructure.Services.Interfaces;
using Infrastructure.Signals;
using Zenject;

namespace Infrastructure.Services
{
    public class GameStateService : IGameStateService, IGameStateProvider
    {
        private readonly SignalBus _signalBus;

        //TODO : Set it splash ( after creating splash)
        public GameState CurrentGameState { get; private set; } = GameState.MainMenu;
        public MainMenuSubState CurrentMainMenuSubState { get; private set; } = MainMenuSubState.Root;
        public PauseSubState CurrentPauseSubState { get; private set; } = PauseSubState.Root;

        public GameStateService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void ChangeState(GameState newState)
        {
            CurrentGameState = newState;
            if (CurrentGameState == GameState.MainMenu)
            {
                CurrentMainMenuSubState = MainMenuSubState.Root;
            }

            _signalBus.Fire(new StateChangedSignal(CurrentGameState));

            if (CurrentGameState == GameState.MainMenu)
            {
                _signalBus.Fire(new MainMenuSubStateChangedSignal(CurrentMainMenuSubState));
            }

            if (CurrentGameState == GameState.Paused)
            {
                _signalBus.Fire(new PauseSubStateChangedSignal(CurrentPauseSubState));
            }
        }

        public void ChangeMainMenuSubState(MainMenuSubState newSubState)
        {
            if (CurrentGameState != GameState.MainMenu) return;

            CurrentMainMenuSubState = newSubState;
            _signalBus.Fire(new MainMenuSubStateChangedSignal(CurrentMainMenuSubState));
        }

        public void ChangePauseSubState(PauseSubState newSubState)
        {
            if (CurrentGameState != GameState.Paused) return;

            CurrentPauseSubState = newSubState;
            _signalBus.Fire(new PauseSubStateChangedSignal(CurrentPauseSubState));
        }
    }
}