using PixelCrew.Common.Tech;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private string _ignoreTag;
        [SerializeField] private UnityEventGameObject _action;

        private IReadOnlyCollection<string> _tags;
        private IReadOnlyCollection<string> _ignoreTags;

        public void Awake()
        {
            _tags = _tag?.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToArray() ?? Array.Empty<string>();
            _ignoreTags = _ignoreTag?.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToArray() ?? Array.Empty<string>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_tags.Count > 0 && !_tags.Any(x => other.gameObject.CompareTag(x))) return;
            if (_ignoreTags.Count > 0 && _ignoreTags.Any(x => other.gameObject.CompareTag(x))) return;

            _action?.Invoke(other.gameObject);
        }
    }
}
