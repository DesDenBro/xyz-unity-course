using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class RestartLevelComponent : MonoBehaviour
    {
        public void Restart()
        {
            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);

            var scene = SceneManager.GetActiveScene();
            if (scene != null)
            {
                SceneManager.LoadScene(scene.name);
            }
            else
            {
                Debug.LogWarning("Scene not found to restart");
            }
        }
    }
}