using PixelCrew.GameObjects;
using PixelCrew.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.LevelsLoader
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _transitionTime;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfrerSceneLoad()
        {
            InitLoader();
        }
        private static void InitLoader()
        {
            SceneManager.LoadScene("LevelLoader", LoadSceneMode.Additive);
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Show(string sceneName)
        {
            StartCoroutine(StartAnimation(sceneName));
        }

        private IEnumerator StartAnimation(string sceneName)
        {
            _animator.SetKeyVal(AnimationKeys.UI.LevelLoad.IsEnabled, true);
            yield return new WaitForSeconds(_transitionTime);

            SceneManager.LoadScene(sceneName);
            _animator.SetKeyVal(AnimationKeys.UI.LevelLoad.IsEnabled, false);
        }
    }
}