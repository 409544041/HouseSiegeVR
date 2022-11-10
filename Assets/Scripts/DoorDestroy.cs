using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class DoorDestroy : MonoBehaviour
{
    
    /// <summary>
    /// Simple battering script
    /// if the lifes = 0 break the door enable the other thing and show particles
    /// </summary>
    //Welcome to the battering ram script
    public bool antispamTimer = true;
    [Header("Enter tag here of how it will be destroyed")]
    public string tag;
    [Header("Enter does it have a replacement?")]
    public bool HasReplacement;
    public GameObject replacement;
    [Header("Health of the door")]
    public int DoorHealth;

    public ParticleSystem particle;

    private void OnCollisionExit(Collision other)
    {
        if(!antispamTimer)
            return;
        if (other.gameObject.CompareTag(tag))
        {
            antispamTimer = false;
            DoorHealth--;
            AudioManager.instance.Play("Bonk");
            WaitExtension.Wait(this,1,antispam);
            
            if (DoorHealth <= 0)
                destroyDoor();
            
            
        }
    }

    public void destroyDoor()
    {
        AudioManager.instance.Play("Collapse");
        if (HasReplacement)
        {
            replacement.SetActive(true);
        }
        particle.Play();
        Destroy(gameObject);
    }

    public void antispam()
    {
        antispamTimer = true;
    }
}
