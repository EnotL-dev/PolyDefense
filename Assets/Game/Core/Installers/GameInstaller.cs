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
using Construction.Services;
using Combat;

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

        private void BindBuild()
        {
            Container.Bind<IBuildService>()
                     .To<BuildService>()
                     .AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<HexSelectedPanelView>()
             .FromComponentInHierarchy()
             .AsSingle();

            Container.Bind<HexPanelController>().AsSingle().NonLazy(); //Создаст без запроса
        }

        private void BindCombat()
        {
            Container.Bind<EnemyFactory>().AsSingle();
            Container.Bind<CombatService>().AsSingle();
            Container.Bind<NavigationService>().AsSingle();
            Container.Bind<TargetingService>().AsSingle();

            Container.BindInterfacesTo<CombatDebugService>().AsSingle();

            Container.Bind<EnemyConfig>()
                .FromScriptableObjectResource("Combat/EnemyConfig")
                .AsSingle();
        }

        private void BindBootstrap()
        {
            Container.BindInterfacesTo<GameBootstrap>().AsSingle();
        }
    }
}