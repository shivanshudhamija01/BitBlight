using System;
using FairyGUI;
using UI.Views.Abstract;

namespace UI.Views
{
    public class RecipeView : UIViewBase
    {
        private GComponent _recipePanel;
        private GButton _closeButton;
        private GList _recipeList;

        private Transition _showTransition;
        private Transition _hideTransition;

        public override void CreateUI()
        {
            UIPackage.AddPackage("Recipes");
            _recipePanel = UIPackage.CreateObject("Recipes", "RecipePanel").asCom;
            GRoot.inst.AddChild(_recipePanel);
            _recipePanel.SetSize(GRoot.inst.width, GRoot.inst.height);
            _recipePanel.AddRelation(GRoot.inst, RelationType.Size);

            _closeButton = _recipePanel.GetChild("CloseBtn").asButton;
            _recipeList = _recipePanel.GetChild("RecipeList").asList;

            _showTransition = _recipePanel.GetTransition("Show");
            _hideTransition = _recipePanel.GetTransition("Hide");

            InitialHide();
        }

        private void InitialHide()
        {
            if (_recipePanel != null)
            {
                _recipePanel.visible = false;
            }
        }

        public override void Show()
        {
            if (_recipePanel != null)
            {
                _recipePanel.visible = true;
                _showTransition.Play();
            }
        }

        public override void Hide()
        {
            if (_recipePanel != null && _recipePanel.visible)
            {
                _hideTransition.Play(() => _recipePanel.visible = false);
            }
        }


        public void OnClose(Action action)
        {
            _closeButton.onClick.Add(() => action());
        }

        public GList GetRecipeList() => _recipeList;
    }
}