using PixelCrew.Common.Tech;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnActionComponent : MonoBehaviour
    {
        [SerializeField] private SpawnComponent[] _spawners;

        private IDictionary<string, SpawnComponent> _spawnComponentsByNameSas;

        public void Awake()
        {
            _spawnComponentsByNameSas = new Dictionary<string, SpawnComponent>(_spawners.Length);
            RefreshNamesSas();
        }

        private void RefreshNamesSas()
        {
            foreach (var spawner in _spawners)
            {
                var names = spawner.GetPrefabChildrenObjNames<SpriteAnimationState>();
                foreach (var name in names)
                {
                    if (_spawnComponentsByNameSas.ContainsKey(name)) continue;
                    _spawnComponentsByNameSas.Add(name, spawner);
                }
            }
        }

        public void SpawnAction(string sasName)
        {
            var fullName = "sas-a-" + sasName;
            if (!_spawnComponentsByNameSas.ContainsKey(fullName)) 
            {
                RefreshNamesSas();
                if (!_spawnComponentsByNameSas.ContainsKey(fullName)) return;
            }

            var spawner = _spawnComponentsByNameSas[fullName];
            if (spawner == null) return;

            var obj = spawner.Spawn();

            var saComp = obj?.GetComponent<SpriteAnimation>();
            if (saComp == null) return;

            obj.name = obj.name + "_spawn_" + sasName;
            saComp.SetStartSasName(sasName);

            // добавляем вектор отклонения относительно нахождения стартовой точки спавна из sas
            var sas = saComp.GetSas(sasName);
            if (sas == null) return;

            sas.gameObject.SetActive(true);
            var sasTransform = sas.transform;
            obj.transform.position += new Vector3(
                sasTransform.localPosition.x * sasTransform.parent.lossyScale.x,
                sasTransform.localPosition.y * sasTransform.parent.lossyScale.y,
                sasTransform.localPosition.z * sasTransform.parent.lossyScale.z 
            );
            obj.transform.localScale = new Vector3(
                sasTransform.parent.lossyScale.x * sasTransform.localScale.x,
                sasTransform.parent.lossyScale.y * sasTransform.localScale.y,
                sasTransform.parent.lossyScale.z * sasTransform.localScale.z
            );
        }
    }
}
