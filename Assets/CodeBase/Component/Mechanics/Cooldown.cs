using System;
using UnityEngine;

namespace PixelCrew.Components
{
    [Serializable]
    public class Cooldown
    {
        public Cooldown() { }
        public Cooldown(float value) : base() { _value = value; } 

        [SerializeField] private float _value;

        private float _timesUp;

        public void Reset()
        {
            _timesUp = Time.time + _value;
        }

        public bool IsReady => _timesUp <= Time.time;
    }
}
