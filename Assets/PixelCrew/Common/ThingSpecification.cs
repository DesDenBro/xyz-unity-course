using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew.Common
{
    public class ThingSpecification : MonoBehaviour
    {
        [SerializeField] private int _costAmount;
        [SerializeField] private int _keysAmount;
        [SerializeField] private int _damagePoints;
        [SerializeField] private int _healthPoints;

        public int CostAmount => _costAmount;
        public int KeysAmount => _keysAmount;
        public int DamagePoints => _damagePoints;
        public int HealthPoints => _healthPoints;
    }
}