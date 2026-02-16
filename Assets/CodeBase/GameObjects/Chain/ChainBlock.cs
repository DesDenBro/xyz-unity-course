using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.GameObjects
{
    public class ChainBlock : MonoBehaviour
    {
        [SerializeField] private OutlineSettings _highlight;

        private SpriteRenderer _hsr;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _hsr = _highlight.GetComponent<SpriteRenderer>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _hsr.sprite = _sr.sprite;
        }
    }
}
