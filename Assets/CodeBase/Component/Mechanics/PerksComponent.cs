using UnityEngine;
using PixelCrew.Model.Data;

namespace PixelCrew.Components
{
    public class PerksComponent : MonoBehaviour 
    {
        [SerializeField] private PerksData _perksData;

        public PerksData PerksData => _perksData;

        public void SetPerks(PerksData pd)
        {
            _perksData = pd;
        }
    }   
}