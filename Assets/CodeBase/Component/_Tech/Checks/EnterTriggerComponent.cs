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
            
            if (_tags.Count > 0 && !_tags.Any(x => other.gameObject.CompareTag(x))) return;
            if (_ignoreTags.Count > 0 && _ignoreTags.Any(x => other.gameObject.CompareTag(x))) return;

            foreach (var action in _actions)
            {
                action?.Invoke(other.gameObject);
            }
        }
    }
}
