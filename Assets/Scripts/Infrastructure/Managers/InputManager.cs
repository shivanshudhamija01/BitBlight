using System;
using Data.Enums;
using Infrastructure.Services.Interfaces;
using Infrastructure.Signals;
using Zenject;

namespace Infrastructure.Managers
{
    public class InputManager : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IInputService _inputService;

        public InputManager(
            SignalBus signalBus,
            IInputService inputService)
        {
            _signalBus = signalBus;
            _inputService = inputService;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<OrderPopupStateChangedSignal>(OnOrderPopupStateChnaged);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<OrderPopupStateChangedSignal>(OnOrderPopupStateChnaged);
        }


        private void OnOrderPopupStateChnaged(OrderPopupStateChangedSignal signal)
        {
            _inputService.ResetJoystick();
        }
    }
}