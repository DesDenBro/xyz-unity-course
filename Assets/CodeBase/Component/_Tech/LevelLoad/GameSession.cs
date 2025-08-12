using PixelCrew.Model.Data;
using UnityEngine;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private LevelsData _levelsData;

        private bool _disposed = false;

        public PlayerData PlayerData => _playerData;
        public LevelsData LevelsData => _levelsData;
        public bool Disposed => _disposed;


        private void Awake()
        {
            if (IsSessionExist())
            {
                Destroy(gameObject);
                _disposed = true;
            }
            else
            {
                DontDestroyOnLoad(this);
            }
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
