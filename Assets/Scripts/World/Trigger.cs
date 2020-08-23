using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    private BoxCollider2D trigger;

    private void Awake()
    {
        trigger = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();

            if (player != null)
            {
                TriggerAction();
            }
        }
    }

    protected virtual void TriggerAction()
    {
        Debug.Log("Trigger");
        if (trigger != null)
        {
            trigger.enabled = false;
        }
    }
}
