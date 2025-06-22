using PixelCrew.Common;
using PixelCrew.Common.Tech;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private bool _isActive = true;
        [SerializeField] private bool _checksToActivate;
        [SerializeField] private UnityEventGameObject[] _actions;

        private ThingSpecification _thingSpecification;

        private void Start()
        {
            _thingSpecification = GetComponent<ThingSpecification>();
        }

        public void Interact(GameObject _activator)
        {
            if (!_isActive && _checksToActivate)
            {
                var checksDone = true;
                var inventoryComp = _activator.GetComponent<InventoryComponent>();
                if (inventoryComp == null || _thingSpecification == null) return;

                checksDone = checksDone && inventoryComp.CheckKeyCountToEvent(_thingSpecification.KeysAmount);
                checksDone = checksDone && inventoryComp.CheckMoneyCountToEvent(_thingSpecification.CostAmount);

                if (checksDone)
                {
                    _isActive = inventoryComp.ChangeKeyAmount(-_thingSpecification.KeysAmount) && inventoryComp.ChangeMoneyAmount(-_thingSpecification.CostAmount);
                }
            }

            if (_isActive)
            {
                foreach (var action in _actions)
                {
                    action?.Invoke(_activator);
                }
            }
        }
    }
}
