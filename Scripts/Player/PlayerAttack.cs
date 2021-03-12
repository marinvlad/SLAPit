using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Events;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private CustomInputSystem _customInputSystem;
    [SerializeField] private SimpleEvent _onPlayerAttackEnd;
    [SerializeField] private SimpleEvent _onWrongTarget;
    [SerializeField] private SimpleEvent _onSlapButton;
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private GameObject _hand;
    
    private List<Enemy> _enemyAttackOrder;
    private Animator _animator;
    private Tween _tween;

    private void Awake()
    {
        _enemyAttackOrder = new List<Enemy>();
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();
    }

    private void OnEnable()
    {
        _customInputSystem.onPointerDownEvent += TargetEnemy;
        _onSlapButton.Subscribe(Attack);
    }

    private void OnDisable()
    {
        _customInputSystem.onPointerDownEvent -= TargetEnemy;
        _onSlapButton.Invoke();
    }

    private void Attack()
    {
        _playerMove.enabled = false;
        _onPlayerAttackEnd.Invoke();
        if(_enemyAttackOrder.Count>0)
        {
            for (int i = 0; i < _enemyAttackOrder.Count; i++)
            {
                _animator.speed = 1;
                Timing.RunCoroutine(AttackEnemy(_enemyAttackOrder[i]));
            }
        }
        else
        {
            transform.DOMove(new Vector3(0, transform.position.y, transform.position.z + 10), 0.1f );
            _playerMove.enabled = true;
        }
    }

    private bool isInAttack = false;

    private IEnumerator<float> AttackEnemy(Enemy enemy)
    {
        while (_tween != null && _tween.IsPlaying() || isInAttack)
            yield return 0;

        _playerMove.enabled = false;
        Vector3 _enemyPosition = enemy.gameObject.transform.position;
        _enemyPosition.z -= 0.5f;
        
        _tween = transform.DOMove(_enemyPosition, 0.1f)
            .OnComplete(() =>
            {
                enemy.TakeHit();
                CheckIfEnemyHarmful(enemy);
                StartCoroutine(PlayAttackAnimation());
                _enemyAttackOrder.Remove(enemy);
            });
    }

    private IEnumerator PlayAttackAnimation()
    {
        isInAttack = true;
        _animator.speed = 2;
        _hand.transform.DOScale(new Vector3(3, 3, 3), 0.5f);
        _animator.SetBool("attack", true);
        yield return new WaitForSeconds(1);
        isInAttack = false;
        _animator.SetBool("attack", false);
        _hand.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        if (_enemyAttackOrder.IsNullOrEmpty())
        {
            _animator.speed = 1;
            transform.DOMove(new Vector3(0, transform.position.y, transform.position.z + 5), 0.1f );
            _playerMove.enabled = true;
        }
    }

    private void CheckIfEnemyHarmful(Enemy enemy)
    {
        if (enemy.IsHarmful)
        {
            Debug.Log("Enemy was harmful. Congrats!");
        }
        else
        {
            _onWrongTarget.Invoke();
            Debug.Log("Enemy was not harmful. Head will scale down!");
        }
    }

    private void TargetEnemy(PointerEventData eventData)
    {
        Ray _ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, 1 << 8))
        {
            Enemy _hitEnemy = _hit.transform.gameObject.GetComponent<Enemy>();
            if (!_enemyAttackOrder.Contains(_hitEnemy))
            {
                _hitEnemy.IsTarget = true;
                _enemyAttackOrder.Add(_hitEnemy);
            }
        }
    }
}