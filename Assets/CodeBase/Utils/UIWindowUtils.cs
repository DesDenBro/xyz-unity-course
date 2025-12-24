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
    }
}