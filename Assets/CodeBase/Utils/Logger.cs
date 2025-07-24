using UnityEngine;

namespace PixelCrew.Components.Tech
{
    public class Logger : MonoBehaviour
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }
    }
}