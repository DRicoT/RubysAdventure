using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public AudioClip damageAudioClip;
    private void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        if (controller != null)
        {
            controller.ChangeHealth(-1);
            Debug.Log("You have been damaged " + controller.health + "/" + controller.maxHealth);
            controller.PlaySound(damageAudioClip);
        }
    }
}
