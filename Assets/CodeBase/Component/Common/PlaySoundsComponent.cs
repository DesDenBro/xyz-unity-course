using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components
{
    public class PlaySoundsComponent : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioData[] _sounds;

        private void Awake()
        {
            if (_source == null)
            {
                _source = GetComponent<AudioSource>();
            }
        }

        public void Play(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (_source == null) { Debug.Log("Audio source empty to play sound!"); return; }

            var sound = _sounds?.FirstOrDefault(x => x.Id == id);
            if (sound == null) return;

            _source.PlayOneShot(sound.Clip);
        }
    }

    [Serializable]
    public class AudioData
    {
        [SerializeField] private string _id;
        [SerializeField] private AudioClip[] _clips;

        public string Id => _id;
        public AudioClip Clip => _clips.Length == 1 ? _clips[0] : _clips[UnityEngine.Random.Range(0, _clips.Length)];
    }
}
