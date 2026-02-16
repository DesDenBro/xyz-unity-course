using System.Collections;
using PixelCrew.Components;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.GameObjects
{
    public class Candle : MonoBehaviour
    {
        [SerializeField] private GameObject _light;
        
        private const float _baseLightRadius = 2.5f;
        private Coroutine _current;
        private bool _isActive = false;
        
        public bool IsActive => _isActive;

        public void TurnOn()
        {
            if (IsActive) { TurnOff(); return; }

            _isActive = true;
            if (_current != null) StopCoroutine(_current);
            gameObject.SetActive(true);
            _current = StartCoroutine(Action());
        }

        public void TurnOff()
        {
            if (!IsActive) return;

            _isActive = false;
            if (_current != null) StopCoroutine(_current);
            gameObject.SetActive(false);
        }

        private IEnumerator Action()
        {
            var inv = GetComponentInParent<InventoryComponent>();
            if (inv != null && inv.Count(InventoryItemName.Candle) > 0)
            {
                do
                {
                    var count = inv.Count(InventoryItemName.Candle);
                    if (count == 0) break;

                    var finalRadius = _baseLightRadius * (count <= 5 ? (float)count / 5 : 1);
                    if (finalRadius < 0.5f) finalRadius = 0.5f;
                    var light = _light.GetComponent<Light2D>();
                    light.pointLightOuterRadius = finalRadius;

                    var success = inv.ChangeInventoryItemCount(InventoryItemName.Candle, -1);
                    if (!success) break;

                    yield return new WaitForSeconds(5);
                } while (true);
            }
            
            TurnOff();
        }
    }
}
