using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class PerksData 
    {
        [SerializeField] private StringProperty _used = new StringProperty();
        [SerializeField] private StringProperty _activePassives = new StringProperty();
        [SerializeField] private List<string> _unlocked;


        public StringProperty Used => _used;
        public StringProperty ActivePassives => _activePassives;
        public IList<string> ActivePassivesList => _activePassives.Value.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        public void AddPerk(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            if (_unlocked.Contains(id)) return;
            
            var perk = DefsFacade.I.Perks.Get(id);
            if (perk.IsVoid) return;

            _unlocked.Add(id);
            if (perk.IsPassive) _activePassives.Value = _activePassives.Value + id + ";";
        }

        public bool IsUnlocked(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            return _unlocked.Contains(id);
        }
                
        public bool IsUsed(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !IsUnlocked(id)) return false;
            if (IsPassive(id)) return true;
            
            return _used.Value == id;
        }

        public bool IsPassive(string id)
        {
            var perk = DefsFacade.I.Perks.Get(id);
            return perk.IsPassive;
        }

        public PerksData Clone()
        {
            var json = JsonUtility.ToJson(this);
            var clone = JsonUtility.FromJson<PerksData>(json);
            return clone;
        }
    }
}