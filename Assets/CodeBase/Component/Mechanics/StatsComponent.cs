using UnityEngine;
using PixelCrew.Model.Data;

namespace PixelCrew.Components
{
    public class StatsComponent : MonoBehaviour 
    {
        [SerializeField] private StatsData _statsData;

        public StatsData StatsData => _statsData;

        public void SetStats(StatsData sd)
        {
            _statsData = sd;
        }
    }   
}