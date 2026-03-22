using UnityEngine;

namespace PixelCrew.GameObjects
{
    public abstract class OnOffObject : MonoBehaviour
    {
        public abstract void TurnOn();
        public abstract void TurnOff();
    }
}
