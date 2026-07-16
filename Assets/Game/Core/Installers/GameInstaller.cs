using Core.StateMachine;
using Core.Bootstrap;
using Zenject;
using Map.Generator;
using Map.Services;
using Map.Presentation;
using UI.Controllers;
using UI.WorldUI;
using Construction.Services;
using Economy.Domain;
using Economy.Services;
using Economy.Presentation;
using Construction.Config;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<StateChangedSignal>();

            BindStateMachine();
            BindStates();

            Container.Bind<BuildingFactory>().AsSingle(); // для генерации карты
            BindMap();

            BindEconomy();
            BindBuild();
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

        private void BindBuild()
        {
            Container.Bind<IBuildService>()
                     .To<BuildService>()
                     .AsSingle();
        }

        private void BindUI()
        {
            Container.BindInterfacesAndSelfTo<HexSelectionService>().AsSingle(); //Биндит себя и Init/Dispose

            Container.Bind<HexSelectedPanelView>()
                    .FromComponentInHierarchy()
                    .AsSingle();

            Container.Bind<HexPanelController>().AsSingle().NonLazy();
        }

        private void BindBootstrap()
        {
            Container.BindInterfacesTo<GameBootstrap>().AsSingle().NonLazy();
        }
    }
}