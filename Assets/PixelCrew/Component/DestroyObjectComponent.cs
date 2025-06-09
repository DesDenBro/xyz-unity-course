using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objToDestroy;

        public void DestroyObj()
        {
            Destroy(_objToDestroy);
        }
    }
}