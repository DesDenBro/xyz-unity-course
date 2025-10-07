using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public abstract class PrefPersistentProperty<TPropertyType> : PersistentProperty<TPropertyType>
    {
        protected string Key;
        protected PrefPersistentProperty(TPropertyType defVal, string key) : base(defVal)
        {
            Key = key;
        }
    }

    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value;
        private TPropertyType _stored;
        private TPropertyType _defaultValue;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;

        public PersistentProperty(TPropertyType defVal)
        {
            _defaultValue = defVal;
        }
        public TPropertyType Value
        {
            get => _value;
            set
            {
                if (_stored.Equals(value)) return;
                
                var oldValue = _value;
                Write(value);
                _stored = _value = value;

                OnChanged?.Invoke(value, oldValue);
            }
        }
        protected void Init()
        {
            _stored = _value = Read(_defaultValue);
        }

        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);

        public void Validate()
        {
            if (_stored.Equals(_value)) return;
            Value = _value;
        }
    }
}