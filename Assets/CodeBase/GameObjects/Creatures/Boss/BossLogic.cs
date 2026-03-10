using PixelCrew.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures.Boss
{
    public class BossLogic : MonoBehaviour
    {
        private HealthComponent _healthComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}