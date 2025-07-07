using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHealth;
        [SerializeField] private UnityEvent _onDie;

        public int Health => _currentHealth;
        public int MaxHealth => _maxHealth;


        public void SetHealth(int health) { _currentHealth = health; }
        public void SetMaxHealth(int maxHealth) { _maxHealth = maxHealth; }


        public void ApplyDamage(int damagePoints)
        {
            _currentHealth -= damagePoints;
            _onDamage?.Invoke();
            if (_currentHealth <= 0)
            {
                _onDie?.Invoke();
            }
            Debug.Log("damage " + damagePoints + ", health " + _currentHealth);
        }
        [ContextMenu("Hit")]
        public void HitIt() => ApplyDamage(100);


        public void RecoverHealth(int healthPoints)
        {
            if (_currentHealth + healthPoints >= _maxHealth)
            {
                _currentHealth = _maxHealth;
                Debug.Log("is max health " + _currentHealth);
            }
            else
            {
                _currentHealth += healthPoints;
                _onHealth?.Invoke();
                Debug.Log("recover " + healthPoints + ", health " + _currentHealth);
            }
        }
        [ContextMenu("Health")]
        public void HealthIt() => RecoverHealth(100);
    }
}
