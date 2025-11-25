using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class LevelsData
    {
        [SerializeField] private List<LevelData> _levelDatas = new List<LevelData>();

        public IReadOnlyDictionary<string, LevelData> LevelDatasDict => _levelDatas?.Where(x => x != null && !string.IsNullOrWhiteSpace(x.Name)).ToDictionary(x => x.Name, y => y);

        public void SaveHeroPosition(string name, string checkPointName)
        {
            LevelData levelData;

            if (LevelDatasDict == null) _levelDatas = new List<LevelData>();
            if (!LevelDatasDict.TryGetValue(name, out levelData))
            {
                levelData = new LevelData(name, checkPointName);
                _levelDatas.Add(levelData);
            }
        }

        public LevelData Get(string name)
        {
            return LevelDatasDict.TryGetValue(name, out LevelData levelData) ? levelData : null;
        }
    }

    [Serializable]
    public class LevelData
    {
        public string Name;
        public string CheckPointName;

        public LevelData(string name, string checkPointName)
        {
            Name = name;
            CheckPointName = string.IsNullOrWhiteSpace(checkPointName) ? name.ToLower() + "-checkpoint1" : checkPointName; // default
        }
    }
}
