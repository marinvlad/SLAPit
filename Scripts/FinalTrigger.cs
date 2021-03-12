using System.Collections;
using System.Collections.Generic;
using Systems.Events;
using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    [SerializeField] private SimpleEvent _onLevelEnd;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {    
            _onLevelEnd.Invoke();
        }
    }
}
