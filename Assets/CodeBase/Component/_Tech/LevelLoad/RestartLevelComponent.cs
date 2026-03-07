using PixelCrew.UI.LevelsLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class RestartLevelComponent : MonoBehaviour
    {
        public void Restart()
        {
            var scene = SceneManager.GetActiveScene();
            if (scene != null)
            {
                FindObjectOfType<LevelLoader>().Show(scene.name);
            }
            else
            {
                Debug.LogWarning("Scene not found to restart");
            }
        }
    }
}