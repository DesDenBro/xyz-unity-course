using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public GameObject Spawn()
        {
            var instanse = Instantiate(_prefab, _target.position, Quaternion.identity);
            //Instantiate(_prefab, _target); // создание внутри объекта родителя вызова
            instanse.transform.localScale = _target.lossyScale; // lossyScale - объект глобально, тут мы поворачиваем спрайт
            
            return instanse;
        }
    }
}