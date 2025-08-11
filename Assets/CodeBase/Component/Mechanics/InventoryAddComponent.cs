using PixelCrew.GameObjects.Creatures;
using UnityEngine;

namespace PixelCrew.Components
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero == null) return;

            hero.AddInInventory(_id, _count);
        }
    }
}
