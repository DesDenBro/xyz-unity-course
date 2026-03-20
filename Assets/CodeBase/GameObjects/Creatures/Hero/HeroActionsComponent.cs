using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures
{
    public class HeroActionsComponent : MonoBehaviour
    {
        public void SetInputLock(bool value)
        {
            var heroInputLock = FindObjectOfType<HeroMovementLock>();
            if (heroInputLock == null) return;

            heroInputLock.SetLock(value);
        }

        public void SetImmune(bool value)
        {
            var hero = FindObjectOfType<Hero>();
            if (hero == null) return;

            hero.GetComponent<HealthComponent>().SetImmune(value);
        }
    }
}