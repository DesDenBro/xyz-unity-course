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
    }
}
