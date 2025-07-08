using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    private Enemy_Melee enemy;
    [SerializeField] private int durability;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Melee>();
    }

    public void ReduceDurability()
    {
        durability--;

        if (durability <= 0)
        {
            enemy.animator.SetFloat("ChaseIndex", 0); // Enables default chase animation
            Destroy(gameObject);
        }
    }
}