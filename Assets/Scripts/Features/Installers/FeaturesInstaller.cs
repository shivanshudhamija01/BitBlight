using Data.ScriptableObjects.Orders;
using Features.Orders;
using Features.Resources;
using UnityEngine;
using Zenject;

namespace Features.Core
{
    public class FeaturesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ResourceInventory>().AsSingle();

            Container.BindInterfacesAndSelfTo<ResourceSaveService>().AsSingle();

            Container.BindInterfacesAndSelfTo<ApplicationPauseSaveBridge>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("ApplicationPauseSaveBridge")
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<OrderService>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<OrderGenerator>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameFlowController>().AsSingle().NonLazy();
        }
    }
}