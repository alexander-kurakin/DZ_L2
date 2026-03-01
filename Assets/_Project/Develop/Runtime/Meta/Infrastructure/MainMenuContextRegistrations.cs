using _Project.Develop.Runtime.Configs.Gameplay;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Meta.Core;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.ConfigsManagement;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            Debug.Log("Процесс регистрации сервисов на сцене меню");
            container.RegisterAsSingle(CreateGameModeChooseService);
            container.RegisterAsSingle(CreateMenuStatsService);
        }
        
        private static GameModeChooseService CreateGameModeChooseService(DIContainer c)
            => new GameModeChooseService(
                c.Resolve<ICoroutinesPerformer>(),
                c.Resolve<SceneSwitcherService>());

        private static MenuStatsService CreateMenuStatsService(DIContainer c)
            => new MenuStatsService(
                c.Resolve<ICoroutinesPerformer>(),
                c.Resolve<WalletService>(),
                c.Resolve<ConfigsProviderService>().GetConfig<GameRules>(),
                c.Resolve<PlayerDataProvider>(),
                c.Resolve<StatsService>()
            );
    }
}
