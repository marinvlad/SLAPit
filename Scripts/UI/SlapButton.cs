using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlapButton : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private SimpleEvent _onSlapButton;
    [SerializeField] private SimpleEvent _onPlayerAtackBegin;

    private void Start()
    {
        _onSlapButton.Subscribe(OnPressed);
        _onPlayerAtackBegin.Subscribe(OnPlayerAttackBegin);
        gameObject.SetActive(false);
    }
    

    public void OnPointerDown(PointerEventData eventData)
    {
        _onSlapButton.Invoke();
        
    }

    private void OnPressed()
    {
        gameObject.SetActive(false);
    }

    private void OnPlayerAttackBegin()
    {
        gameObject.SetActive(true);
    }
}
