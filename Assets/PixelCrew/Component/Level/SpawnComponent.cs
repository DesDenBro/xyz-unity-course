using PixelCrew.Common.Tech;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void SpawnIt() => Spawn();

        public IReadOnlyCollection<string> GetPrefabChildrenObjNames<T>() where T : MonoBehaviour
        {
            return _prefab.GetComponentsInChildren<T>(true).Select(x => x.name).ToList();
        }

        public GameObject Spawn()
        {
            var instanse = Instantiate(_prefab, _target.position, Quaternion.identity);
            //Instantiate(_prefab, _target); // создание внутри объекта родителя вызова
            instanse.transform.localScale = _target.lossyScale; // lossyScale - объект глобально, тут мы поворачиваем спрайт
            
            return instanse;
        }
    }
}