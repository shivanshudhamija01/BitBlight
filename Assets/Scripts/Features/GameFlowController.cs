using System;
using Data.Enums;
using Data.ScriptableObjects.Orders;
using Features.Orders;
using Infrastructure.Signals;
using Zenject;

namespace Features
{
    public class GameFlowController : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IOrderService _orderService;
        private readonly PCAssemblyOrdersData _levelOrders;

        public GameFlowController(
            SignalBus signalBus,
            IOrderService orderService,
            PCAssemblyOrdersData levelOrders)
        {
            _signalBus = signalBus;
            _orderService = orderService;
            _levelOrders = levelOrders;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<StateChangedSignal>(OnStateChanged);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<StateChangedSignal>(OnStateChanged);
        }

        private void OnStateChanged(StateChangedSignal signal)
        {
            if (signal.NewState == GameState.Gameplay)
            {
                if (_levelOrders.recipes.Length > 0)
                {
                    _orderService.StartOrder(_levelOrders.recipes[0], 1);
                }

                _signalBus.Unsubscribe<StateChangedSignal>(OnStateChanged);
            }
        }
    }
}