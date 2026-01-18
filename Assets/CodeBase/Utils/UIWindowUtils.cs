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

        public static Color GetColor(ColorPalette color, float alpha = 1f)
        {
            switch (color)
            {
                case ColorPalette.PassivePerkUse: return new Color(0.4901961f, 0.4901961f, 0.4901961f, alpha); // серый
                case ColorPalette.ActivePerkUse: return new Color(0, 0.5176471f, 0.003921569f, alpha); // темно-зеленый
            }

            return new Color(0, 0, 0, alpha);
        }

        public enum ColorPalette
        { 
            PassivePerkUse,
            ActivePerkUse
        }
    }
}