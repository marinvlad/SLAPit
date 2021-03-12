using System.Collections;
using System.Collections.Generic;
using Systems.Events;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Logo : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private GameObject _levelIndicator;
    [SerializeField] private Text _pressToStart;
    [SerializeField] private Image _pressToStartImage;
    [SerializeField] private SimpleEvent _onGameStart;
    
    
    private void Awake()
    {
        _pressToStart.DOFade(0, 2f).SetLoops(-1, LoopType.Yoyo);
        _pressToStartImage.rectTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 2f).SetLoops(-1,LoopType.Yoyo);
    }

    private void HideMenu()
    {
        _levelIndicator.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HideMenu();
        _onGameStart.Invoke();
    }
}
