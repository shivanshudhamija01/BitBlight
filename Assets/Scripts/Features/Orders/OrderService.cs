using System.Runtime.InteropServices;
using Data.ScriptableObjects.Recipes;
using Infrastructure.Services.Interfaces;
using Infrastructure.Signals;
using Zenject;

namespace Features.Orders
{
    public class OrderService : IOrderService
    {
        private readonly SignalBus _signalBus;

        private PCAssemblyRecipeData _activeOrder;
        private int _remainingAmount;

        public OrderService(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<PCAssembledSignal>(OnPCAssembled);
        }

        public void StartOrder(PCAssemblyRecipeData order, int amount = 1)
        {
            _activeOrder = order;
            _remainingAmount = amount;

            _signalBus.Fire(new OrderStartedSignal(order.pcId, amount, order.componentInputs));
        }

        private void OnPCAssembled(PCAssembledSignal signal)
        {
            if (_activeOrder == null)
                return;

            if (signal.PcId != _activeOrder.pcId)
                return;

            _remainingAmount--;

            if (_remainingAmount <= 0)
            {
                _signalBus.Fire(new OrderCompletedSignal(_activeOrder.pcId));
                _activeOrder = null;
            }
        }
    }
}