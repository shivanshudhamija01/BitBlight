using Infrastructure.Signals;
using UI.Views;
using Zenject;

namespace UI.Presenters
{
    public class PausePresenter : IInitializable
    {
        private SignalBus _signalBus;
        private PauseView _pauseView;

        public PausePresenter(
            SignalBus signalBus,
            PauseView pauseView)
        {
            _signalBus = signalBus;
            _pauseView = pauseView;
        }

        public void Initialize()
        {
            _pauseView.CreateUI();

            _pauseView.OnResume((() => { _signalBus.Fire(new ResumeGameEvent()); }));
            _pauseView.OnOptionsMenu((() => { _signalBus.Fire(new GotoPauseOptionsEvent()); }));
            _pauseView.OnMainMenu((() => { _signalBus.Fire(new GoToMainMenuEvent()); }));
        }
    }
}