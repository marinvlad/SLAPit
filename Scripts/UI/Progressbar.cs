using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Events;
using UnityEngine;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private SimpleEvent _onWrongTarget;

    private void OnEnable()
    {
        _onWrongTarget.Subscribe(UpdateValue);
        _slider.value = 0;
    }

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void UpdateValue()
    {
        _slider.value += 0.2f;
    }
}
