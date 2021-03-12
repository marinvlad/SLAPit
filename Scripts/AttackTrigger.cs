using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Events;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private SimpleEvent _onPlayerAttackBegin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {    
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].CanBeAttacked = true;
            }
            _onPlayerAttackBegin.Invoke();
        }
    }
}