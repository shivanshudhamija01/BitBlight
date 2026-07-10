using Data;
using Data.ScriptableObjects;
using Data.ScriptableObjects.Components;
using Data.ScriptableObjects.Factories;
using Data.ScriptableObjects.Orders;
using Data.ScriptableObjects.Resources;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Bitblight/GameSettings")]
    public class GameDataInstaller : ScriptableObjectInstaller<GameDataInstaller>
    {
        [Header("Player Settings")]
        public PlayerSettings playerSettings;

        [Header("Audio Settings")]
        public AudioConfigSO audioConfig;

        [Header("Order Settings")]
        public PCAssemblyOrdersData orderPool;

        [Header("Data References")]
        [SerializeField]
        private RawMaterialData[] rawMaterials;

        [SerializeField]
        private ProcessedMaterialData[] processedMaterials;

        [SerializeField]
        private ComponentData[] components;

        [SerializeField]
        private ProcessingFactoryData[] processingFactories;

        [SerializeField]
        private ComponentFactoryData[] componentFactories;

        public override void InstallBindings()
        {
            Container.BindInstance(playerSettings).IfNotBound();
            Container.BindInstance(audioConfig).AsSingle();
            Container.BindInstance(orderPool).AsSingle();
            Container.BindInstance(rawMaterials).AsSingle();
            Container.BindInstance(processedMaterials).AsSingle();
            Container.BindInstance(components).AsSingle();
            Container.BindInstance(processingFactories).AsSingle();
            Container.BindInstance(componentFactories).AsSingle();
        }
    }
}