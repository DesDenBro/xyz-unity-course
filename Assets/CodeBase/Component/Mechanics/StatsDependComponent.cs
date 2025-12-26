using PixelCrew.GameObjects.Creatures;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Components
{
    public class StatsDependComponent : MonoBehaviour 
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private StatId _stat;

        private void Awake()
        {
            var hero = FindObjectOfType<Hero>();
            if (hero == null)
            {
                Debug.Log("Hero not found for stat depend");
                return;
            }

            if (_container == null) _container = this.gameObject;
            switch (_stat)
            {
                case StatId.RangeDamage:
                    var scaleCoef = hero.AttackRangeRange / 2;
                    _container.transform.localScale = new Vector3(
                        _container.transform.localScale.x + scaleCoef,
                        _container.transform.localScale.y + scaleCoef,
                        _container.transform.localScale.z + scaleCoef
                    );
                    break;
            }
        }
    }
}