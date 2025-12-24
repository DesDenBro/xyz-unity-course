using System.Linq;
using PixelCrew.Model.Definitions.Repositories;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/Repository/Stats", fileName = "StatsRepository")]
    public class StatsRepository : DefRepository<StatDef>
    {
        public StatDef GetStat(StatId id) => _collection.FirstOrDefault(x => x.StatId == id);
    }
}