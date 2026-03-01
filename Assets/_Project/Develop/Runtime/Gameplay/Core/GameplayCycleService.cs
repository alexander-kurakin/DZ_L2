using System;
using _Project.Develop.Runtime.Configs.Gameplay;
using _Project.Develop.Runtime.Gameplay.Utilities;
using _Project.Develop.Runtime.Meta.Features.Stats;
using _Project.Develop.Runtime.Meta.Features.Wallet;
using _Project.Develop.Runtime.Utilities.CoroutinesManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Logic
{
    public class GameplayCycleService
    {
        private SymbolGeneratorService _symbolGeneratorService;
        private LevelConfig _currentLevelConfig;
        private ICoroutinesPerformer _coroutinesPerformer;
        private SceneSwitcherService _sceneSwitcherService;
        private IInputSceneArgs _inputArgs;
        private WalletService _walletService;
        private StatsService _statsService;
        private GameRules _gameRules;
        private PlayerDataProvider _playerDataProvider;
        
        private string _targetSequence;
        private bool _shouldCheckInput;
        private string _currentUserInput;
        private int _currentIndex = 0;
        private bool _winConditionReached;
        private bool _loseConditionReached;
        

        public GameplayCycleService(
            SymbolGeneratorService symbolGeneratorService, 
            LevelConfig currentLevelConfig,  
            SceneSwitcherService sceneSwitcherService, 
            ICoroutinesPerformer coroutinesPerformer,
            IInputSceneArgs inputArgs,
            WalletService walletService,
            StatsService statsService,
            GameRules gameRules,
            PlayerDataProvider playerDataProvider)
        {
            _symbolGeneratorService = symbolGeneratorService;
            _currentLevelConfig = currentLevelConfig;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _inputArgs = inputArgs;
            _walletService = walletService;
            _statsService = statsService;
            _gameRules = gameRules;
            _playerDataProvider = playerDataProvider;
        }

        public void Prepare()
        {
            Debug.Log("Preparing target sequence");
            SetTargetSequence();
        }

        public void Launch()
        {
            if (_targetSequence != null)
            {
                Debug.Log("Launching target sequence= " + _targetSequence);
                _shouldCheckInput = true;
            }
            else
            {
                throw new ArgumentException("Target sequence is empty");
            }

        }

        public void SetTargetSequence()
        {
            _targetSequence = _symbolGeneratorService.GenerateRandom(_currentLevelConfig.CheckStringLength);
        }

        public void Update(float deltaTime)
        {
            if (_shouldCheckInput)
            {
                CheckUserInput();
            }
            else
            {
                if (_winConditionReached ||  _loseConditionReached)
                    
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (_winConditionReached)
                            OnGameWon();
                        else
                            OnGameLost();
                        
                        
                    }
            }
        }

        private void CheckUserInput()
        {
            if (Input.anyKeyDown)
            {
                _currentUserInput = Input.inputString;
                
                foreach (char inputSymbol in _currentUserInput)
                {
                    if (char.IsLetterOrDigit(inputSymbol) == false)
                        continue;
                    
                    if (char.ToLower(inputSymbol) == char.ToLower(_targetSequence[_currentIndex]))
                    {
                        _currentIndex++;
                        Debug.Log($"Верно! {_currentIndex}/{_targetSequence.Length}");

                        if (_currentIndex >= _targetSequence.Length)
                        {
                            Debug.Log("Win! Press space to continue");
                            _winConditionReached = true;
                            _shouldCheckInput = false;
                        }
                    }
                    else
                    {
                        Debug.Log("Error! Press space to try again");
                        _loseConditionReached = true; 
                        _shouldCheckInput = false;
                    }
                }
            }
        }

        private void OnGameLost()
        {
            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, _inputArgs));
            
            if (_walletService.Enough(CurrencyTypes.Gold, _gameRules.GoldForLosing))
                _walletService.Spend(CurrencyTypes.Gold, _gameRules.GoldForLosing);
            _statsService.RecordLoss();
            _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
        }

        private void OnGameWon()
        {
            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
            
            _walletService.Add(CurrencyTypes.Gold, _gameRules.GoldForWinning);
            _statsService.RecordWin();
            _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
        }        
    }
}