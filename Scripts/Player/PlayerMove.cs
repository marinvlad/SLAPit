using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Events;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private SimpleEvent _onPlayerAttackBegin;
    [SerializeField] private SimpleEvent _onPlayerAttackEnd;
    [SerializeField] private SimpleEvent _onLevelEnd;
    [SerializeField] private SimpleEvent _onGameStart;
    

    private void Start()
    {
        _onPlayerAttackBegin.Subscribe(SlowDown);
        _onPlayerAttackEnd.Subscribe(GoFaster);
        _onLevelEnd.Subscribe(LevelEnd);
        _onGameStart.Subscribe(GameStart);
        enabled = false;
    }

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    private void SlowDown()
    {
        _speed = 0.2f;
    }

    private void GoFaster()
    {
        _speed = 1;
    }

    private void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * _speed);
    }

    private void LevelEnd()
    {
        enabled = false;
    }

    private void GameStart()
    {
        enabled = true;
    }
}
