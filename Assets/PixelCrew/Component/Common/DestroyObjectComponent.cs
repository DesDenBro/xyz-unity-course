using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        public void DestroyObj(GameObject target)
        {
            Destroy(target);
        }
    }
}