using PixelCrew.UI.LevelsLoader;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void Exit()
        {
            FindObjectOfType<LevelLoader>().Show(_sceneName);
        }
    }
}
