using Core.StateMachine;
using Core.Bootstrap;
using Zenject;
using Map.Generator;
using Map.Services;
using Map.Presentation;
using UI.Controllers;

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

            BindMap();
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

        private void BindUI()
        {
            Container.BindInterfacesAndSelfTo<HexSelectionService>().AsSingle(); //Биндит себя и Init/Dispose
        }

        private void BindBootstrap()
        {
            Container.BindInterfacesTo<GameBootstrap>().AsSingle().NonLazy();
        }
    }
}