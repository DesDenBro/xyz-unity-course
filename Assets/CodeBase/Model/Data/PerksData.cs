using System;
using System.Collections.Generic;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class PerksData 
    {
        [SerializeField] private StringProperty _used = new StringProperty();
        [SerializeField] private List<string> _unlocked;

        public StringProperty Used => _used;


        public void AddPerk(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;

            if (!_unlocked.Contains(id)) 
                _unlocked.Add(id);
        }

        public bool IsUnlocked(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            return _unlocked.Contains(id);
        }
                
        public bool IsUsed(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            return IsUnlocked(id) && _used.Value == id;
        }
    }
}