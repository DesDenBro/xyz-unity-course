using PixelCrew.Components;
using PixelCrew.GameObjects.Creatures;
using PixelCrew.Model.Data;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public static class GameSessionSearch
    {
        public delegate GameSession[] findMethod();

        public static GameSession Get(findMethod m) { return m().FirstOrDefault(x => !x.Disposed); }
    }

    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private LevelsData _levelsData;

        private bool _disposed = false;

        public PlayerData PlayerData => _playerData;
        public LevelsData LevelsData => _levelsData;
        public bool Disposed => _disposed;

        public LevelData ActiveLevelData => _levelsData == null ? null : _levelsData.Get(SceneManager.GetActiveScene().name.ToLower());
        public QuickInventoryModel QuickInventory {  get; private set; }

        private void Awake()
        {
            LoadHud();

            if (IsSessionExist())
            {
                Destroy(gameObject);
                _disposed = true;

                var existSession = GameSessionSearch.Get(FindObjectsOfType<GameSession>);
                if (existSession != null) existSession.DefaultFuncs();

                return;
            }
            else
            {
                DefaultFuncs();
                DontDestroyOnLoad(this);
            }

            ReloadLinks();
        }

        private void Spawn()
        {
            GetComponent<SpawnComponent>().Spawn();
        }
        private void DeleteObjectsById()
        {
            var objIds = ActiveLevelData.CheckpointDestroyedObjIds;
            if (objIds == null || objIds.Count == 0) return;

            var allGOWithUID = FindObjectsOfType<ObjectId>();
            if (allGOWithUID == null || allGOWithUID.Length == 0) return;

            var dict = allGOWithUID.ToDictionary(x => x.UniqueId, y => y);
            foreach (var uid in objIds)
            {
                if (dict.ContainsKey(uid))
                {
                    Destroy(dict[uid].gameObject);
                }
            }
        }

        private void DefaultFuncs()
        {
            if (ActiveLevelData == null) return;

            Spawn();
            DeleteObjectsById();
        }

        public void ReloadLinks()
        {
            QuickInventory = new QuickInventoryModel(FindObjectOfType<Hero>().GetComponent<InventoryComponent>());
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        private bool IsSessionExist()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var session in sessions)
            {
                if (session != this) return true;
            }
            return false;
        }
    }
}
