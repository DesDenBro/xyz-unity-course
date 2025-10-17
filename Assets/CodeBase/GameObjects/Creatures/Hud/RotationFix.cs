using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures.Hud
{
    public class RotationFix : MonoBehaviour
    {
        void FixedUpdate()
        {
            var parentTransform = gameObject.GetComponentInParent<Transform>();
            var x = (parentTransform.lossyScale.x < 0 ? -1 : 1) * gameObject.transform.localScale.x;
            gameObject.transform.localScale = new Vector3(x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
    }
}