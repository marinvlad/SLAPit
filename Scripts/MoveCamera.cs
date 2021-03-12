using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Events;
using Systems.Utility;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private SimpleEvent _onLevelEnd;
    private bool _followPlayer = true;

    private void Awake()
    {
        _followPlayer = true;
        StartFollow();
    }

    private void OnEnable()
    {
        _onLevelEnd.Subscribe(LevelEnd);
    }

    private void OnDisable()
    {
        _onLevelEnd.Unsubscribe(LevelEnd);
    }

    public GameObject Player
    {
        get => _player;
        set
        {
            _player = value;
            StartFollow();
        }
    }

    private void StartFollow()
    {
        _followPlayer = true;
        Timing.RunCoroutine(Move().CancelWith(_player.gameObject), Segment.Update);
    }
    

    private IEnumerator<float> Move()
    {
        while (_followPlayer)
        {
            var playerPosition = _player.transform.TransformPoint(_positionOffset);
            transform.position = new Vector3(transform.position.x, transform.position.y, playerPosition.z);
            yield return 0;
        }
    }

    private void LevelEnd()
    {
        _followPlayer = false;
        Timing.KillCoroutines();
        
        Vector3 finalPosition = new Vector3(_player.transform.position.x + 1, transform.position.y , _player.transform.position.z+ 2);
        transform.DOMove(finalPosition, 0.5f).OnComplete(() =>
        {
            transform.DOLookAt(_player.transform.position, 0.5f);
        });
    }
}