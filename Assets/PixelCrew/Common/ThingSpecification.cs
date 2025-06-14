using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew.Common
{
    public class ThingSpecification : MonoBehaviour
    {
        [SerializeField] private int _amount;
        [SerializeField] private int _damage;
        [SerializeField] private int _recovery;

        public int Amount => _amount;
        public int Damage => _damage;
        public int Recovery => _recovery;
    }
}