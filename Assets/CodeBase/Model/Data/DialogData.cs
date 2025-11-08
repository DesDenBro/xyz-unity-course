using PixelCrew.Common.Tech;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private string[] _sentences;
        [Space][Space][SerializeField] private UnityEvent _afterDialogEvent;

        public string[] Sentences => _sentences;
        public UnityEvent AfterDialogEvent => _afterDialogEvent;
    }
}