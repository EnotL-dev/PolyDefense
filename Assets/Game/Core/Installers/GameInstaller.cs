using Core.StateMachine;
using Core.Bootstrap;
using Zenject;
using Map.Generator;
using Map.Services;
using Map.Presentation;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStateMachine();
            BindStates();
            BindMap();
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

        private void BindBootstrap()
        {
            Container.BindInterfacesTo<GameBootstrap>().AsSingle();
        }
    }
}