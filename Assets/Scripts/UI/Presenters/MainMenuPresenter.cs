using System;
using Data.Enums;
using Infrastructure.Signals;
using UI.Views;
using UnityEngine;
using Zenject;

namespace UI.Presenters
{
    public class MainMenuPresenter : IInitializable

    {
        private readonly MainMenuView _mainMenuView;
        private readonly SignalBus _signalBus;

        public MainMenuPresenter(MainMenuView mainMenuView, SignalBus signalBus)
        {
            _mainMenuView = mainMenuView;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _mainMenuView.CreateUI();

            _mainMenuView.OnPlay((() =>
            {
                Debug.Log("Play");
                _signalBus.Fire(new StartGameEvent());
            }));
            _mainMenuView.OnOptions((() =>
            {
                Debug.Log("Options");
                _signalBus.Fire(new GoToMainMenuOptionsEvent());
            }));
            _mainMenuView.OnQuit((() => { Debug.Log("Quit"); }
                ));
        }
    }
}