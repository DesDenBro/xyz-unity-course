using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model
{
    public class DestroyGameSession : MonoBehaviour
    {
        public void FindAndDestroyGS()
        {
            var existSession = GameSessionSearch.Get(FindObjectsOfType<GameSession>);
            if (existSession != null) Destroy(existSession.gameObject);
        }
    }
}