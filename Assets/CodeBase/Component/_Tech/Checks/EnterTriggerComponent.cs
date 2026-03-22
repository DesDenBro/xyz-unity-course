using PixelCrew.Common.Tech;
using PixelCrew.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private string _ignoreTag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private UnityEventGameObject[] _actions;

        private IReadOnlyCollection<string> _tags;
        private IReadOnlyCollection<string> _ignoreTags;

        public void Awake()
        {
            _tags = _tag?.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToArray() ?? Array.Empty<string>();
            _ignoreTags = _ignoreTag?.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToArray() ?? Array.Empty<string>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(_layer)) return;
            
            if (_tags.Count > 0)
            {
                var found = false;
                foreach (string tag in _tags)
                {
                    if (other.gameObject.CompareTag(tag)) found = true;
                }
                if (!found) return;
            }
            if (_ignoreTags.Count > 0)
            {
                foreach (string tag in _ignoreTags)
                {
                    if (other.gameObject.CompareTag(tag)) return;
                }
            }

            foreach (var action in _actions)
            {
                action?.Invoke(other.gameObject);
            }
        }
    }
}
