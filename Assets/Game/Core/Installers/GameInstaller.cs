using Core.StateMachine;
using Core.Bootstrap;
using Zenject;
using Map.Generator;
using Map.Services;
using Map.Presentation;
using UI.WorldUI;
using UI.Controllers;
using Economy.Services;
using Economy.Domain;
using Economy.Presentation;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStateMachine();
            BindStates();
            BindMap();
            BindEconomy();
            BindUI();
            BindBootstrap();
        }

        private void BindStateMachine()
        {
            Container.Bind<IGameStateMachine>()
                     .To<GameStateMachine>()
                     .AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<DayState>().AsSingle();
            Container.Bind<NightState>().AsSingle();
        }

        private void BindMap()
        {
            Container.Bind<MapView>()
             .FromComponentInHierarchy()
             .AsSingle();

            Container.Bind<IHexSelectionService>()
                     .To<HexSelectionService>()
                     .AsSingle();

            Container.Bind<IMapGenerator>()
                     .To<MapGenerator>()
                     .AsSingle();

            Container.Bind<IMapService>()
                     .To<MapService>()
                     .AsSingle();
        }

        private void BindEconomy()
        {
            Container.Bind<ResourceBase>().AsSingle().NonLazy(); //Создаст без запроса

            Container.Bind<IEconomyService>()
                     .To<EconomyService>()
                     .AsSingle();

            Container.Bind<EconomyView>()
             .FromComponentInHierarchy()
             .AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<HexSelectedPanelView>()
             .FromComponentInHierarchy()
             .AsSingle();

            Container.Bind<HexPanelController>().AsSingle().NonLazy(); //Создаст без запроса
        }

        private void BindBootstrap()
        {
            Container.BindInterfacesTo<GameBootstrap>().AsSingle();
        }
    }
}