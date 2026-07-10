using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Data.Enums;
using Data.ScriptableObjects.Components;
using FairyGUI;
using Infrastructure.Services.Interfaces;
using Infrastructure.Signals;
using UI.Views;
using UnityEngine;
using Zenject;

namespace UI.Presenters
{
    public class OrderPopupPresenter : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly OrderPopUpView _orderPopupView;
        private readonly IGameStateProvider _gameStateProvider;
        private readonly ComponentData[] _components;

        private bool _isShowing;
        private CancellationTokenSource _cts;

        private GList _componentList;
        private List<ComponentInput> _cachedComponents = new();

        private string _currentOrderTitle;
        private string _currentOrderDetails;

        public OrderPopupPresenter(
            SignalBus signalBus,
            OrderPopUpView orderPopupView,
            IGameStateProvider gameStateProvider,
            ComponentData[] components)
        {
            _signalBus = signalBus;
            _orderPopupView = orderPopupView;
            _gameStateProvider = gameStateProvider;
            _components = components;
        }

        public void Initialize()
        {
            _cts = new CancellationTokenSource();
            _orderPopupView.CreateUI();

            _orderPopupView.OnClose((() =>
            {
                _orderPopupView.Hide();
                _isShowing = false;

                _signalBus.Fire(new OrderPopupStateChangedSignal(false));
            }));

            _componentList = _orderPopupView.GetComponentList();
            if (_componentList != null)
            {
                _componentList.defaultItem = "ui://Recipes/IngredientItem";
                _componentList.itemRenderer = RenderComponentItem;
                _componentList.SetVirtual();
            }

            _signalBus.Subscribe<StateChangedSignal>(OnStateChanged);
            _signalBus.Subscribe<OrderStartedSignal>(OnOrderStarted);
            _signalBus.Subscribe<OrderCompletedSignal>(OnOrderCompleted);
            _signalBus.Subscribe<ShowCurrentOrderEvent>(OnShowCurrentOrder);
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _signalBus.Unsubscribe<StateChangedSignal>(OnStateChanged);
            _signalBus.Unsubscribe<OrderStartedSignal>(OnOrderStarted);
            _signalBus.Unsubscribe<OrderCompletedSignal>(OnOrderCompleted);
            _signalBus.Unsubscribe<ShowCurrentOrderEvent>(OnShowCurrentOrder);
        }

        private void RenderComponentItem(int index, GObject obj)
        {
            GComponent item = obj.asCom;
            var data = _cachedComponents[index];

            var countText = item.GetChild("Count");
            if (countText != null)
            {
                countText.text = data.amount.ToString();
            }

            GLoader iconLoader = item.GetChild("Icon")?.asLoader;
            if (iconLoader != null)
            {
                Sprite sprite = GetSprite(data.type.ToString());
                iconLoader.SetSize(60, 60);
                iconLoader.fill = FillType.ScaleFree;
                iconLoader.texture = sprite != null ? new NTexture(sprite) : null;
            }
        }

        private Sprite GetSprite(string enumValue)
        {
            var component = _components.FirstOrDefault(m => m.id == enumValue);
            return component != null ? component.icon : null;
        }

        private void OnStateChanged(StateChangedSignal signal)
        {
            if (_isShowing)
            {
                if (signal.NewState == GameState.Gameplay)
                {
                    _orderPopupView.Show();
                }
                else
                {
                    _orderPopupView.Hide();
                }
            }
        }

        private void OnOrderStarted(OrderStartedSignal signal)
        {
            _cachedComponents.Clear();
            if (signal.RequiredComponents != null)
            {
                _cachedComponents.AddRange(signal.RequiredComponents);
            }

            if (_componentList != null)
            {
                _componentList.numItems = _cachedComponents.Count;
                _componentList.RefreshVirtualList();
            }

            _currentOrderTitle = "New Order Received!";
            _currentOrderDetails = $"Assemble: {signal.OrderId} x{signal.Amount}";

            ShowPopup("New Order Received!", $"Assemble: {signal.OrderId} x{signal.Amount}");
        }

        private void OnOrderCompleted(OrderCompletedSignal signal)
        {
            _cachedComponents.Clear();
            if (_componentList != null)
            {
                _componentList.numItems = 0;
                _componentList.RefreshVirtualList();
            }

            ShowPopup("Order Completed!", $"Successfully shipped: {signal.PcId}");
        }

        private void OnShowCurrentOrder(ShowCurrentOrderEvent signal)
        {
            if (_isShowing) return;


            if (string.IsNullOrEmpty(_currentOrderDetails)) return;

            ShowPopupImmediate(_currentOrderTitle, _currentOrderDetails);
        }

        private async void ShowPopup(string title, string details)
        {
            if (_isShowing) return;
            try
            {
                await DelayWhilePlaying(2000, _cts.Token);

                _isShowing = true;
                _signalBus.Fire(new OrderPopupStateChangedSignal(true));

                _orderPopupView.SetText(title, details);
                if (_gameStateProvider.CurrentGameState == GameState.Gameplay)
                {
                    _orderPopupView.Show();
                }

                await DelayWhilePlaying(3000, _cts.Token);
            }
            catch (OperationCanceledException) { }
        }

        private void ShowPopupImmediate(string title, string details)
        {
            if (_isShowing) return;

            _isShowing = true;
            _signalBus.Fire(new OrderPopupStateChangedSignal(true));

            _orderPopupView.SetText(title, details);
            _orderPopupView.Show();
        }

        private async Task DelayWhilePlaying(int milliseconds, CancellationToken token)
        {
            int elapsed = 0;
            int step = 100;

            while (elapsed < milliseconds)
            {
                token.ThrowIfCancellationRequested();

                await Task.Delay(step, token);

                if (_gameStateProvider.CurrentGameState == GameState.Gameplay)
                {
                    elapsed += step;
                }
            }
        }
    }
}