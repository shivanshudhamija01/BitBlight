using System;
using Data.Enums;
using Infrastructure.Services.Interfaces;
using Infrastructure.Signals;
using Zenject;

namespace Infrastructure.Services
{
    public class GameFlowService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IGameStateService _gameStateService;
        private readonly IGameStateProvider _gameStateProvider;
        private readonly IPauseService _pauseService;


        public GameFlowService(
            SignalBus signalBus,
            IGameStateService gameStateService,
            IGameStateProvider gameStateProvider,
            IPauseService pauseService
        )
        {
            _signalBus = signalBus;
            _gameStateService = gameStateService;
            _gameStateProvider = gameStateProvider;
            _pauseService = pauseService;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<SplashFinished>(LoadMainMenu);
            _signalBus.Subscribe<GoToMainMenuOptionsEvent>(LoadMainMenuOptions);
            _signalBus.Subscribe<StartGameEvent>(StartGame);

            _signalBus.Subscribe<GoToRecipesPanelEvent>(LoadRecipesPanel);
            _signalBus.Subscribe<PauseGameEvent>(PauseGame);
            _signalBus.Subscribe<GotoPauseOptionsEvent>(LoadPauseMenuOptions);
            _signalBus.Subscribe<ResumeGameEvent>(ResumeGame);
            _signalBus.Subscribe<GoToMainMenuEvent>(LoadMainMenu);
            _signalBus.Subscribe<CloseMenuEvent>(CloseMenu);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<SplashFinished>(LoadMainMenu);
            _signalBus.Unsubscribe<GoToMainMenuOptionsEvent>(LoadMainMenuOptions);
            _signalBus.Unsubscribe<StartGameEvent>(StartGame);

            _signalBus.Unsubscribe<GoToRecipesPanelEvent>(LoadRecipesPanel);
            _signalBus.Unsubscribe<PauseGameEvent>(PauseGame);
            _signalBus.Unsubscribe<GotoPauseOptionsEvent>(LoadPauseMenuOptions);
            _signalBus.Unsubscribe<ResumeGameEvent>(ResumeGame);
            _signalBus.Unsubscribe<GoToMainMenuEvent>(LoadMainMenu);
            _signalBus.Unsubscribe<CloseMenuEvent>(CloseMenu);
        }

        private void LoadMainMenu()
        {
            _gameStateService.ChangeState(GameState.MainMenu);
        }

        private void LoadMainMenuRoot()
        {
            _gameStateService.ChangeMainMenuSubState(MainMenuSubState.Root);
        }

        private void LoadMainMenuOptions()
        {
            _gameStateService.ChangeMainMenuSubState(MainMenuSubState.Options);
        }

        private void StartGame()
        {
            _pauseService.Reset();
            _gameStateService.ChangeState(GameState.Gameplay);
        }

        private void PauseGame()
        {
            _pauseService.Pause();
        }

        private void ResumeGame()
        {
            _pauseService.Resume();
        }

        private void LoadPauseMenuRoot()
        {
            _gameStateService.ChangePauseSubState(PauseSubState.Root);
        }

        private void LoadPauseMenuOptions()
        {
            _gameStateService.ChangePauseSubState(PauseSubState.Options);
        }

        private void LoadRecipesPanel()
        {
            _gameStateService.ChangeState(GameState.Recipes);
        }

        private void CloseMenu()
        {
            if (_gameStateProvider.CurrentGameState == GameState.MainMenu)
            {
                if (_gameStateProvider.CurrentMainMenuSubState == MainMenuSubState.Options)
                {
                    LoadMainMenuRoot();
                }
            }
            else if (_gameStateProvider.CurrentGameState == GameState.Paused)
            {
                if (_gameStateProvider.CurrentPauseSubState == PauseSubState.Options)
                {
                    LoadPauseMenuRoot();
                }
            }
            else if (_gameStateProvider.CurrentGameState == GameState.Recipes)
            {
                _gameStateService.ChangeState(GameState.Gameplay);
            }
        }
    }
}