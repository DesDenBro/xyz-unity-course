using System;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components
{
    public interface IHaveId
    {
        string Id { get; }
    }
    
    public class ObjectId : MonoBehaviour
    {
        [SerializeField] private string _uniqueId;
        [SerializeField] private bool _isAddedToDestroy = false;

        public string UniqueId => _uniqueId;
    
        [ContextMenu("Set new UId")]
        public void SetNewUniqueId()
        {
            _uniqueId = Guid.NewGuid().ToString();
        }

        public void AddToDestroyed()
        {
            if (_isAddedToDestroy) return;
            _isAddedToDestroy = true;

            if (string.IsNullOrWhiteSpace(_uniqueId))
            {
                Debug.Log("Object " + gameObject.name + " has no uid!");
                return;
            }

            var session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);
            if (session == null || session.ActiveLevelData == null) return;

            session.LevelsData.AddObjectToDelete(session.ActiveLevelData.Name, _uniqueId);
        }

        private void OnDestroy()
        {
            AddToDestroyed();
        }
    }
}