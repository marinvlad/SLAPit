using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Events;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SimpleEvent _onPlayerAttackBegin;
    [SerializeField] private SimpleEvent _onPlayerAttackEnd;
    [SerializeField ]private TrailRenderer _trailRenderer;
    private Animator _animator;

    [SerializeField] private SimpleEvent _onWrongTarget;
    [SerializeField] private SimpleEvent _onGameOver;
    [SerializeField] private SimpleEvent _onLevelEnd;
    [SerializeField] private SimpleEvent _onGameStart;
    
    [SerializeField] private GameObject _head;

    [SerializeField] private GameObject _ragdoll;
    [SerializeField] private GameObject _deadEffect;
    
    private float _scaleFactor = 1.0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _onPlayerAttackBegin.Subscribe(SlowDownAnimation);
        _onPlayerAttackEnd.Subscribe(FasterAnimation);
        _onWrongTarget.Subscribe(ScaleHead);
        _onWrongTarget.Subscribe(CheckGameOver);
        _onGameOver.Subscribe(GameOver);
        
        _onLevelEnd.Subscribe(LevelEnd);
        _onGameStart.Subscribe(Run);
    }

    private void OnDisable()
    {
        _onPlayerAttackEnd.Unsubscribe(SlowDownAnimation);
        _onPlayerAttackEnd.Unsubscribe(FasterAnimation);
        _onWrongTarget.Unsubscribe(ScaleHead);
        _onWrongTarget.Unsubscribe(CheckGameOver);
        _onGameOver.Unsubscribe(GameOver);
        
        _onLevelEnd.Unsubscribe(LevelEnd);
        _onGameStart.Unsubscribe(Run);
    }

    private void SlowDownAnimation()
    {
        _animator.SetFloat("animSpeed", 0.2f);
        _trailRenderer.time = 15;
    }

    private void FasterAnimation()
    {
        _animator.SetFloat("animSpeed", 1.0f);
        _trailRenderer.time = 10f;
    }

    private void ScaleHead()
    {
        _scaleFactor -= 0.2f;
        _head.transform.DOScale(Vector3.one * _scaleFactor, 0.5f);
    }

    private void CheckGameOver()
    {
        if (_scaleFactor < 0.1f)
        {
            _onGameOver.Invoke();
        }
    }

    private void GameOver()
    {
        GameObject deadEffect = Instantiate(_deadEffect, _head.transform.position, Quaternion.identity);
        GameObject ragDoll = Instantiate(_ragdoll, transform.position, transform.rotation);
        
        Destroy(deadEffect,1.5f);
        Destroy(gameObject);
    }

    private void Run()
    {
        _animator.SetBool("gameStart",true);
    }

    private void LevelEnd()
    {
        _animator.SetBool("winAnim", true);
    }
}
