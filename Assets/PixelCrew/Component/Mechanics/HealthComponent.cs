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

        public void ApplyDamage(int damageValue)
        {
            _currentHealth -= damageValue;
            _onDamage?.Invoke();
            if (_currentHealth <= 0)
            {
                _onDie?.Invoke();
            }
            Debug.Log("damage " + damageValue + ", health " + _currentHealth);
        }

        public void RecoverHealth(int recover)
        {
            if (_currentHealth + recover >= _maxHealth)
            {
                _currentHealth = _maxHealth;
                Debug.Log("is max health " + _currentHealth);
            }
            else
            {
                _currentHealth += recover;
                _onHealth?.Invoke();
                Debug.Log("recover " + recover + ", health " + _currentHealth);
            }
        } 
    }
}
