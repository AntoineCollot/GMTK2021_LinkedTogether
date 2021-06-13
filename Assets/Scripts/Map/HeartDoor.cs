using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeartDoor : MonoBehaviour
{
    public UnityEvent onWin = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && KeyBunch.Instance.HasHeartKey)
            onWin.Invoke();
    }
}
