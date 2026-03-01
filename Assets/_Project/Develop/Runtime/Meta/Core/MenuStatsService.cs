using _Project.Develop.Runtime.Configs.Gameplay;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using UnityEngine;

namespace _Project.Develop.Runtime.Meta.Core
{
    public class MenuStatsService
    {
        private ICoroutinesPerformer _coroutinesPerformer;
        private WalletService _walletService;
        private GameRules _gameRules;
        private PlayerDataProvider _playerDataProvider;
        private StatsService _statsService;

        public MenuStatsService(
            ICoroutinesPerformer coroutinesPerformer, 
            WalletService walletService, 
            GameRules gameRules, 
            PlayerDataProvider playerDataProvider, 
            StatsService statsService)
        {
            _coroutinesPerformer = coroutinesPerformer;
            _walletService = walletService;
            _gameRules = gameRules;
            _playerDataProvider = playerDataProvider;
            _statsService = statsService;
        }

        public void Update(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_walletService.Enough(CurrencyTypes.Gold, _gameRules.GoldToRestartGame))
                {
                    _walletService.Spend(CurrencyTypes.Gold, _gameRules.GoldToRestartGame);
                    _statsService.ResetProgress();
                    _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
                }
                else
                {
                    Debug.Log("Not enough gold to restart!");
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Current gold: " +  _walletService.GetCurrency(CurrencyTypes.Gold).Value);
                Debug.Log("Current wins: " +  _statsService.Wins.Value);
                Debug.Log("Current losses: " +  _statsService.Losses.Value);
            }


        }
    }
}