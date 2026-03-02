using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    [SerializeField] private float _monsterHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _monsterMoveSpeed;

    private void Start()
    {
        _monsterHealth = _maxHealth;
    }

    private void Hit(float damage)
    {
       _monsterHealth -= damage;
    }
}
