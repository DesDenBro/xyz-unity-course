using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixleCrew.Components
{
    public class RestartLevelComponent : MonoBehaviour
    {
        public void Restart()
        {
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