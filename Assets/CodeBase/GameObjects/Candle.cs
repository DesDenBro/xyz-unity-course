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
            _current = StartCoroutine(Action());
        }

        public void TurnOff()
        {
            if (!IsActive) return;

            _isActive = false;
            gameObject.SetActive(false);
            if (_current != null) StopCoroutine(_current);
        }

        private IEnumerator Action()
        {
            var inv = GetComponentInParent<InventoryComponent>();
            if (inv == null || inv.Count(InventoryItemName.Candle) == 0) yield break;

            gameObject.SetActive(true);
            do
            {
                var count = inv.Count(InventoryItemName.Candle);
                if (count == 0) break;

                var finalRadius = _baseLightRadius * (count <= 10 ? count / 10 : 1);
                var light = _light.GetComponent<Light2D>();
                light.pointLightOuterRadius = finalRadius;

                var success = inv.ChangeInventoryItemCount(InventoryItemName.Candle, -1);
                if (success) break;

                yield return new WaitForSeconds(3);
            } while (true);
            
            TurnOff();
        }
    }
}
