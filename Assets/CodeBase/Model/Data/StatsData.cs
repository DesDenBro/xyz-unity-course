using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class StatsData
    {
        [SerializeField] private List<StatsProgress> _progress;
        
        public int GetLevel(StatId id)
        {
            var progress = _progress.FirstOrDefault(x => x.Id == id);
            return progress?.Level ?? 0;
        }

        public void LevelUp(StatId id)
        {
            var progress = _progress.FirstOrDefault(x => x.Id == id);
            if (progress == null) 
            {
                progress = new StatsProgress(id, 0);
                _progress.Add(progress);
            }
            
            progress.Level++;
        }
    }

    [Serializable]
    public class StatsProgress
    {
        public StatId Id;
        public int Level;

        public StatsProgress(StatId id, int level)
        {
            Id = id;
            Level = level;
        }
    }
}