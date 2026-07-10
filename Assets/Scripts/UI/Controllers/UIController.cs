using System;
using Data.Enums;
using Infrastructure.Signals;
using UI.Views;
using Zenject;

namespace UI.Controllers
{
    public class UIController : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly MainMenuView _mainMenuView;
        private readonly HUDView _hudView;
        private readonly OptionsView _optionsView;
        private readonly PauseView _pauseView;
        private readonly RecipeView _recipeView;
        private readonly Joystick _joystick;

        private GameState _currentGameState = GameState.MainMenu;
        private bool _isOrderPopupShowing = false;

        public UIController(SignalBus signalBus,
            MainMenuView mainMenuView,
            HUDView hudView,
            OptionsView optionsView,
            PauseView pauseView,
            RecipeView recipeView,
            Joystick joystick)
        {
            _signalBus = signalBus;
            _mainMenuView = mainMenuView;
            _hudView = hudView;
            _optionsView = optionsView;
            _pauseView = pauseView;
            _recipeView = recipeView;
            _joystick = joystick;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<StateChangedSignal>(OnStateChanged);
            _signalBus.Subscribe<MainMenuSubStateChangedSignal>(OnMainMenuSubStateChanged);
            _signalBus.Subscribe<PauseSubStateChangedSignal>(OnPauseSubStateChanged);

            _signalBus.Subscribe<OrderPopupStateChangedSignal>(OnOrderPopupStateChanged);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<StateChangedSignal>(OnStateChanged);
            _signalBus.Unsubscribe<MainMenuSubStateChangedSignal>(OnMainMenuSubStateChanged);
            _signalBus.Unsubscribe<PauseSubStateChangedSignal>(OnPauseSubStateChanged);

            _signalBus.Unsubscribe<OrderPopupStateChangedSignal>(OnOrderPopupStateChanged);
        }

        private void OnStateChanged(StateChangedSignal signal)
        {
            _currentGameState = signal.NewState;
            UpdateState(signal.NewState);
        }

        private void OnMainMenuSubStateChanged(MainMenuSubStateChangedSignal signal)
        {
            UpdateMainMenuSubState(signal.NewSubState);
        }

        private void OnPauseSubStateChanged(PauseSubStateChangedSignal signal)
        {
            UpdatePauseSubState(signal.NewSubState);
        }

        private void OnOrderPopupStateChanged(OrderPopupStateChangedSignal signal)
        {
            _isOrderPopupShowing = signal.IsShowing;
            UpdateJoystickVisibility();
        }


        private void UpdateState(GameState newState)
        {
            HideAll();

            switch (newState)
            {
                case GameState.Splash:
                    // _splashView.Show();
                    break;
                case GameState.MainMenu:
                    _mainMenuView.Show();
                    break;
                case GameState.Gameplay:
                    _hudView.Show();
                    _joystick.gameObject.SetActive(true);
                    break;
                case GameState.Paused:
                    _pauseView.Show();
                    break;
                case GameState.Recipes:
                    _recipeView.Show();
                    break;
            }
        }

        private void UpdateMainMenuSubState(MainMenuSubState subState)
        {
            switch (subState)
            {
                case MainMenuSubState.Root:
                    _optionsView.Hide();
                    _mainMenuView.Show();
                    break;
                case MainMenuSubState.Options:
                    _optionsView.SetBackgroundMode(true);
                    _optionsView.Show();
                    break;
            }
        }

        private void UpdatePauseSubState(PauseSubState subState)
        {
            switch (subState)
            {
                case PauseSubState.Root:
                    _optionsView.Hide();
                    _pauseView.Show();
                    break;
                case PauseSubState.Options:
                    _pauseView.Hide();
                    _optionsView.SetBackgroundMode(false);
                    _optionsView.Show();
                    break;
            }
        }

        private void HideAll()
        {
            // _splashView.Hide();
            _mainMenuView.Hide();
            _optionsView.Hide();
            _hudView.Hide();
            _pauseView.Hide();
            _recipeView.Hide();
            _joystick.gameObject.SetActive(false);
        }

        private void UpdateJoystickVisibility()
        {
            bool shouldShow = (_currentGameState == GameState.Gameplay) && !_isOrderPopupShowing;
            _joystick.gameObject.SetActive(shouldShow);
        }
    }
}