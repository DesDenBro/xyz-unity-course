using UnityEngine;

namespace PixelCrew.Components
{
    public class ActionInteractComponent : MonoBehaviour
    {
        private bool _isActionPressed;

        public void SetIsPressed(bool val)
        {
            _isActionPressed = val;
        }

        public void ActionInteract(GameObject go)
        {
            var interactable = go.GetComponent<InteractableComponent>();
            if (interactable == null) return;

            interactable.Interact(this.gameObject, _isActionPressed);
        }
    }
}
