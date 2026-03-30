using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        [SerializeField] private bool _isImmune;
        [SerializeField] private Cooldown _damageCooldown;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHealth;
        [SerializeField] private UnityEvent _onDie;

        public int Health => _currentHealth;
        public int MaxHealth => _maxHealth;

        public delegate void OnHealthChangedDel(int newValue, int oldValue);
        public event OnHealthChangedDel OnHealthChanged;

        public void SetImmune(bool value)
        {
            _isImmune = value;
        }
        public void SetHealth(int health) 
        {
            var oldHealth = _currentHealth;
            _currentHealth = health;
            OnHealthChanged?.Invoke(_currentHealth, oldHealth);
            Debug.Log("current hp: " + _currentHealth);
        }
        public void SetMaxHealth(int maxHealth) 
        {
            _maxHealth = maxHealth; 
            OnHealthChanged?.Invoke(_currentHealth, _currentHealth);
            Debug.Log("new hp max: " + _maxHealth + ", current hp: " + _currentHealth);
        }

        public void ApplyDamage(int damagePoints)
        {
            if (_currentHealth <= 0 || _isImmune) return;

            if (!_damageCooldown.IsReady) return;
            _damageCooldown.Reset();

            var oldHealth = _currentHealth;
            _currentHealth -= damagePoints;
            _onDamage?.Invoke();
            OnHealthChanged?.Invoke(_currentHealth, oldHealth);

            if (_currentHealth <= 0)
            {
                _onDie?.Invoke();
            }
            Debug.Log("damage " + damagePoints + ", health " + _currentHealth);
        }

        public void RecoverHealth(int healthPoints)
        {
            if (_currentHealth <= 0) return;

            var oldHealth = _currentHealth;
            _onHealth?.Invoke();

            if (_currentHealth + healthPoints >= _maxHealth)
            {
                _currentHealth = _maxHealth;
                Debug.Log("is max health " + _currentHealth);
            }
            else
            {
                _currentHealth += healthPoints;
                Debug.Log("recover " + healthPoints + ", health " + _currentHealth);
            }
            OnHealthChanged?.Invoke(_currentHealth, oldHealth);
        }

        [ContextMenu("Health")]
        public void HealthIt() => RecoverHealth(100);
    }
}
