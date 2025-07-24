using UnityEngine;

namespace PixelCrew.Components
{
    public class HigthlightInteractableComponent : MonoBehaviour
    {
        public void Higthlight(GameObject go)
        {
            var interactable = go.GetComponent<InteractableComponent>();
            if (interactable == null) return;

            interactable.Highlight();
        }
    }
}
