using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components.Audio
{
    public class AutoContinueMusicComponent : MonoBehaviour
    {
        void Start()
        {
            var audioSource = gameObject.GetComponent<AudioSource>();
            var session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);
            if (session != null)
            {
                audioSource.timeSamples = (int)(session.LevelsData.MusicPlayStartTime * audioSource.clip.frequency);
            }
            audioSource.Play();
        }

    }
}
