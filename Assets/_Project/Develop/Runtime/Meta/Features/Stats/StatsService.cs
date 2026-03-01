using _Project.Develop.Runtime.Utilities.DataManagement;
using _Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using _Project.Develop.Runtime.Utilities.Reactive;

namespace _Project.Develop.Runtime.Meta.Features.Stats
{
    public class StatsService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private ReactiveVariable<int> _wins;
        private ReactiveVariable<int> _losses;
        
        public StatsService(ReactiveVariable<int> wins, ReactiveVariable<int> losses, PlayerDataProvider playerDataProvider)
        {
            _wins = wins;
            _losses = losses;
            
            playerDataProvider.RegisterWriter(this);
            playerDataProvider.RegisterReader(this);
        }
        
        public IReadOnlyVariable<int> Wins => _wins;
        public IReadOnlyVariable<int> Losses => _losses;

        public void RecordLoss()
        {
            _losses.Value++;
        }

        public void RecordWin()
        {
            _wins.Value++;
        }

        public void ResetProgress()
        {
            _wins.Value = 0;
            _losses.Value = 0;
        }

        public void ReadFrom(PlayerData data)
        {
            _wins.Value =  data.Wins;
            _losses.Value = data.Losses;
        }

        public void WriteTo(PlayerData data)
        {
            data.Wins = _wins.Value;
            data.Losses = _losses.Value;
        }
    }
}