using System.Linq;
using UnityEngine;

namespace PixelCrew.Utils
{
    public class UIWindowUtils : MonoBehaviour
    {
        public static void InitWindow(string path)
        {
            var UIWindow = Resources.Load<GameObject>(path);
            var initWindowTag = UIWindow.tag;
            if (GameObject.FindGameObjectWithTag(initWindowTag)) return;

            var canvas = FindObjectsOfType<Canvas>().FirstOrDefault(x => x.tag == "MenuCanvas");
            Instantiate(UIWindow, canvas.transform);
        }

        public static void ClearContainer(GameObject container)
        {
            if (container == null) return;

            int i = 0;
            var allChildren = new GameObject[container.transform.childCount];
            foreach (Transform child in container.transform)
            {
                allChildren[i] = child.gameObject;
                allChildren[i].SetActive(false);
                i++;
            }

            // удаление сразу не сработает из-за зависимости во время чтения данных о элементах внутри
            foreach (GameObject child in allChildren)
            {
                Destroy(child.gameObject);
            }
        }
    }
}