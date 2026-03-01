using UnityEngine;

namespace _Project.Develop.Runtime.Configs.Gameplay
{
    [CreateAssetMenu(fileName = "GameRules", menuName = "Configs/Gameplay/GameRules")]
    public class GameRules : ScriptableObject
    {
        [field: SerializeField] public int GoldToRestartGame { get; private set; }
        [field: SerializeField] public int GoldForLosing { get; private set; }
        [field: SerializeField] public int GoldForWinning { get; private set; }
    }
}