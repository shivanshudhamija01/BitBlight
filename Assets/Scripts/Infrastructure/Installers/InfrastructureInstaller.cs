using Data;
using Data.Enums;
using Infrastructure.Managers;
using Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        [Header("Joystick")]
        [SerializeField]
        Joystick joystick;

        [Header("Audio")]
        [SerializeField]
        private AudioSource musicSource;

        [SerializeField]
        private AudioSource sfxSource;

        public override void InstallBindings()
        {
            //! Signal Bus
            SignalInstaller.Install(Container);

            //! Input
            Container.BindInstance(joystick).AsSingle();
            Container.BindInterfacesTo<JoystickInputService>().AsSingle();

            //! Audio
            Container.BindInstance(musicSource).WithId(ServiceKeys.Music);
            Container.BindInstance(sfxSource).WithId(ServiceKeys.Sfx);
            Container.Bind<AudioSettingsModel>().AsSingle();

            //! Services
            Container.BindInterfacesTo<GameStateService>().AsSingle();
            Container.BindInterfacesTo<PauseService>().AsSingle();
            Container.BindInterfacesTo<GameFlowService>().AsSingle();
            Container.BindInterfacesTo<AudioService>().AsSingle();

            //! Managers
            Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
        }
    }
}