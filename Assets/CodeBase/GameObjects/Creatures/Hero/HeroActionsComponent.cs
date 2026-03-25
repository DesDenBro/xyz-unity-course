using PixelCrew.Components;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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

        public void SetInnerLightState(bool state)
        {   
            var hero = FindObjectOfType<Hero>();
            if (hero == null) return;

            hero.GetComponentInChildren<Light2D>().enabled = false;
        }
    }
}