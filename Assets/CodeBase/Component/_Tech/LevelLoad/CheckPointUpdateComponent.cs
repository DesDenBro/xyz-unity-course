using PixelCrew.GameObjects.Creatures;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class CheckPointUpdateComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private UnityEvent _actionAfterCheckPoint;

        public string Id => _id;

        public void UpdateSessionData()
        {
            var hero = FindObjectOfType<Hero>();
            if (hero == null) return;

            hero.UpdateSessionData(_id);

            _actionAfterCheckPoint?.Invoke();
        }
    }
}
