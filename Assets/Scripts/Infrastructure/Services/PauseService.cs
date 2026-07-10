using System.Collections.Generic;
using Data.Enums;
using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services
{
    public class PauseService : IPauseService, IPauseRegistry
    {
        private readonly IGameStateService _gameStateService;
        private readonly IGameStateProvider _gameStateProvider;

        public PauseService(
            IGameStateService gameStateService,
            IGameStateProvider gameStateProvider)
        {
            _gameStateService = gameStateService;
            _gameStateProvider = gameStateProvider;
        }

        List<IPausable> _pausables = new();

        public void Register(IPausable pausable)
        {
            _pausables.Add(pausable);
        }

        public void Unregister(IPausable pausable)
        {
            _pausables.Remove(pausable);
        }

        public void Pause()
        {
            if (_gameStateProvider.CurrentGameState != GameState.Gameplay) return;

            Notify(true);
            _gameStateService.ChangeState(GameState.Paused);
        }

        public void Resume()
        {
            if (_gameStateProvider.CurrentGameState != GameState.Paused) return;

            Notify(false);
            _gameStateService.ChangeState(GameState.Gameplay);
        }

        public void Reset()
        {
            Notify(false);
        }

        public void SetPausedState(bool isPaused)
        {
            Notify(isPaused);
        }

        private void Notify(bool paused)
        {
            foreach (var p in _pausables)
                p.SetPaused(paused);
        }
    }
}