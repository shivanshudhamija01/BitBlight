using System;
using FairyGUI;
using UI.Views.Abstract;

namespace UI.Views
{
    public class OrderPopUpView : UIViewBase
    {
        private GComponent _orderRoot;
        private GComponent _popupPanel;
        private GTextField _titleText;
        private GTextField _detailsText;
        private GList _componentList;
        private GButton _closeBtn;

        private Transition _showTransition;
        private Transition _hideTransition;

        public override void CreateUI()
        {
            UIPackage.AddPackage("Announcements");
            _orderRoot = UIPackage.CreateObject("Announcements", "OrderRoot").asCom;
            GRoot.inst.AddChild(_orderRoot);

            _orderRoot.SetSize(GRoot.inst.width, GRoot.inst.height);
            _orderRoot.AddRelation(GRoot.inst, RelationType.Size);

            _popupPanel = _orderRoot.GetChild("OrderPanel").asCom;
            _titleText = _popupPanel.GetChild("TitleText").asTextField;
            _detailsText = _popupPanel.GetChild("DetailsText").asTextField;
            _componentList = _popupPanel.GetChild("ComponentList")?.asList;
            _closeBtn = _popupPanel.GetChild("CloseBtn").asButton;

            _showTransition = _orderRoot.GetTransition("Show");
            _hideTransition = _orderRoot.GetTransition("Hide");

            InitialHide();
        }

        private void InitialHide()
        {
            if (_orderRoot != null) _orderRoot.visible = false;
        }

        public void SetText(string title, string details)
        {
            _titleText.text = title;
            _detailsText.text = details;
        }

        public GList GetComponentList() => _componentList;

        public void OnClose(Action action) => _closeBtn.onClick.Add((() => action()));

        public override void Show()
        {
            if (_orderRoot != null)
            {
                _hideTransition.Stop();
                _orderRoot.visible = true;
                _showTransition.Play();
            }
        }

        public override void Hide()
        {
            if (_orderRoot != null && _orderRoot.visible)
            {
                _showTransition.Stop();
                _hideTransition.Play(() => _orderRoot.visible = false);
            }
        }
    }
}