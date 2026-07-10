using UI.Controllers;
using UI.Presenters;
using UI.Views;
using Zenject;

namespace UI.Installers
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //! Views
            Container.Bind<HUDView>().AsSingle();

            Container.Bind<MainMenuView>().AsSingle();

            Container.Bind<OptionsView>().AsSingle();

            Container.Bind<PauseView>().AsSingle();

            Container.Bind<RecipeView>().AsSingle();

            Container.Bind<OrderPopUpView>().AsSingle();

            //! Presenter

            Container.BindInterfacesAndSelfTo<HUDPresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<MainMenuPresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<OptionsPresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<PausePresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<RecipePresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<OrderPopupPresenter>().AsSingle();

            //! Controller
            Container.BindInterfacesAndSelfTo<UIController>().AsSingle();
        }
    }
}