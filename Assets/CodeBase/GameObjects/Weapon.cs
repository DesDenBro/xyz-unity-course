using PixelCrew.GameObjects.Creatures;
using UnityEngine;

namespace PixelCrew.GameObjects
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] int _damage;
        [SerializeField] GameObject _prefab;

        public int Damage => _damage;

        public void ArmWeapon(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero == null) return;

            hero.ArmWeapon(_prefab);
        }
    }
}