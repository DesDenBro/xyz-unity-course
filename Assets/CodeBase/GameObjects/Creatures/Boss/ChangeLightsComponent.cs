using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.GameObjects.Creatures.Boss
{
    public class ChangeLightsComponent : MonoBehaviour
    {
        [SerializeField] private Light2D[] _lights;

        [ColorUsage(true)][SerializeField] private Color _color;
        [ColorUsage(true)][SerializeField] private Color _baseColor;
        
        public void SetColor(bool reset = false)
        {
            foreach (var light2D in _lights)
            {
                light2D.color = reset ? _baseColor : _color;
            }
        }
    }
}
