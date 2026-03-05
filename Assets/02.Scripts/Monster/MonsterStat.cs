using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;

    [SerializeField] private float _attackPower = 10f;
    [SerializeField] private float _attackSpeed = 2f;

    public float Health => _currentHealth;
    public float AttackPower => _attackPower;
    public float AttackSpeed => _attackSpeed;
    public bool IsDead => _currentHealth <= 0f;


    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);
    }
}
