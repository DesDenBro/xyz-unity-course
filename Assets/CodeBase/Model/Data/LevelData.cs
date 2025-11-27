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
            if (LevelDatasDict == null) _levelDatas = new List<LevelData>();

            LevelData levelData;
            if (!LevelDatasDict.TryGetValue(name, out levelData))
            {
                levelData = new LevelData(name, checkPointName, new List<string>());
                _levelDatas.Add(levelData);
            }
            else
            {
                levelData.CheckPointName = checkPointName;
                var copyList = new string[levelData.DestroyedObjectsIds.Count];
                levelData.DestroyedObjectsIds.CopyTo(copyList);
                levelData.CheckpointDestroyedObjIds = copyList.ToList();
            }
        }

        public void AddObjectToDelete(string name, string objectId)
        {
            LevelData levelData;
            if (!LevelDatasDict.TryGetValue(name, out levelData))
            {
                levelData = new LevelData(name, LevelData.GetDefaultCheckpointName(name), new List<string>());    
            }
            else
            {
                if (levelData.DestroyedObjectsIds.Contains(objectId)) return;
            }

            levelData.DestroyedObjectsIds.Add(objectId);
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
        public List<string> CheckpointDestroyedObjIds;
        public List<string> DestroyedObjectsIds;

        public LevelData(string name, string checkPointName, List<string> destroyedObjectsIds)
        {
            Name = name;
            CheckPointName = string.IsNullOrWhiteSpace(checkPointName) ? GetDefaultCheckpointName(name) : checkPointName; // default
            DestroyedObjectsIds = destroyedObjectsIds;
        }

        public static string GetDefaultCheckpointName(string levelName)
        {
            if (string.IsNullOrWhiteSpace(levelName)) return "LEVELNOTFOUND";

            return levelName.ToLower() + "-checkpoint1";
        }
    }
}
