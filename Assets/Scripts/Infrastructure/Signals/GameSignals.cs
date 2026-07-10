using System;
using Data;
using Data.Enums;

namespace Infrastructure.Signals
{
    //! RESOURCES 
    public readonly struct ResourceChangedSignal
    {
        public readonly Enum ResourceType;
        public readonly int NewAmount;

        public ResourceChangedSignal(Enum resourceType, int newAmount)
        {
            ResourceType = resourceType;
            NewAmount = newAmount;
        }
    }

    //? Task Station
    public readonly struct TaskStationStateChangedSignal
    {
        public readonly bool IsWorking;

        public TaskStationStateChangedSignal(bool isWorking)
        {
            IsWorking = isWorking;
        }
    }

    //! FACTORIES
    public readonly struct FactoryStateChangedSignal
    {
        public readonly string FactoryId;
        public readonly FactoryState State;

        public FactoryStateChangedSignal(string factoryId, FactoryState state)
        {
            FactoryId = factoryId;
            State = state;
        }
    }

    public readonly struct ProcessedMaterialProducedSignal
    {
        public readonly ProcessedMaterialType ProcessedMaterialType;
        public readonly int Amount;

        public ProcessedMaterialProducedSignal(ProcessedMaterialType type, int amount)
        {
            ProcessedMaterialType = type;
            Amount = amount;
        }
    }

    public readonly struct ComponentProducedSignal
    {
        public readonly ComponentType ComponentType;
        public readonly int Amount;

        public ComponentProducedSignal(ComponentType componentType, int amount)
        {
            ComponentType = componentType;
            Amount = amount;
        }
    }

    public readonly struct PCAssembledSignal
    {
        public readonly string PcId;

        public PCAssembledSignal(string pcId)
        {
            PcId = pcId;
        }
    }

    //! ORDERS
    public readonly struct OrderStartedSignal
    {
        public readonly string OrderId;
        public readonly int Amount;
        public readonly ComponentInput[] RequiredComponents;

        public OrderStartedSignal(string orderId,
            int amount,
            ComponentInput[] requiredComponets)
        {
            OrderId = orderId;
            Amount = amount;
            RequiredComponents = requiredComponets;
        }
    }

    public readonly struct OrderCompletedSignal
    {
        public readonly string PcId;

        public OrderCompletedSignal(string orderId)
        {
            PcId = orderId;
        }
    }

    public readonly struct OrderPopupStateChangedSignal
    {
        public readonly bool IsShowing;

        public OrderPopupStateChangedSignal(bool isShowing)
        {
            IsShowing = isShowing;
        }
    }

    //! STATE
    public readonly struct StateChangedSignal
    {
        public readonly GameState NewState;

        public StateChangedSignal(GameState newState)
        {
            NewState = newState;
        }
    }

    public readonly struct MainMenuSubStateChangedSignal
    {
        public readonly MainMenuSubState NewSubState;

        public MainMenuSubStateChangedSignal(MainMenuSubState newSubState)
        {
            NewSubState = newSubState;
        }
    }

    public readonly struct PauseSubStateChangedSignal
    {
        public readonly PauseSubState NewSubState;

        public PauseSubStateChangedSignal(PauseSubState newSubState)
        {
            NewSubState = newSubState;
        }
    }

    //! UI

    public readonly struct ButtonClick { }

    public readonly struct SplashFinished { }

    public readonly struct StartGameEvent { }

    public readonly struct GoToMainMenuOptionsEvent { }

    public readonly struct GoToRecipesPanelEvent { }

    public readonly struct PauseGameEvent { }

    public readonly struct GotoPauseOptionsEvent { }

    public readonly struct ResumeGameEvent { }

    public readonly struct GoToMainMenuEvent { }

    public readonly struct CloseMenuEvent { }

    public readonly struct ShowCurrentOrderEvent { }

    //! Audio
    public struct SetSfxVolumeEvent
    {
        public readonly float Volume;
        public SetSfxVolumeEvent(float volume) => Volume = volume;
    }

    public struct SetMusicVolumeEvent
    {
        public readonly float Volume;
        public SetMusicVolumeEvent(float volume) => Volume = volume;
    }

    public struct SetMusicMuteEvent
    {
        public readonly bool IsMusicMuted;
        public SetMusicMuteEvent(bool isMusicMuted) => IsMusicMuted = isMusicMuted;
    }

    public struct SetSfxMuteEvent
    {
        public readonly bool IsSfxMuted;
        public SetSfxMuteEvent(bool isSfxMuted) => IsSfxMuted = isSfxMuted;
    }
}