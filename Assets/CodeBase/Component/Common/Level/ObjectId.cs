using System;
using PixelCrew.Model;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixelCrew.Components
{
    public class ObjectId : MonoBehaviour
    {
        [SerializeField] private string _uniqueId;
        [SerializeField] private bool _isAddedToDestroy = false;

        public string UniqueId => _uniqueId;

#if UNITY_EDITOR
        private void OnValidate()
        {
            return;
            try
            {
                if (PrefabUtility.IsPartOfPrefabAsset(gameObject)) return;
                var stage = PrefabStageUtility.GetCurrentPrefabStage();
                if (stage != null && stage.IsPartOfPrefabContents(gameObject)) return;

                SetUniqueId(); 
            } catch { }
        }
#endif

        [ContextMenu("SetId")]
        public void SetUniqueId()
        {
            if (string.IsNullOrWhiteSpace(_uniqueId))
            {
                _uniqueId = Guid.NewGuid().ToString();
            }
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