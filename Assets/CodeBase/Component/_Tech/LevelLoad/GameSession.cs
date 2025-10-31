﻿using PixelCrew.Components;
using PixelCrew.GameObjects.Creatures;
using PixelCrew.Model.Data;
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

        public QuickInventoryModel QuickInventory {  get; private set; }

        private void Awake()
        {
            LoadHud();

            if (IsSessionExist())
            {
                Destroy(gameObject);
                _disposed = true;
                return;
            }
            else
            {
                InitModels();
                DontDestroyOnLoad(this);
            }

            ReloadLinks();
        }

        private void InitModels() { }

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
