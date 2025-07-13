using System.Collections;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures
{
    public abstract class BasePatrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}
