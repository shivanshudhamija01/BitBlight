using Infrastructure.Signals;
using Zenject;

namespace Infrastructure.Installers
{
    public class SignalInstaller : Installer<SignalInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            //! Gameplay 
            Container.DeclareSignal<ResourceChangedSignal>();
            Container.DeclareSignal<TaskStationStateChangedSignal>();
            Container.DeclareSignal<FactoryStateChangedSignal>();
            Container.DeclareSignal<ProcessedMaterialProducedSignal>();
            Container.DeclareSignal<ComponentProducedSignal>();
            Container.DeclareSignal<PCAssembledSignal>();
            Container.DeclareSignal<OrderStartedSignal>();
            Container.DeclareSignal<OrderCompletedSignal>();

            //! State
            Container.DeclareSignal<StateChangedSignal>();
            Container.DeclareSignal<MainMenuSubStateChangedSignal>();
            Container.DeclareSignal<PauseSubStateChangedSignal>();

            //! UI
            Container.DeclareSignal<ButtonClick>();
            Container.DeclareSignal<SplashFinished>();
            Container.DeclareSignal<StartGameEvent>();
            Container.DeclareSignal<GoToMainMenuOptionsEvent>();
            Container.DeclareSignal<GoToRecipesPanelEvent>();
            Container.DeclareSignal<PauseGameEvent>();
            Container.DeclareSignal<GotoPauseOptionsEvent>();
            Container.DeclareSignal<ResumeGameEvent>();
            Container.DeclareSignal<GoToMainMenuEvent>();
            Container.DeclareSignal<CloseMenuEvent>();
            Container.DeclareSignal<OrderPopupStateChangedSignal>();
            Container.DeclareSignal<ShowCurrentOrderEvent>();

            //! Audio
            Container.DeclareSignal<SetSfxVolumeEvent>();
            Container.DeclareSignal<SetMusicVolumeEvent>();
            Container.DeclareSignal<SetMusicMuteEvent>();
            Container.DeclareSignal<SetSfxMuteEvent>();
        }
    }
}