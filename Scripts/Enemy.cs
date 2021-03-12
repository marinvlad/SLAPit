using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Events;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   private bool _canBeAttacked;
   private bool _isTarget;
   [SerializeField] private bool _isHarmful;
   [SerializeField] private SpriteRenderer _canBeAttackedIndicator;
   [SerializeField] private GameObject _ragdollPrefab;
   [SerializeField] private SimpleEvent _onPlayerAttackBegin;
   [SerializeField] private SimpleEvent _onPlayerAttackEnd;

   private Animator _animator;

   private void Awake()
   {
      _canBeAttacked = false;
      _isTarget = false;
      _canBeAttackedIndicator.enabled = false;
      _animator = GetComponent<Animator>();
   }

   private void OnEnable()
   {
      _onPlayerAttackBegin.Subscribe(SlowDownAnimation);
      _onPlayerAttackEnd.Subscribe(FasterAnimation);
   }

   private void OnDisable()
   {
      _onPlayerAttackBegin.Unsubscribe(SlowDownAnimation);
      _onPlayerAttackEnd.Unsubscribe(FasterAnimation);
   }

   public void TakeHit()
   {
      StartCoroutine(PlayDeadEffect());
   }

   private IEnumerator PlayDeadEffect()
   {
      yield return new WaitForSeconds(0.5f);
      
      GameObject _ragdoll = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
      Destroy(_ragdoll.gameObject,2f);
      Destroy(gameObject);
   }

   private void SlowDownAnimation()
   {
      _animator.SetFloat("animSpeed", 0.2f);
   }

   private void FasterAnimation()
   {
      _animator.SetFloat("animSpeed", 1.0f);
   }
   
   public bool CanBeAttacked
   {
      get => _canBeAttacked;
      set
      {
         _canBeAttackedIndicator.enabled = true;
         _canBeAttacked = value;
      }
   }

   public bool IsTarget
   {
      get => _isTarget;
      set
      {
         _isTarget = value;
         if (_isTarget)
         {
            _canBeAttackedIndicator.color = Color.red;
         }
      }
   }

   public bool IsHarmful => _isHarmful;
}
