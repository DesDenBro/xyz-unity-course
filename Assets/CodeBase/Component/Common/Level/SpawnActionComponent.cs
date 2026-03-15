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

        private const string _predicatName = "sas-a-";
        private void RefreshNamesSas()
        {
            foreach (var spawner in _spawners)
            {
                var names = spawner.GetPrefabChildrenObjNames<SpriteAnimationState>();
                foreach (var name in names)
                {
                    if (!name.Contains(_predicatName)) continue;

                    var clearedName = name.Trim().Replace(_predicatName, string.Empty);
                    if (_spawnComponentsByNameSas.ContainsKey(clearedName)) continue;

                    _spawnComponentsByNameSas.Add(clearedName, spawner);
                }
            }
        }

        public void SpawnAction(string sasName)
        {
            if (!_spawnComponentsByNameSas.ContainsKey(sasName)) 
            {
                RefreshNamesSas();
                if (!_spawnComponentsByNameSas.ContainsKey(sasName)) return;
            }

            var spawner = _spawnComponentsByNameSas[sasName];
            if (spawner == null) return;

            var obj = spawner.Spawn();

            var saComp = obj?.GetComponent<SpriteAnimation>();
            if (saComp == null) return;

            // жрет память, а по факту бессмысленно
            // obj.name = gameObject.name + "_spawn_" + sasName;
            saComp.SetupAnimation(sasName);

            // добавляем вектор отклонения относительно нахождения стартовой точки спавна из sas
            var sas = saComp.GetSas(sasName);
            if (sas == null) return;

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
            sas.gameObject.SetActive(true);

            saComp.RunAnimation();
        }
    }
}
