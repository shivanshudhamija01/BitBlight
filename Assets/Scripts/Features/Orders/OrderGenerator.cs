using System;
using Data.ScriptableObjects.Orders;
using Infrastructure.Signals;
using Zenject;
using Random = UnityEngine.Random;

namespace Features.Orders
{
    public class OrderGenerator : IInitializable, IDisposable
    {
        private readonly IOrderService _orderService;
        private readonly SignalBus _signalBus;
        private readonly PCAssemblyOrdersData _orderPool;

        public OrderGenerator(
            IOrderService orderService,
            SignalBus signalBus,
            PCAssemblyOrdersData orderPool)
        {
            _orderService = orderService;
            _signalBus = signalBus;
            _orderPool = orderPool;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<OrderCompletedSignal>(OnOrderCompleted);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<OrderCompletedSignal>(OnOrderCompleted);
        }

        private void OnOrderCompleted() => GenerateRandomOrder();

        private void GenerateRandomOrder()
        {
            var rnd = Random.Range(0, _orderPool.recipes.Length);

            var randomRecipe = _orderPool.recipes[rnd];
            _orderService.StartOrder(randomRecipe, 1);
        }
    }
}